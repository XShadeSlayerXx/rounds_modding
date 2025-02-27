using System.Collections;
using System.Collections.Generic;
using ModsPlus;
using UnboundLib.GameModes;
using UnityEngine;
using Shade.Extensions;

public class SC_Power : SupportClass
{
    protected override void updateStats()
    {
        statDuration = 5f;
        teamStatEffects = new Shade_StatChanges
        {
            //// 50% + this player's gun base dmg / 2. minimum 150% increase
            //Damage = .5f + Mathf.Max(gun.damage, 110f) / 110f

            //20% damage buff + half of the shooter's damage
            Damage = 1.2f,
            damage_add = gun.damage / 2
        };
    }
}
