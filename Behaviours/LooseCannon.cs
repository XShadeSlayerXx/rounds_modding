using System.Collections;
using System.Collections.Generic;
using ModsPlus;
using Shade.Extensions;
using UnityEngine;
using System.Linq;

public class LooseCannon : PlayerHook
{
    public float teamDmgMlt = 2f;
    public int numCards = 1;

    protected override void Start()
    {
        base.Start();

        gun.GenAdditionalData().teamDamageMultiplier *= teamDmgMlt;
    }

    protected override void Awake()
    {
        base.Awake();

        var existingEffects = player.GetComponentsInChildren<LooseCannon>();
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

        //gun.GenAdditionalData().teamDamageMultiplier /= teamDmgMlt;
        gun.GenAdditionalData().teamDamageMultiplier /= Mathf.Pow(teamDmgMlt, numCards);
    }
}
