using UnityEngine;
using HarmonyLib;
using Shade.Extensions;
using UnboundLib;

namespace Shade.Patches
{
    [HarmonyPatch(typeof(HealthHandler))]
    internal class HealthHandler_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch("CallTakeDamage")]
        public static void HH_Prefix(HealthHandler __instance, ref Vector2 damage, GameObject damagingWeapon, Player damagingPlayer)
        {
            if (damagingPlayer != null &&
                ((Player)__instance.GetFieldValue("player")).teamID == damagingPlayer.teamID &&
                damagingWeapon != null &&
                damagingWeapon.GetComponent<Gun>().GenAdditionalData().teamDamageMultiplier != 1)
            {
                //UnityEngine.Debug.Log($"Damage before: {damage}, dmgMult: {damagingWeapon.GetComponent<Gun>().GenAdditionalData().teamDamageMultiplier}");
                damage *= damagingWeapon.GetComponent<Gun>().GenAdditionalData().teamDamageMultiplier;
                //UnityEngine.Debug.Log($"Damage after: {damage}");
            }
        }
    }
}
