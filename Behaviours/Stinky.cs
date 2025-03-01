using System.Collections;
using ModsPlus;
using UnityEngine;
using UnboundLib;
using System.Linq;
using Shade;
using UnboundLib.GameModes;
using System.Collections.Generic;

public class Stinky : PlayerHook
{
    int maxStacks = 3;
    float currentStacks = 0f;

    float movespeedPerStack = .33f;
    float jumpHeightPerStack = .1f;
    //float stackDuration = 6f;
    List<StatChangeTracker> effects = new List<StatChangeTracker>();

    GameObject cloud;

    protected override void Start()
    {
        base.Start();

        cloud = ShadeCards.assets.LoadAsset<GameObject>("Shade_Cloud");
    }

    protected override void Awake()
    {
        base.Awake();
        var existingEffects = player.GetComponentsInChildren<Stinky>();
        if (existingEffects.Length > 1)
        {
            // upgrade the preexisting effect
            existingEffects.First(e => e != this).Upgrade();

            // destroy this effect
            Destroy(gameObject);
        }
    }

    public override IEnumerator OnBattleStart(IGameModeHandler gameModeHandler)
    {
        foreach (var effect in effects)
        {
            StatManager.Remove(effect);
        }
        currentStacks = 0f;
        return base.OnBattleStart(gameModeHandler);
    }

    public override IEnumerator OnOutOfAmmoCoroutine(int bulletsReloaded)
    {
        if (currentStacks < maxStacks)
        {
            //gain 1 stack per full reload. do not go over maxStacks
            float stacksToGain = Mathf.Min(bulletsReloaded / gunAmmo.maxAmmo, maxStacks - currentStacks);
            currentStacks += stacksToGain;

            float movementToGain = 1 + stacksToGain * movespeedPerStack;
            float jumpToGain = 1 + stacksToGain * jumpHeightPerStack;

            var statsToChange = new StatChanges
            {
                MovementSpeed = movementToGain,
                JumpHeight = jumpToGain
            };

            var effect = StatManager.Apply(player, statsToChange);
            effects.Add(effect);
            // create the green cloud
            var obj = Instantiate(cloud, player.transform, false);
            obj.transform.position = player.transform.position;
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule why = ps.main;

            // play the sound
            ShadeCards.playSoundFromAsset("balloon", player.transform, .6f);

            yield return null;
        }
    }

    public void Upgrade()
    {
        maxStacks *= 2;
        //stackDuration *= 1.2f;
    }
}