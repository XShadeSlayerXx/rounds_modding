using System.Collections;
using ModsPlus;
using UnboundLib.GameModes;
using UnityEngine;
using Shade.Extensions;

public class SC_Booster : SupportClass
{
    protected override void updateStats()
    {
        teamStatEffects = new Shade_StatChanges
        {
            //// 30% + this player's gun base dmg / 2. minimum 1.3x
            //MaxHealth = .3f + Mathf.Max(gun.damage, 110f) / 110f

            //10% + half of this player's gun dmg
            MaxHealth = .1f + gun.damage / 2,
        };
    }
}
