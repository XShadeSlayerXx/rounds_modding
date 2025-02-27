using System.Collections;
using System.Collections.Generic;
using ModsPlus;
using UnboundLib.GameModes;
using UnityEngine;
using Shade.Extensions;

public class SC_AntiGrav : SupportClass
{
    protected override void updateStats()
    {
        teamStatEffects = new Shade_StatChanges
        {
            PlayerGravity = .5f
        };
    }
}
