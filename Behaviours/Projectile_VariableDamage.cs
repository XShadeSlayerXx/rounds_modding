using UnityEngine;
using Shade.Extensions;
using Photon.Pun;

public class Projectile_VariableDamage : RayHitEffect
{
    MoveTransform move;
    //SyncProjectile sync;
    public float variableDamageMin;
    public float variableDamageMax;
    SpawnedAttack spawned;
    public bool normalizeDamage = true;

    // I'll re-add this after implementing RPCA_CallHeal
    //float chanceToHeal = 0f;

    private void Start()
    {
        this.move = base.GetComponentInParent<MoveTransform>();
        base.GetComponentInParent<ProjectileHit>().bulletCanDealDeamage = false;
        //this.sync = base.GetComponentInParent<SyncProjectile>();
        //this.sync.active = true;
        spawned = base.GetComponentInParent<SpawnedAttack>();
    }

    public override HasToReturn DoHitEffect(HitInfo hit)
    {
        if (!hit.transform)
        {
            return (HasToReturn)1;
        }
        ProjectileHit componentInParent = base.GetComponentInParent<ProjectileHit>();
        HealthHandler health = hit.transform.GetComponent<HealthHandler>();
        if (health && spawned.IsMine())
        {
            float damageMult = Random.Range(variableDamageMin, variableDamageMax);
            if (normalizeDamage)
            {
                damageMult = Mathf.Max(damageMult, Random.Range(variableDamageMin, variableDamageMax));
            }
            //if (chanceToHeal > 0 && Random.Range(0, 1f) < chanceToHeal)
            //{
                //...is there a CallHeal...?
                //damageMult *= -1;
            //}
            health.CallTakeDamage(damageMult * componentInParent.damage * base.transform.forward, base.transform.position, base.GetComponentInParent<ProjectileHit>().ownWeapon, base.GetComponentInParent<ProjectileHit>().ownPlayer, true);
        }
        return (HasToReturn)1;
    }
}