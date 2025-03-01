using System.Collections;
using System.Collections.Generic;
using Shade.Extensions;
using UnityEngine;

public class SC_Revive : SupportClass
{
    protected override void updateStats()
    {
        statDuration = 5f;
        statCooldown = 10f;
        canApplyMultipleTimes = false;
        // why did they forget to reset "remainingRespawns" in ModdingUtils.Extensions.TemporaryModifiers.cs
        teamStatEffects = new Shade_StatChanges
        {
                respawns = 1
        };
    }

}
