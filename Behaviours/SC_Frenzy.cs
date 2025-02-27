using System.Collections;
using System.Collections.Generic;
using ModsPlus;
using UnboundLib.GameModes;
using UnityEngine;
using Shade.Extensions;

public class SC_Frenzy : SupportClass
{
    protected override void updateStats()
    {
        statDuration = 10f;
        teamStatEffects = new Shade_StatChanges
        {
            AttackSpeed = 1.5f,
            MovementSpeed = 1.5f,
            JumpHeight = 1.3f,
            PlayerSize = .75f,
            reloadTime_mult = .8f
        };
    }
}
