using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ModsPlus;
using Photon.Pun.Simple;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LaserPointer : PlayerHook
{
    private LineRenderer lr;
    private LineRenderer sanityCheck;
    public int sanity_positions = 5;
    public int fidelity = 55;
    private int debug_counter = 0;

    public float bullet_gravity = 30f;

    public Vector2 ArenaBounds = new Vector2(100, 100);

    protected override void Awake()
    {
        base.Awake();
        lr = Instantiate(Shade.ShadeCards.assets.LoadAsset<GameObject>("Shade_Laser")).GetComponent<LineRenderer>();
        lr.enabled = true;
        lr.positionCount = fidelity;
        sanityCheck = Instantiate(Shade.ShadeCards.assets.LoadAsset<GameObject>("Shade_Laser")).GetComponent<LineRenderer>();
        sanityCheck.startColor = Color.green;
        sanityCheck.endColor = Color.cyan;
        sanityCheck.positionCount = sanity_positions;
        sanityCheck.startWidth = .15f;
        sanityCheck.endWidth = .15f;
        sanityCheck.enabled = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if ((bool)player && lr.enabled && player.data.view.IsMine)
        {
            lr.SetPositions(Points().ToArray());

            
            Vector3 facing_direction = MainCam.instance.cam.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
            facing_direction.z = 0f;
            facing_direction.Normalize();
            Vector3 position = player.transform.position + facing_direction * 2f;

            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < sanity_positions; i++)
            {
                points.Add(position + facing_direction * i);
            }
            //Shade.Debug.Log(string.Join(", ",points));
            sanityCheck.SetPositions(points.ToArray());
        }
    }

    public override IEnumerator OnShootCoroutine(GameObject projectile)
    {
        var proj = projectile.GetComponent<MoveTransform>();
        this.ExecuteAfterFrames(1, () =>
        {
            bullet_gravity = proj.gravity * proj.multiplier;
            Shade.Debug.Log($"Velocity: {proj.velocity.magnitude}. Gravity: {proj.gravity}. Drag: {proj.drag}");
        });
        return base.OnShootCoroutine(projectile);
    }

    public override IEnumerator OnPointStart(IGameModeHandler gameModeHandler)
    {
        lr.enabled = true;
        return base.OnPointStart(gameModeHandler);
    }

    public override IEnumerator OnPointEnd(IGameModeHandler gameModeHandler)
    {
        lr.enabled = false;
        return base.OnPointEnd(gameModeHandler);
    }

    protected override void OnDestroy()
    {
        lr.enabled = false;
        base.OnDestroy();
        Destroy(lr);
    }

    protected List<Vector3> Points()
    {
        // get the player's normalized facing direction
        Vector3 facing_direction = MainCam.instance.cam.ScreenToWorldPoint(Input.mousePosition) - base.transform.position;
        facing_direction.z = 0f;
        facing_direction.Normalize();
        //Vector3 facing_direction = gun.transform.forward;

        // and the parameters necessary to determine height
        float proj_speed = gun.projectileSpeed * (55f - Vector2.Angle(facing_direction, Vector3.up)/18); //proj speed decreases by 10 when looking down
        float gravity = bullet_gravity * 30f; //30f is the default gravity in MoveTransform.gravity

        // separate the x and y components because x_delta is constant, y_delta is not
        float angle = Vector2.Angle(facing_direction, Vector3.up); // take the angle from y-axis instead of x-axis to avoid 0 && 180 degree asymptotes
        float x_speed = proj_speed * Mathf.Sin(angle * Mathf.Deg2Rad) * (facing_direction.x > 0 ? 1 : -1);
        float y_speed = proj_speed * Mathf.Cos(angle * Mathf.Deg2Rad);

        Vector3 velocity = new Vector3(x_speed, y_speed);

        //float x_offsets_distance = x_speed;
        Vector3 origin = gun.transform.position;
        Vector3 position = player.transform.position + facing_direction * 2f;

        List<Vector3> points = new List<Vector3>();
        float y_next = position.y;
        float seconds_to_emulate = 1;
        for (int i = 0; i < proj_speed * seconds_to_emulate; i++)
        {
            float current_time_step = i / proj_speed;
            // simulate a plot
            //y_next = y_speed * i * current_time_step - gravity * Mathf.Pow(i * current_time_step, 1.5f);
            points.Add(origin + new Vector3(velocity.x * current_time_step, y_next, 0));
            y_next = origin.y + y_speed * current_time_step - .5f * gravity * Mathf.Pow(current_time_step, 2f);

            // simulate velocity
            //points.Add(position);
            //Shade.Debug.Log(velocity);
            //velocity += Vector3.down * gravity / proj_speed;
            //position += velocity / proj_speed;
        }
        if (++debug_counter > 500)
        {
            debug_counter = 0;
            Shade.Debug.Log($"Speed: {proj_speed}. X:{x_speed}, Y:{y_speed}");
            Shade.Debug.Log($"Laser origin: {position}");
            Shade.Debug.Log($"Laser angle: {angle}");
            string path_string = "";
            foreach (var point in points)
            {
                path_string += point.ToString();
            }
            Shade.Debug.Log($"Laser Path ({points.Count}): {path_string}");
        }
        return points;
    }
}
