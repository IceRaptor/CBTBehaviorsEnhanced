﻿using BattleTech;
using CBTBehaviorsEnhanced.Helper;
using FluffyUnderware.DevTools.Extensions;
using Harmony;
using System.Linq;
using UnityEngine;
using us.frostraptor.modUtils;

namespace CBTBehaviorsEnhanced.Melee {


    [HarmonyPatch(typeof(MechMeleeSequence), "BuildMeleeDirectorSequence")]
    [HarmonyBefore("io.mission.modrepuation")]
    static class MechMeleeSequence_BuildMeleeDirectorSequence
    {
        static void Prefix(MechMeleeSequence __instance)
        {
            // TODO: If this happens before the above... need to grab the selected melee type from state
            Mod.Log.Info($"Setting current melee type to: {__instance.selectedMeleeType} and weapon to: {__instance.OwningMech.MeleeWeapon}");
            ModState.MeleeWeapon = __instance.OwningMech.MeleeWeapon;
            ModState.MeleeType = __instance.selectedMeleeType;

            if (ModState.MeleeStates?.SelectedState != null)
            {
                ModState.MeleeType = ModState.MeleeStates.SelectedState.AttackAnimation;

                // Modify the owning mech melee weapon to do the 'first' hit
                float targetDamage = ModState.MeleeStates.SelectedState?.TargetDamageClusters?.Length > 0 ?
                    ModState.MeleeStates.SelectedState.TargetDamageClusters[0] : 0;
                ModState.MeleeWeapon.StatCollection.Set<float>(ModStats.HBS_Weapon_DamagePerShot, targetDamage);
                ModState.MeleeWeapon.StatCollection.Set<float>(ModStats.HBS_Weapon_Instability, ModState.MeleeStates.SelectedState.TargetInstability);
                Mod.Log.Info($"For {CombatantUtils.Label(__instance.OwningMech)} set melee weapon damage: {targetDamage}  and instability: {ModState.MeleeStates.SelectedState.TargetInstability}");
            }
        }

        static void Postfix(MechMeleeSequence __instance)
        {
            ModState.MeleeType = MeleeAttackType.NotSet;
            ModState.MeleeWeapon = null;
        }
    }


    [HarmonyPatch(typeof(MechMeleeSequence), "OnMeleeComplete")]
    [HarmonyBefore("io.mission.modrepuation")]
    static class MechMeleeSequence_OnMeleeComplete
    {
        static void Prefix(MechMeleeSequence __instance, MessageCenterMessage message, AttackStackSequence ___meleeSequence)
        {
            Mod.Log.Trace("MMS:OMC entered.");

            AttackCompleteMessage attackCompleteMessage = message as AttackCompleteMessage;
            Mod.Log.Info($"== Resolving cluster damage, instability, and unsteady on melee attacker: {CombatantUtils.Label(__instance.OwningMech)} and target: {CombatantUtils.Label(__instance.MeleeTarget)}.");
            if (attackCompleteMessage.stackItemUID == ___meleeSequence.SequenceGUID && ModState.MeleeStates?.SelectedState != null)
            {
                if (!__instance.OwningMech.IsOrWillBeProne)
                {
                    // Target stability and unsteady - always applies as we're always a mech
                    if (ModState.MeleeStates.SelectedState.ForceUnsteadyOnAttacker)
                    {
                        Mod.Log.Info(" -- Forcing attacker to become unsteady from attack!");
                        __instance.OwningMech.ApplyUnsteady();
                    }
                    if (ModState.MeleeStates.SelectedState.AttackerInstability != 0)
                    {
                        Mod.Log.Info($" -- Adding {ModState.MeleeStates.SelectedState.AttackerInstability} absolute instability to attacker.");
                        __instance.OwningMech.AddAbsoluteInstability(ModState.MeleeStates.SelectedState.AttackerInstability, StabilityChangeSource.Attack, "-1");
                    }
                }

                // Attacker cluster damage
                if (!__instance.OwningMech.IsDead)
                {
                    if (ModState.MeleeStates.SelectedState.AttackerDamageClusters.Length > 0)
                    {
                        Mod.Log.Info($" -- Applying {ModState.MeleeStates.SelectedState.AttackerDamageClusters.Sum()} damage to attacker as {ModState.MeleeStates.SelectedState.AttackerDamageClusters.Length} clusters.");
                        AttackHelper.CreateImaginaryAttack(__instance.OwningMech, __instance.OwningMech, __instance.SequenceGUID,
                            ModState.MeleeStates.SelectedState.AttackerDamageClusters, ModState.MeleeStates.SelectedState.AttackAnimation);
                    }
                }

                // Target stability and unsteady - only applies to mech targets
                if (__instance.MeleeTarget is Mech targetMech && !targetMech.IsProne)
                {
                    if (ModState.MeleeStates.SelectedState.ForceUnsteadyOnTarget)
                    {
                        Mod.Log.Info(" -- Forcing target to become unsteady from attack!");
                        targetMech.ApplyUnsteady();
                    }
                    if (ModState.MeleeStates.SelectedState.TargetInstability != 0)
                    {
                        Mod.Log.Info($" -- Adding {ModState.MeleeStates.SelectedState.TargetInstability} absolute instability to target.");
                        targetMech.AddAbsoluteInstability(ModState.MeleeStates.SelectedState.TargetInstability, StabilityChangeSource.Attack, "-1");
                    }
                }

                // Target cluster damage - first attack was applied through melee weapon
                if (ModState.MeleeStates.SelectedState.TargetDamageClusters.Length > 1 && !__instance.MeleeTarget.IsDead)
                {
                    // The target already got hit by the first cluster as the weapon damage. Only add the additional hits
                    float[] clusterDamage = ModState.MeleeStates.SelectedState.TargetDamageClusters.SubArray(1, ModState.MeleeStates.SelectedState.TargetDamageClusters.Length);
                    Mod.Log.Info($" -- Applying {clusterDamage.Sum()} damage to target as {clusterDamage.Length} clusters.");
                    AttackHelper.CreateImaginaryAttack(__instance.OwningMech, __instance.MeleeTarget, __instance.SequenceGUID, clusterDamage, 
                        ModState.MeleeStates.SelectedState.AttackAnimation);
                }

                Mod.Log.Info($"== Done.");
            }

            // Reset melee state
            ModState.MeleeStates = null;
        }
    }

    [HarmonyPatch(typeof(MechMeleeSequence), "CompleteOrders")]
    [HarmonyBefore("io.mission.modrepuation")]
    static class MechMeleeSequence_CompleteOrders
    {
        static void Postfix(MechMeleeSequence __instance)
        {
            Mod.Log.Trace("MMS:CO - entered.");

            // Base method checks for target knockdown. Do the same for the attacker.
            if (!__instance.OwningMech.IsDead)
            {
                __instance.OwningMech.CheckForInstability();
                __instance.OwningMech.HandleKnockdown(__instance.RootSequenceGUID, __instance.owningActor.GUID, Vector2.one, null);
            }


            // Invalidate our melee state as we're done
            ModState.MeleeStates = null;
            ModState.MeleeType = MeleeAttackType.NotSet;
            ModState.MeleeWeapon = null;
        }
    }
}
