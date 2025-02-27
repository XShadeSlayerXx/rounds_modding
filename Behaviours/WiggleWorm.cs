using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModsPlus;
using System.Linq;
using UnboundLib;
using Photon.Pun;

public class WiggleWorm : PlayerHook
{
    float amplitudeRandom = .1f;
    float amplitudeRandomUpgrade = .1f;

    float cosMult = 1.5f;
    int upgradeCount = 1;

    protected override void Awake()
    {
        base.Awake();
        var existingEffects = player.GetComponentsInChildren<WiggleWorm>();
        if (existingEffects.Length > 1)
        {
            // upgrade the preexisting effect
            existingEffects.First(e => e != this).Upgrade();

            // destroy this effect
            Destroy(gameObject);
        }
    }

    public override void OnShoot(GameObject projectile)
    {
        base.OnShoot(projectile);

        float newAmplitude = Random.Range(-amplitudeRandom, amplitudeRandom);
        projectile.gameObject.GetOrAddComponent<Cos>().multiplier = upgradeCount * cosMult * (1 + newAmplitude);
    }

    public void Upgrade()
    {
        amplitudeRandom += amplitudeRandomUpgrade;
        upgradeCount++;
    }
}
