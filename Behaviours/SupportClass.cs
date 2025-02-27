using System.Collections;
using ModsPlus;
using UnityEngine;
using Shade.Extensions;
using UnboundLib.GameModes;
using Shade;

public class SupportClass : PlayerHook
{
    //float numCards = 1;
    [SerializeField]
    protected float teamDmgMlt = .5f;
    
    [SerializeField]
    protected float statDuration = 15f;

    [SerializeField]
    protected float statCooldown = 0f;

    //[SerializeField]
    protected Shade_StatChanges teamStatEffects;// = new StatChanges();

    [SerializeField]
    protected bool canApplyMultipleTimes = true;

    protected bool canApplyEffect = true;

    protected override void Start()
    {
        base.Start();

        gun.GenAdditionalData().teamDamageMultiplier *= teamDmgMlt;
        updateStats();
    }

    //protected override void Awake()
    //{
    //    base.Awake();

        //var existingEffects = player.GetComponentsInChildren<SupportClass>();
        //if (existingEffects.Length > 1)
        //{
        //    // upgrade the preexisting effect
        //    existingEffects.First(e => e != this).Upgrade();

        //    // destroy this effect
        //    Destroy(gameObject);
        //}
    //}

    //public void Upgrade()
    //{
    //    numCards++;

    //    gun.GenAdditionalData().teamDamageMultiplier *= teamDmgMlt;
    //}

    protected override void OnDestroy()
    {
        base.OnDestroy();

        gun.GenAdditionalData().teamDamageMultiplier /= teamDmgMlt;
        //gun.GenAdditionalData().teamDamageMultiplier /= Mathf.Pow(teamDmgMlt, numCards);
    }

    public override IEnumerator OnBulletHitCoroutine(GameObject projectile, HitInfo hit)
    {
        if (canApplyEffect &&
            hit.collider.gameObject.GetComponentInChildren<Player>() &&
            hit.collider.gameObject.GetComponentInChildren<Player>() != null &&
            hit.collider.gameObject.GetComponentInChildren<Player>().teamID == player.teamID)
        {
            Player other = hit.collider.gameObject.GetComponentInChildren<Player>();
            Shade_StatChangeTracker effect = Apply(other, teamStatEffects);
            Shade.Debug.Log($"{teamStatEffects} => {teamStatEffects.MyToString()}\nRevives: {characterStats.respawns}");
            canApplyEffect = canApplyMultipleTimes;
            yield return new WaitForSeconds(statDuration);
            Remove(effect);
            Shade.Debug.Log($"finished {teamStatEffects}\nRevives: {characterStats.respawns}");
            yield return new WaitForSeconds(statCooldown);
            canApplyEffect = true;
        }
    }

    public static Shade_StatChangeTracker Apply(Player player, Shade_StatChanges stats)
    {
        var effect = player.gameObject.AddComponent<Shade_TemporaryEffect>();
        return effect.Initialize(stats);
    }


    public static void Remove(Shade_StatChangeTracker status)
    {
        Shade.Debug.Log(status.active);
        if (!status.active) return;
        UnityEngine.Object.Destroy(status.effect);
    }

    public override IEnumerator OnPickPhaseEnd(IGameModeHandler gameModeHandler)
    {
        updateStats();
        return base.OnPickPhaseEnd(gameModeHandler);
    }

    protected virtual void updateStats() { }
}

public static class StatChanges_Extension
{
    public static string MyToString(this StatChanges stats)
    {
        string info =
            "\n" +
            $"Bullets:      {stats.Bullets}\n" +
            $"Jumps:        {stats.Jumps}\n" +
            $"Max Ammo:     {stats.MaxAmmo}\n" +
            "-------------\n" +
            $"AttackSpeed:  {stats.AttackSpeed} \n" +
            $"PlayerGravity:{stats.PlayerGravity} \n" +
            $"MoveSpeed:    {stats.MovementSpeed} \n" +
            $"ProjGravity:  {stats.ProjectileGravity} \n" +
            $"Damage:       {stats.Damage} \n" +
            $"PlayerSize:   {stats.PlayerSize} \n" +
            $"MaxHealth:    {stats.MaxHealth} \n" +
            $"BulletSpread: {stats.BulletSpread} \n" +
            $"BulletSpeed:  {stats.BulletSpeed} \n" +
            $"JumpHeight:   {stats.JumpHeight} \n";



        return info;
    }
}