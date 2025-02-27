using System.Linq;



public class Overpressure : DirectedExplosion
{
    protected override void Awake()
    {
        forwards = true;
        base.Awake();
        var existingEffects = player.GetComponentsInChildren<Overpressure>();
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
    //    Shade.Debug.Log($"Overpressure Angle: {angle}");
    //    return angle < 60f;
    //}
}
