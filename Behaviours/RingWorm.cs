using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModsPlus;
using System.Linq;
using UnboundLib;

public class RingWorm : PlayerHook
{
    float amplitudeRandom = .1f;
    float amplitudeRandomUpgrade = .1f;

    float ringMult = 1.5f;
    int upgradeCount = 1;

    protected override void Start()
    {
        base.Start();

    }

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
        var ring = projectile.gameObject.GetOrAddComponent<RingBehaviour>();
        ring.cosMultiplier = upgradeCount * ringMult * (1 + newAmplitude) * gun.projectileSpeed * 2f;
        ring.sinMultiplier = upgradeCount * ringMult * (1 + newAmplitude);
        ring.period *= .8f;
    }

    public void Upgrade()
    {
        amplitudeRandom += amplitudeRandomUpgrade;
        upgradeCount++;
    }
}
