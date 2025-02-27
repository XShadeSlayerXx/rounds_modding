using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModsPlus;
using Photon.Pun.Simple;
using UnityEngine;

public class Relativity : PlayerHook
{
    float maxDamageMult = 1.5f;
    float maxReloadRate = 2f;
    const float bulletSpeedCutoff = 1f;

    const float upgradeDamage = 1.5f;
    const float upgradeReload = 2f;

    protected override void Awake()
    {
        base.Awake();
        var existingEffects = player.GetComponentsInChildren<Relativity>();
        if (existingEffects.Length > 1)
        {
            // upgrade the preexisting effect
            existingEffects.First(e => e != this).Upgrade();

            // destroy this effect
            Destroy(gameObject);
        }
    }

    void Upgrade()
    {
        maxDamageMult += upgradeDamage;
        maxReloadRate += upgradeReload;
    }

    public override void OnShoot(GameObject projectile)
    {
        if (gun)
        {
            ProjectileHit hit = projectile.GetComponent<ProjectileHit>();
            var spd = gun.projectielSimulatonSpeed * gun.projectileSpeed;
            if (spd < 1)
            {
                hit.dealDamageMultiplierr += Mathf.Min((bulletSpeedCutoff - spd / 2) * maxDamageMult, maxDamageMult);
            }
            else
            {
                gun.sinceAttack += gun.defaultCooldown / Mathf.Min((spd - bulletSpeedCutoff) * maxReloadRate, maxReloadRate);
            }
        }
    }
}
