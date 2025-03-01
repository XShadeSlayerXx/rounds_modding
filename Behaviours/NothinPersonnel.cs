using System.Collections;
using System.Collections.Generic;
using ModdingUtils.AIMinion.Patches;
using ModsPlus;
using UnboundLib.Extensions;
using UnityEngine;

public class NothinPersonnel : PlayerHook
{
    public float maxCooldown = 5f;
    protected float currentCooldown = 0;

    //on block, teleport behind the nearest enemy player
    public override void OnBlock(BlockTrigger.BlockTriggerType blockTriggerType)
    {
        if (currentCooldown < Time.time)
        {
            base.OnBlock(blockTriggerType);
            Player other = PlayerManager.instance.GetClosestPlayerInOtherTeam(player.transform.position, player.teamID);
            if (other != null)
            {
                currentCooldown = maxCooldown + Time.time;
                GetComponentInParent<PlayerCollision>().IgnoreWallForFrames(2);
                Vector3 position = other.transform.position + other.transform.forward;
                player.transform.position = position;
            }
            else
            {
                Shade.Debug.Log("Nothin Personnel: No other player found?");
            }
        }
    }
}
