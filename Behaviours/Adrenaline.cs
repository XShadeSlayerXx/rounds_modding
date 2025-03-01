using System.Collections;
using System.Collections.Generic;
using ModsPlus;
using UnityEngine;

public class Adrenaline : PlayerHook
{
    public float HP_amount,
        damage_amount,
        shot_speed_amount;
    StatChanges stat_changes;
    StatChangeTracker stat_changes_tracker;

    protected override void Start()
    {
        stat_changes = new StatChanges
        {
            MaxHealth = HP_amount,
            Damage = damage_amount,
            BulletSpeed = shot_speed_amount,
        };
        base.Start();
    }


    public override void OnReloadDone(int bulletsReloaded)
    {
        StatManager.Remove(stat_changes_tracker);
        base.OnReloadDone(bulletsReloaded);
    }

    public override void OnOutOfAmmo(int bulletsReloaded)
    {
        stat_changes_tracker = StatManager.Apply(player, stat_changes);
        base.OnOutOfAmmo(bulletsReloaded);
    }
}
