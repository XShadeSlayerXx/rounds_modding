using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModsPlus;
using UnboundLib;
using UnityEngine;

public class Martyrdom : PlayerHook
{
    Explosion explosion;
    float minDamage = 80f;
    int damageMult = 1;

    protected override void Awake()
    {
        base.Awake();
        var existingEffects = player.GetComponentsInChildren<Martyrdom>();
        if (existingEffects.Length > 1)
        {
            // upgrade the preexisting effect
            existingEffects.First(e => e != this).Upgrade();

            // destroy this effect
            Destroy(gameObject);
        }

    }

    protected override void Start()
    {
        base.Start();

        var explosive = Resources.Load<GameObject>("0 cards/Explosive Bullet").GetComponent<Gun>().objectsToSpawn[0];
        GameObject explosion_effect = Instantiate(explosive.effect);
        explosion_effect.hideFlags = HideFlags.HideAndDontSave;
        explosion_effect.name = "A_SHADE_MartyrExplosion";

        Destroy(explosion_effect.GetComponent<RemoveAfterSeconds>());
        explosion_effect.AddComponent<SpawnedAttack>().spawner = player;

        explosion = explosion_effect.GetComponent<Explosion>();
        explosion.scaleDmg = true;
        explosion.scaleRadius = true;
        explosion.scaleForce = true;
    }

    public void Upgrade()
    {
        damageMult += 1;
    }

    public override void OnTakeDamage(Vector2 damage, bool selfDamage)
    {
        if (player.data.health < 0f)
        {
            BlowUp();
        }
    }

    public void BlowUp()
    {
        Vector3 where = player.transform.position;
        float sizeMult = player.transform.localScale.x;
        float damage = gun.damage * gun.damage;

        explosion.damage = Mathf.Max(damage, minDamage) * damageMult;
        explosion.range = 40f;
        explosion.dmgColor = Color.yellow;

        Instantiate(explosion.gameObject, where, Quaternion.identity).transform.localScale = Vector3.one * sizeMult;
    }
}