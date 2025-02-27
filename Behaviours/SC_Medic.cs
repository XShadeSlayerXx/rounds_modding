using System.Collections;
using System.Collections.Generic;
using ModsPlus;
using UnboundLib.GameModes;
using UnityEngine;
using Shade.Extensions;

public class SC_Medic : SupportClass
{
    protected override void updateStats()
    {
        teamStatEffects = new Shade_StatChanges
        {
            MaxHealth = 1.5f,
            regen_add = 3,
            //projectileColor = Color.green
        };
    }
}
