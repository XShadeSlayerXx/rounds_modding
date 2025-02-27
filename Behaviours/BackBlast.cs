using System.Linq;



public class BackBlast : DirectedExplosion
{
    protected override void Awake()
    {
        forwards = false;
        base.Awake();
        var existingEffects = player.GetComponentsInChildren<BackBlast>();
        if (existingEffects.Length > 1)
        {
            // upgrade the preexisting effect
            existingEffects.First(e => e != this).Upgrade();

            // destroy this effect
            Destroy(gameObject);
        }
    }

    //protected override bool angleIsCorrect(float angle)
    //{
    //    Shade.Debug.Log($"Backblast Angle: {angle}");
    //    return angle > 120f && angle < 170f;
    //}
}
