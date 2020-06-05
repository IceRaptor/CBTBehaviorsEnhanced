﻿using BattleTech;
using Localize;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using us.frostraptor.modUtils;

namespace CBTBehaviorsEnhanced.Objects
{
    public class KickMeleeState : MeleeState
    {
        // Per BT Manual pg.38,
        //   * target takes 1 pt. each 5 tons of attacker, rounded up
        //   *   x0.5 damage for each missing leg actuator
        //   * One attack
        //   * Normally resolves on kick table
        //   * -2 to hit base
        //   *   +1 for foot actuator, +2 to hit for each upper/lower actuator hit
        //   *   -2 modifier if target is prone

        public KickMeleeState(Mech attacker, Vector3 attackPos, AbstractActor target,
            HashSet<MeleeAttackType> validAnimations) : base(attacker)
        {
            this.Label = Mod.LocalizedText.Labels[ModText.LT_Label_Melee_Type_Kick];
            this.IsValid = ValidateAttack(attacker, target, validAnimations);
            if (IsValid)
            {
                CalculateDamages(attacker, target);
                CalculateInstability(attacker, target);
                CalculateModifiers(attacker, target);
                CreateDescriptions(attacker, target);

                // Damage tables 
                this.AttackerTable = DamageTable.NONE;
                this.TargetTable = DamageTable.KICK;

                // Unsteady
                this.ForceUnsteadyOnAttacker = false;
                this.ForceUnsteadyOnTarget = Mod.Config.Melee.Kick.AttackAppliesUnsteady;

                // Set the animation type
                if (target is Vehicle) this.AttackAnimation = MeleeAttackType.Stomp;
                else this.AttackAnimation = MeleeAttackType.Kick;
            }
        }

        private bool ValidateAttack(Mech attacker, AbstractActor target, HashSet<MeleeAttackType> validAnimations)
        {
            // If neither kick (mech) or stomp (vehicle) - we're not a valid attack.
            if (!validAnimations.Contains(MeleeAttackType.Kick) && !validAnimations.Contains(MeleeAttackType.Stomp))
            {
                Mod.Log.Info("Animations do not include a kick or stomp, cannot kick.");
                return false;
            }

            // Damage check - left leg
            if (!this.AttackerCondition.LeftHipIsFunctional || !this.AttackerCondition.RightHipIsFunctional)
            {
                Mod.Log.Info("One or more hip actuators are damaged. Cannot kick!");
                return false;
            }

            // If distance > walkSpeed, disable kick/physical weapon/punch
            float distance = (attacker.CurrentPosition - target.CurrentPosition).magnitude;
            float maxWalkSpeed = MechHelper.FinalWalkSpeed(attacker);
            if (distance > maxWalkSpeed)
            {
                Mod.Log.Info($"Attack distance of {distance} is greater than attacker walkSpeed: {maxWalkSpeed}. Cannot kick!");
                return false;
            }

            Mod.Log.Info("KICK ATTACK validated");
            return true;
        }

        private void CreateDescriptions(Mech attacker, AbstractActor target)
        {
            int sumTargetDamage = this.TargetDamageClusters.Count() > 0 ?
                (int)Math.Ceiling(this.TargetDamageClusters.Sum()) : 0;
            string localText = new Text(
                Mod.LocalizedText.AttackDescriptions[ModText.LT_AtkDesc_Kick_Desc],
                new object[] {
                })
                .ToString();

            this.DescriptionNotes.Add(localText);
        }

        private void CalculateModifiers(Mech attacker, AbstractActor target)
        {
            // -2 to hit base
            this.AttackModifiers.Add(ModText.LT_Label_Easy_to_Kick, Mod.Config.Melee.Kick.BaseAttackBonus);

            // If target is prone, -2 modifier
            if (target.IsProne) 
                this.AttackModifiers.Add(ModText.LT_Label_Target_Prone, Mod.Config.Melee.ProneTargetAttackModifier);

            // Actuator damage; +1 for foot actuator, +2 to hit for each upper/lower actuator hit
            int leftLegMalus = (2 - this.AttackerCondition.LeftLegActuatorsCount) * Mod.Config.Melee.Kick.LegActuatorDamageMalus;
            if (!this.AttackerCondition.LeftFootIsFunctional) leftLegMalus += Mod.Config.Melee.Kick.FootActuatorDamageMalus;

            int rightLegMalus = (2 - this.AttackerCondition.RightLegActuatorsCount) * Mod.Config.Melee.Kick.LegActuatorDamageMalus;
            if (!this.AttackerCondition.RightFootIsFunctional) rightLegMalus += Mod.Config.Melee.Kick.FootActuatorDamageMalus;

            int bestLegMalus = leftLegMalus >= rightLegMalus ? leftLegMalus : rightLegMalus;
            if (bestLegMalus != 0)
            {
                this.AttackModifiers.Add(ModText.LT_Label_Acutator_Damage, bestLegMalus);
            }
        }

        private void CalculateDamages(Mech attacker, AbstractActor target)
        {
            Mod.Log.Info($"Calculating kick damage for attacker: {CombatantUtils.Label(attacker)} " +
                $"vs. target: {CombatantUtils.Label(target)}");

            Mod.Log.Info($" - Attacker tonnage is: {attacker.tonnage}");

            float baseTargetDamage = (float)Math.Ceiling(Mod.Config.Melee.Kick.TargetDamagePerAttackerTon * attacker.tonnage);
            Mod.Log.Info($" - Target baseDamage: {Mod.Config.Melee.Kick.TargetDamagePerAttackerTon} x " +
                $"attacker tonnage: {attacker.tonnage} = {baseTargetDamage}");

            // Modifiers
            float damageMod = attacker.StatCollection.ContainsStatistic(ModStats.KickDamageMod) ?
                attacker.StatCollection.GetValue<float>(ModStats.KickDamageMod) : 0f;
            float damageMulti = attacker.StatCollection.ContainsStatistic(ModStats.KickDamageMulti) ?
                attacker.StatCollection.GetValue<float>(ModStats.KickDamageMulti) : 1f;

            // Leg actuator damage
            float leftLegReductionMulti = 1f;
            int damagedLeftActuators = 2 - this.AttackerCondition.LeftLegActuatorsCount;
            for (int i = 0; i < damagedLeftActuators; i++) leftLegReductionMulti *= Mod.Config.Melee.Kick.LegActuatorDamageReduction;
            Mod.Log.Info($" - Left leg actuator damage multi is: {leftLegReductionMulti}");

            float rightLegReductionMulti = 1f;
            int damagedRightActuators = 2 - this.AttackerCondition.RightLegActuatorsCount;
            for (int i = 0; i < damagedRightActuators; i++) rightLegReductionMulti *= Mod.Config.Melee.Kick.LegActuatorDamageReduction;
            Mod.Log.Info($" - Right leg actuator damage multi is: {rightLegReductionMulti}");

            float legReductionMulti = leftLegReductionMulti >= rightLegReductionMulti ? leftLegReductionMulti : rightLegReductionMulti;
            Mod.Log.Info($" - Using leg actuator damage multi of: {legReductionMulti}");

            // Roll up final damage
            float finalTargetDamage = (float)Math.Ceiling((baseTargetDamage + damageMod) * damageMulti * legReductionMulti);
            Mod.Log.Info($" - Target finalDamage: {finalTargetDamage} = (baseDamage: {baseTargetDamage} + damageMod: {damageMod}) x " +
                $"damageMulti: {damageMulti} x legReductionMulti: {legReductionMulti}");

            // Target damage applies as a single modifier
            this.TargetDamageClusters = new float[] { finalTargetDamage };
        }

        private void CalculateInstability(Mech attacker, AbstractActor target)
        {
            Mod.Log.Info($"Calculating instability for attacker: {CombatantUtils.Label(attacker)} " +
                $"vs. target: {CombatantUtils.Label(target)}");

            // TODO: Actuator damage should apply to instab as well
            this.TargetInstability = (float)Math.Ceiling(Mod.Config.Melee.Kick.TargetInstabilityPerAttackerTon * attacker.tonnage);
            Mod.Log.Info($" - Target takes {Mod.Config.Melee.Kick.TargetInstabilityPerAttackerTon} instability x " +
                $"target tonnage {attacker.tonnage} = {this.TargetInstability}");

        }
    }
}
