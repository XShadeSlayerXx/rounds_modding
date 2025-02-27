using UnityEngine;
using ModsPlus;
using ModdingUtils.MonoBehaviours;
using System;

namespace Shade.Extensions
{

    public class Shade_StatChangeTracker
    {
        public bool active;
        internal Shade_TemporaryEffect effect;

        internal Shade_StatChangeTracker(Shade_TemporaryEffect effect)
        {
            this.effect = effect;
        }
    }

    [Serializable]
    public class Shade_StatChanges : StatChanges
    {
        // Health modifiers
        public float regen_add = 0f,
            regen_mult = 1f;

        // Character Stat modifiers
        public float decay_add = 0f,
            decay_mult = 1f,
            lifesteal_add = 0f,
            lifesteal_mult = 1f;
        public int respawns = 0;
            
        // Block modifiers
        public int additionalBlocks = 0;
        public float cdAdd_add = 0f,
            cdAdd_mult = 1f;

        // Gun modifiers
        public int bounces = 0;
        public float damage_add = 0,
            damageAfterDistanceMult_add = 0,
            damageAfterDistanceMult_mult = 1f,
            slow_add = 0,
            multiplySpread_mult = 1f,
            size_add = 0f,
            size_mult = 1f;
        public Color projectileColor = Color.black;

        // Gun Ammo modifiers
        public float reloadTime_add = 0f,
            reloadTime_mult = 1f;
    }

    public class Shade_TemporaryEffect : ReversibleEffect
    {
        private Shade_StatChanges statChanges;
        private Shade_StatChangeTracker status;

        public Shade_StatChangeTracker Initialize(Shade_StatChanges stats)
        {
            this.statChanges = stats;
            this.status = new Shade_StatChangeTracker(this);
            return status;
        }

        public override void OnStart()
        {
            healthHandlerModifier.regen_add = statChanges.regen_add;
            healthHandlerModifier.regen_mult = statChanges.regen_mult;

            characterStatModifiersModifier.secondsToTakeDamageOver_add = statChanges.decay_add;
            characterStatModifiersModifier.secondsToTakeDamageOver_mult = statChanges.decay_mult;
            characterStatModifiersModifier.lifeSteal_add = statChanges.lifesteal_add;
            characterStatModifiersModifier.lifeSteal_mult = statChanges.lifesteal_mult;
            characterStatModifiersModifier.respawns_add = statChanges.respawns;

            blockModifier.additionalBlocks_add = statChanges.additionalBlocks;
            blockModifier.cdAdd_add = statChanges.cdAdd_add;
            blockModifier.cdAdd_mult = statChanges.cdAdd_mult;

            gunAmmoStatModifier.reloadTimeAdd_add = statChanges.reloadTime_add;
            gunAmmoStatModifier.reloadTimeMultiplier_mult = statChanges.reloadTime_mult;

            gunStatModifier.reflects_add = statChanges.bounces;
            gunStatModifier.damage_add = statChanges.damage_add;
            gunStatModifier.damageAfterDistanceMultiplier_add = statChanges.damageAfterDistanceMult_add;
            gunStatModifier.damageAfterDistanceMultiplier_mult = statChanges.damageAfterDistanceMult_mult;
            gunStatModifier.slow_add = statChanges.slow_add;
            gunStatModifier.spread_mult = statChanges.multiplySpread_mult;
            gunStatModifier.size_add = statChanges.size_add;
            gunStatModifier.size_mult = statChanges.size_mult;
            gunStatModifier.projectileColor = statChanges.projectileColor;

            //from the original temporary effect class
            characterStatModifiersModifier.sizeMultiplier_mult = statChanges.PlayerSize;
            characterStatModifiersModifier.movementSpeed_mult = statChanges.MovementSpeed;
            characterStatModifiersModifier.jump_mult = statChanges.JumpHeight;

            characterDataModifier.numberOfJumps_add = statChanges.Jumps;
            characterDataModifier.maxHealth_mult = statChanges.MaxHealth;

            gravityModifier.gravityForce_mult = statChanges.PlayerGravity;

            gunStatModifier.numberOfProjectiles_add = statChanges.Bullets;
            gunStatModifier.spread_mult = statChanges.BulletSpread;
            gunStatModifier.attackSpeed_mult = statChanges.AttackSpeed;
            gunStatModifier.gravity_mult = statChanges.ProjectileGravity;
            gunStatModifier.damage_mult = statChanges.Damage;
            gunStatModifier.projectileSpeed_mult = statChanges.BulletSpeed;

            gunAmmoStatModifier.maxAmmo_add = statChanges.MaxAmmo;
            //end

            status.active = true;

        }

        public override void OnOnDestroy()
        {
            status.active = false;
        }
    }

}
