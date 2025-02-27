using System;
using System.Runtime.CompilerServices;

namespace Shade.Extensions
{
    // ADD FIELDS TO GUN
    [Serializable]
    public class GunAdditionalData
    {
        public float variableDamage;
        public bool charging;
        public int ammoToAddBeforeFiring;
        public float cosAmplitude;
        public float teamDamageMultiplier;
        public GunAdditionalData()
        {
            variableDamage = 0;
            charging = false;
            ammoToAddBeforeFiring = 0;
            cosAmplitude = 0;
            teamDamageMultiplier = 1f;
        }
    }
    public static class GunExtension
    {
        public static readonly ConditionalWeakTable<Gun, GunAdditionalData> data =
            new ConditionalWeakTable<Gun, GunAdditionalData>();

        public static GunAdditionalData GenAdditionalData(this Gun gun)
        {
            return data.GetOrCreateValue(gun);
        }

        public static void AddData(this Gun gun, GunAdditionalData value)
        {
            try
            {
                data.Add(gun, value);
            }
            catch (Exception) { }
        }

        public static void StartCharging(this Gun gun)
        {
            gun.GenAdditionalData().charging = true;
        }

        public static void StopCharging(this Gun gun)
        {
            gun.GenAdditionalData().charging = false;
        }
    }
    // apply additional projectile stats
    //[HarmonyPatch(typeof(Gun), "ApplyProjectileStats")]
    //class GunPatchApplyProjectileStats
    //{
    //    private static void Prefix(Gun __instance, GameObject obj, int numOfProj = 1, float damageM = 1f, float randomSeed = 0f)
    //    {
    //        MoveTransform component3 = obj.GetComponent<MoveTransform>();
    //        component3.allowStop = __instance.GetAdditionalData().allowStop;
    //    }
    //}
    // reset extra gun attributes when resetstats is called
}