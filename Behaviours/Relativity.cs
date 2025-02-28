using System.Linq;
using ModsPlus;
using UnityEngine;

public class Relativity : PlayerHook
{
    float maxDamageMult = 1.5f;
    float maxReloadRate = 2f;
    const float bulletSpeedCutoff = 1f;

    const float upgradeDamage = 1.5f;
    const float upgradeReload = 2f;

    StatChanges statChanges = null;
    StatChangeTracker statChangeTracker = null;

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
        if (statChanges == null)
        {
            UpdateStats();
        }
    }

    void Upgrade()
    {
        maxDamageMult += upgradeDamage;
        maxReloadRate += upgradeReload;
        UpdateStats();
    }

    void UpdateStats()
    {
        if (statChangeTracker != null) { StatManager.Remove(statChangeTracker); }
        var spd = gun.projectielSimulatonSpeed * gun.projectileSpeed;
        statChanges = new StatChanges
        {
            Damage = spd > 1 ? Mathf.Min((bulletSpeedCutoff - spd / 2) * maxDamageMult, maxDamageMult) : 1,
            AttackSpeed = spd < 1 ? gun.defaultCooldown / Mathf.Min((spd - bulletSpeedCutoff) * maxReloadRate, maxReloadRate) : 1,
        };
        statChangeTracker = StatManager.Apply(player, statChanges);
    }
}
