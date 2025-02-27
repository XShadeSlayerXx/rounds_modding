using System.Collections;
using System.Collections.Generic;
using Shade.Extensions;
using UnityEngine;

public class SC_Lifesteal : SupportClass
{

    public override IEnumerator OnBulletHitCoroutine(GameObject projectile, HitInfo hit)
    {
        if (canApplyEffect &&
            hit.collider.gameObject.GetComponentInChildren<Player>() &&
            hit.collider.gameObject.GetComponentInChildren<Player>() != null &&
            hit.collider.gameObject.GetComponentInChildren<Player>().teamID == player.teamID)
        {
            Player other = hit.collider.gameObject.GetComponentInChildren<Player>();
            Shade.Debug.Log($"{teamStatEffects} => {teamStatEffects.MyToString()}");
            Shade_StatChangeTracker effect = Apply(other, teamStatEffects);
            Shade_StatChangeTracker myEffect = Apply(player, new Shade_StatChanges { lifesteal_add = -characterStats.lifeSteal });
            canApplyEffect = canApplyMultipleTimes;
            
            yield return new WaitForSeconds(statDuration);
            
            Shade.Debug.Log($"finished {teamStatEffects}");
            Remove(effect);
            Remove(myEffect);
            canApplyEffect = true;
        }
    }
    protected override void updateStats()
    {
        canApplyMultipleTimes = false;
        teamStatEffects = new Shade_StatChanges
        {
            lifesteal_add = characterStats.lifeSteal
        };
    }
}
