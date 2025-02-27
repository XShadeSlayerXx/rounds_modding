using System.Collections;
using System.Collections.Generic;
using ModsPlus;
using UnityEngine;
using UnboundLib;
using System;
using System.Linq;
using Shade.Extensions;

public class Tests : PlayerHook
{
    float numCards = 1;
    float teamDmgMlt = .5f;
    float statDuration = 15f;

    protected override void Start()
    {
        base.Start();

        gun.GenAdditionalData().teamDamageMultiplier *= teamDmgMlt;
    }

    protected override void Awake()
    {
        base.Awake();
        var existingEffects = player.GetComponentsInChildren<Tests>();
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
        numCards++;

        gun.GenAdditionalData().teamDamageMultiplier *= teamDmgMlt;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        gun.GenAdditionalData().teamDamageMultiplier /= Mathf.Pow(teamDmgMlt, numCards);
    }

    //example bulletHitEffect for teammate stuff later
    public override IEnumerator OnBulletHitCoroutine(GameObject projectile, HitInfo hit)
    {
        if (hit.collider.gameObject.GetComponentInChildren<Player>() &&
            hit.collider.gameObject.GetComponentInChildren<Player>() != null &&
            hit.collider.gameObject.GetComponentInChildren<Player>().teamID == player.teamID)
        {
            var effect = StatManager.Apply(player, TeamStatEffects());
            yield return new WaitForSeconds(statDuration);
            StatManager.Remove(effect);
        }
    }

    protected virtual StatChanges TeamStatEffects() { return null; }
}

//public class TestEffect : CardEffect
//{

//protected override void OnDestroy()
//{
//    base.OnDestroy();
//    PlayerManager.instance.RemovePlayerDiedAction(AfterPlayerDiedBlowUp);
//}

//public override IEnumerator OnShootCoroutine(GameObject projectile)
//{
//    if (gun)
//    {
//        ProjectileHit hit = projectile.GetComponent<ProjectileHit>();
//        var spd = gun.projectielSimulatonSpeed * gun.projectileSpeed;
//        UnityEngine.Debug.Log(spd);
//    }
//    yield return null;
//}

// //example bulletHitEffect for teammate stuff later
//public override IEnumerator OnBulletHitCoroutine(GameObject projectile, HitInfo hit)
//{
//    if (hit.collider.gameObject.GetComponentInChildren<Player>() &&
//        hit.collider.gameObject.GetComponentInChildren<Player>() != null)
//    {
//        var effect = StatManager.Apply(player, new StatChanges { Bullets = -1 });
//        yield return new WaitForSeconds(5f);
//        StatManager.Remove(effect);
//    }
//}

//private void SafeStartCoroutine(IEnumerator coroutine)
//{
//    Unbound.Instance.StartCoroutine(ExecuteWhenPlayerActive(() => player.StartCoroutine(coroutine)));
//}

//private void SafeExecuteAction(Action action)
//{
//    Unbound.Instance.StartCoroutine(ExecuteWhenPlayerActive(action));
//}

//private IEnumerator ExecuteWhenPlayerActive(Action action)
//{
//    yield return new WaitUntil(() => player.gameObject.activeInHierarchy);
//    action?.Invoke();
//}
//}


//public class Template : PlayerHook
//{
//    //Explosion explosion;

//    protected override void Start()
//    {
//        base.Start();

//        //  example adding an explosion from a pre-existing card in the base game
//        //var explosive = Resources.Load<GameObject>("0 cards/Explosive Bullet").GetComponent<Gun>().objectsToSpawn[0];
//        //GameObject explosion_effect = Instantiate(explosive.effect);
//        //explosion_effect.hideFlags = HideFlags.HideAndDontSave;
//        //explosion_effect.name = "A_SHADE_MartyrExplosion";

//        //Destroy(explosion_effect.GetComponent<RemoveAfterSeconds>());
//        //explosion_effect.AddComponent<SpawnedAttack>().spawner = player;

//        //explosion = explosion_effect.GetComponent<Explosion>();
//        //explosion.scaleDmg = true;
//        //explosion.scaleRadius = true;
//        //explosion.scaleForce = true;
//    }

//    protected override void Awake()
//    {
//        base.Awake();
//        var existingEffects = player.GetComponentsInChildren<Template>();
//        if (existingEffects.Length > 1)
//        {
//            // upgrade the preexisting effect
//            existingEffects.First(e => e != this).Upgrade();

//            // destroy this effect
//            Destroy(gameObject);
//        }
//    }

//    public void Upgrade()
//    {
//        // do stuff to upgrade the effect here
//    }
//}