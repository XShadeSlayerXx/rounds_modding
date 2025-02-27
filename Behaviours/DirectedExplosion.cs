using UnityEngine;
using ModsPlus;
using UnboundLib;
using Photon.Pun;



public class DirectedExplosion : PlayerHook
{
    public float blastRange = 7f;
    public float blastForce = 10000f;
    public float blastForceMult = 1f;
    public float blastDamage = 40;
    public float blastDamageMult = 1f;

    public float blastStun = 0f;

    public bool scaleRadius = true;
    public bool ignoreWalls = false;

    protected LineEffect lineEffect;
    protected GameObject explosive;
    protected GameObject blastParticles;

    protected bool forwards;

    protected override void Start()
    {
        base.Start();

        lineEffect = GetComponentInChildren<LineEffect>(true);
        explosive = Shade.ShadeCards.assets.LoadAsset<GameObject>("Shade_DirectedExplosion");
        blastParticles = Shade.ShadeCards.assets.LoadAsset<GameObject>("Shade_BurstParticles");
        lineEffect = Shade.ShadeCards.assets.LoadAsset<GameObject>("Shade_LineEffect").GetComponent<LineEffect>();
    }

    public void Upgrade()
    {
        blastDamageMult += 1;
        blastForceMult += .5f;
        blastStun += 1;
    }

    public override void OnShoot(GameObject projectile)
    {
        base.OnShoot(projectile);

        Emit_DirectedExplosion(projectile);
    }
    protected void Emit_DirectedExplosion(GameObject projectile)
    {
        var explode = Instantiate(explosive, player.transform);
        var s_explode = explode.GetComponent<Shade_Explosion>();
        s_explode.spawned = explode.GetComponent<SpawnedAttack>();
        s_explode.spawned.spawner = player;
        s_explode.view = explode.GetComponent<PhotonView>();
        float gun_dmg_cap = Mathf.Clamp(gun.damage, .2f, 1);
        this.ExecuteAfterFrames(1, () =>
        {
            if ((bool)projectile)
            {
                Shade.Debug.Log($"Blast - Damage Total: {blastDamage * blastDamageMult * gun_dmg_cap}. Force Total: {blastForce * blastForceMult * gun_dmg_cap}");
                s_explode.damage = blastDamage * blastDamageMult * gun_dmg_cap;
                s_explode.force = blastForce * blastForceMult * gun_dmg_cap;
                s_explode.range = blastRange;// * gun_dmg_cap;
                s_explode.particles = blastParticles;
                s_explode.Explode(player, projectile.transform.forward,lineEffect,forwards);
            }
        });
    }
}
