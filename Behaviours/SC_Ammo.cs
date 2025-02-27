using System.Collections;
using System.Collections.Generic;
using ModsPlus;
using UnboundLib.GameModes;
using UnityEngine;
using Shade.Extensions;

public class SC_Ammo : SupportClass
{
    protected override void updateStats()
    {
        canApplyMultipleTimes = true;
        teamStatEffects = new Shade_StatChanges
        {
            MaxAmmo = 2,
            reloadTime_mult = .75f,
            damageAfterDistanceMult_mult = 1.2f,
            bounces = 2
        };
    }
}
