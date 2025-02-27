using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModsPlus;
using Shade.Extensions;
using UnityEngine;

public class Exp_Toxin : PlayerHook
{
    public float statDuration = 7f;
    public int effectStrength = 2;
    List<Shade_StatChanges> usableEffects = new List<Shade_StatChanges>();

    Shade_StatChanges[] temporaryEffects = new Shade_StatChanges[]
    {
        new Shade_StatChanges { AttackSpeed = .75f },
        new Shade_StatChanges { MovementSpeed = .75f },
        new Shade_StatChanges { BulletSpeed = .5f },
        new Shade_StatChanges { BulletSpread = 1.5f },
        new Shade_StatChanges { Damage = .75f },
        new Shade_StatChanges { JumpHeight = .75f },
        new Shade_StatChanges { MaxHealth = .75f },
        new Shade_StatChanges { MaxAmmo = -1 },
        new Shade_StatChanges { PlayerGravity = 1.5f },
        new Shade_StatChanges { ProjectileGravity = 1.5f },
        new Shade_StatChanges { projectileColor = Color.red },
        new Shade_StatChanges { reloadTime_add = .25f },
        new Shade_StatChanges { reloadTime_mult = 1.1f },
        new Shade_StatChanges { size_add = 2 },
        new Shade_StatChanges { slow_add = .5f },
    };

    public Shade_StatChanges random_status()
    {
        if (usableEffects.Count == 0)
        {
            usableEffects = temporaryEffects.ToList();
        }
        int index = UnityEngine.Random.Range(0, usableEffects.Count);
        var effect = usableEffects.ElementAt(index);
        usableEffects.RemoveAt(index);

        return effect;
    }

    protected override void Awake()
    {
        base.Awake();
        var existingEffects = player.GetComponentsInChildren<Exp_Toxin>();
        if (existingEffects.Length > 1)
        {
            // upgrade the preexisting effect
            existingEffects.First(e => e != this).Upgrade();

            // destroy this effect
            Destroy(gameObject);
        }
    }

    public void Upgrade()
    {
        statDuration += 10f;
        effectStrength++;
    }

    public override IEnumerator OnBulletHitCoroutine(GameObject projectile, HitInfo hit)
    {
        if (hit.collider.gameObject.GetComponentInChildren<Player>() &&
            hit.collider.gameObject.GetComponentInChildren<Player>() != null)
        {
            Player other = hit.collider.gameObject.GetComponentInChildren<Player>();
            Shade_StatChanges random_stat = random_status();
            List<Shade_StatChangeTracker> effects = new List<Shade_StatChangeTracker>();
            for (int i = 0; i < effectStrength;  i++)
            {
                effects.Add(SupportClass.Apply(other, random_stat));
            }
            Shade.Debug.Log($"D20: Tried to apply: {effects}");
            yield return new WaitForSeconds(statDuration);
            Shade.Debug.Log("D20: effect ended");
            foreach (var effect in effects)
            {
                SupportClass.Remove(effect);
            }
        }
    }
}
