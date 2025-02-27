using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Shade.Extensions;
using System;
using UnboundLib;
using static Shade.Patches.Gun_Patch;

namespace Shade.Patches
{

    [HarmonyPatch(typeof(Gun))]
    internal class Gun_Patch
    {
        [HarmonyPatch("ApplyProjectileStats")]
        public static void Prefix(Gun __instance, ref float damageM)
        {
            if (__instance.GenAdditionalData().variableDamage > 0)
            {
                var amt = __instance.GenAdditionalData().variableDamage;
                damageM *= Mathf.Max(UnityEngine.Random.Range(-amt, 1+amt), UnityEngine.Random.Range(-amt, 1+amt));
                //UnityEngine.Debug.Log($"Damage Mult should be: {damageM}");
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch("ApplyProjectileStats")]
        public static void APS_Postfix(Gun __instance, GameObject obj)
        {
            if (__instance.GenAdditionalData().cosAmplitude > 0)
            {
                var comp = obj.GetComponent<Cos>();
                if (comp != null)
                {
                    comp.multiplier = __instance.GenAdditionalData().cosAmplitude;
                }
            }
        }

        internal class gunStats
        {
            public int numberOfProjectiles = 0;
            public int attacks = 0;
            public int bursts = 0;
            public ProjectilesToSpawn[] projectilesToSpawn;
            //public int projectiles = 0;

            public gunStats(int numberOfProjectiles, int attacks, int bursts, ProjectilesToSpawn[] projectilesToSpawn)
            {
                this.numberOfProjectiles = numberOfProjectiles;
                this.attacks = attacks;
                this.bursts = bursts;
                this.projectilesToSpawn = projectilesToSpawn;
                //this.projectiles = projectiles;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("DoAttacks")]
        public static void DoAttacks_Prefix(ref Gun __instance, int attacks)
        {
            if (__instance.GenAdditionalData().ammoToAddBeforeFiring > 0) {
                GunAmmo gunAmmo = __instance.GetComponentInChildren<GunAmmo>();
                int currentAmmo = (int)gunAmmo.GetFieldValue("currentAmmo");
                gunAmmo.SetFieldValue("currentAmmo", currentAmmo + __instance.GenAdditionalData().ammoToAddBeforeFiring);
            }
        }



        [HarmonyPostfix]
        [HarmonyPatch("Attack")]
        public static void Attack_Postfix(ref Gun __instance, float charge, bool __result)
        {
            if (__instance.useCharge)
            {
                //UnityEngine.Debug.Log($"Result: {__result}, Charge: {charge}, CurrentCharge: {__instance.currentCharge}");
                __instance.StopCharging();
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("ResetStats")]
        private static void ResetStats_Prefix(Gun __instance)
        {
            __instance.GenAdditionalData().variableDamage = 0;
            __instance.GenAdditionalData().charging = false;
        }
    }

    [HarmonyPatch(typeof(WeaponHandler))]
    internal class WeaponHandler_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Attack")]
        public static void Attack_Prefix(WeaponHandler __instance, CharacterData ___data)//, out gunStats __state)
        {
            //__state = null;
            if (__instance &&
                ___data &&
                __instance.gun &&
                __instance.gun.useCharge &&
                !__instance.gun.GenAdditionalData().charging &&
                ___data.input.shootIsPressed &&
                !___data.dead &&
                (bool)typeof(PlayerVelocity).GetField("simulated", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField).GetValue(___data.playerVel) &&
                0 < (int)typeof(GunAmmo).GetField("currentAmmo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField).GetValue(((Component)__instance.gun).GetComponentInChildren<GunAmmo>())
            )
                if (__instance.gun.useCharge)
                {
                    __instance.gun.StartCharging();
                }
        }
    }
}