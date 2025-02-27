using System.Collections;
using System.Collections.Generic;
//using ModdingUtils.Extensions;
using UnityEngine;
using GunChargePatch;
using GunChargePatch.Extensions;
using System.Reflection;
using Shade.Extensions;
using UnboundLib;

public class Railgun : MonoBehaviour
{
    Player player;
    Gun gun;
    GunAmmo gunAmmo;
    Coroutine thereCanOnlyBeOne = null;

    //current ammo that is consumed
    //internal float chargeLevel; //reset to 1
    //internal float chargeAmt;
    //internal int chargeThresh; //reset to 0
    internal float startChargeAmmo;
    //speed multiplier on fireRate
    internal float chargeSpeed  = 10f;
    //maxCharge based on ammo count
    internal float maxCharge = .34f;

    internal float previousMaxAmmo = 0;
    internal bool previouslyCharging = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
        gun = player.data.weaponHandler.gun;
        gunAmmo = gun.GetComponentInChildren<GunAmmo>();
        updateChargeInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player || !player.data.view.IsMine || !gun || !gun.useCharge) { return; }
        //UnityEngine.Debug.Log($"Charge: {gun.currentCharge}");
        if (previouslyCharging != gun.GenAdditionalData().charging)
        {
            previouslyCharging = gun.GenAdditionalData().charging;
            if (previouslyCharging) //just started charging
            {
                //UnityEngine.Debug.Log("----charging start");
                StartCharging();
            }
            else
            {
                //UnityEngine.Debug.Log("-------charging stop");
                StopCoroutine(thereCanOnlyBeOne);
                //TODO: Pulse the player's sprite a color
            }
        }
    }

    void StartCharging()
    {
        if (thereCanOnlyBeOne != null) StopCoroutine(thereCanOnlyBeOne);
        if (gunAmmo.maxAmmo !=  previousMaxAmmo)
        {
            updateChargeInfo();
        }
        float waitForCharge = gun.GetAdditionalData().chargeTime / gun.GetAdditionalData().maxCharge;
        thereCanOnlyBeOne = StartCoroutine(KeepCharging(waitForCharge));
    }

    IEnumerator KeepCharging(float timeBetweenCharges)
    {
        while (gun.currentCharge < gun.GetAdditionalData().maxCharge && 0 < (int)gunAmmo.GetFieldValue("currentAmmo"))
        {
            yield return new WaitForSeconds(timeBetweenCharges);
            chargeBullet();
        }
    }

   // __instance.gun.currentCharge = Mathf.Clamp(
   //     __instance.gun.currentCharge + 
   //     TimeHandler.deltaTime / __instance.gun.GetAdditionalData().chargeTime* __instance.gun.GetAdditionalData().maxCharge, 
   //     0f, __instance.gun.GetAdditionalData().maxCharge);

void chargeBullet()
    {
        //int currentAmmo = (int)typeof(GunAmmo)
        //    .GetField("currentAmmo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField)
        //    .GetValue(gun.GetComponentInChildren<GunAmmo>());
        int currentAmmo = (int)gunAmmo.GetFieldValue("currentAmmo");
        gunAmmo.SetFieldValue("currentAmmo", currentAmmo-1);
        gunAmmo.SetFieldValue("freeReloadCounter", 0);
        gunAmmo.SetFieldValue("reloadCounter", (gunAmmo.reloadTime + gunAmmo.reloadTimeAdd) * gunAmmo.reloadTimeMultiplier);
        //UnityEngine.Debug.Log($"Ammo: {gun.ammo}");
        //gunAmmo.currentAmmo--;
        gunAmmo.InvokeMethod("SetActiveBullets", false);
        //UnityEngine.Debug.Log($"Ammo: {currentAmmo-1}");
    }

    void updateChargeInfo()
    {
        //UnityEngine.Debug.Log("charging update info");

        startChargeAmmo = gun.ammo;
        previousMaxAmmo = gunAmmo.maxAmmo;
        gun.GetAdditionalData().maxCharge = Mathf.Max(maxCharge * previousMaxAmmo, 1);
        gun.GetAdditionalData().chargeTime = chargeSpeed * gun.attackSpeed * gun.attackSpeedMultiplier / previousMaxAmmo * gun.GetAdditionalData().maxCharge;
        //chargeAmt = gun.attackSpeed * gun.attackSpeedMultiplier / previousMaxAmmo;
    }
}
