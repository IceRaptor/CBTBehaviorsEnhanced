﻿using BattleTech;
using Harmony;
using IRBTModUtils.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTBehaviorsEnhanced.Helper
{
    public static class UnitHelper
    {

        // TODO: EVERYTHING SHOULD CONVERT TO CACHED CALL IF POSSIBLE
        public static BehaviorVariableValue GetBehaviorVariableValue(BehaviorTree bTree, BehaviorVariableName name)
        {

            BehaviorVariableValue bhVarVal = null;
            if (ModState.RolePlayerBehaviorVarManager != null && ModState.RolePlayerGetBehaviorVar != null)
            {
                // Ask RolePlayer for the variable
                //getBehaviourVariable(AbstractActor actor, BehaviorVariableName name)
                Mod.Log.Trace?.Write($"Pulling BehaviorVariableValue from RolePlayer for unit: {bTree.unit.DistinctId()}.");
                bhVarVal = (BehaviorVariableValue)ModState.RolePlayerGetBehaviorVar.Invoke(ModState.RolePlayerBehaviorVarManager, new object[] { bTree.unit, name });
            }

            if (bhVarVal == null)
            {
                // RolePlayer does not return the vanilla value if there's no configuration for the actor. We need to check that we're null here to trap that edge case.
                // Also, if RolePlayer isn't configured we need to read the value. 
                Mod.Log.Trace?.Write($"Pulling BehaviorVariableValue from Vanilla for unit: {bTree.unit.DistinctId()}.");
                bhVarVal = GetBehaviorVariableValueDirectly(bTree, name);
            }

            Mod.Log.Trace?.Write($"  Value is: {bhVarVal}");
            return bhVarVal;

        }

        private static BehaviorVariableValue GetBehaviorVariableValueDirectly(BehaviorTree bTree, BehaviorVariableName name)
        {
            BehaviorVariableValue behaviorVariableValue = bTree.unitBehaviorVariables.GetVariable(name);
            if (behaviorVariableValue != null)
            {
                return behaviorVariableValue;
            }

            Pilot pilot = bTree.unit.GetPilot();
            if (pilot != null)
            {
                BehaviorVariableScope scopeForAIPersonality = bTree.unit.Combat.BattleTechGame.BehaviorVariableScopeManager.GetScopeForAIPersonality(pilot.pilotDef.AIPersonality);
                if (scopeForAIPersonality != null)
                {
                    behaviorVariableValue = scopeForAIPersonality.GetVariableWithMood(name, bTree.unit.BehaviorTree.mood);
                    if (behaviorVariableValue != null)
                    {
                        return behaviorVariableValue;
                    }
                }
            }

            if (bTree.unit.lance != null)
            {
                behaviorVariableValue = bTree.unit.lance.BehaviorVariables.GetVariable(name);
                if (behaviorVariableValue != null)
                {
                    return behaviorVariableValue;
                }
            }

            if (bTree.unit.team != null)
            {
                Traverse bvT = Traverse.Create(bTree.unit.team).Field("BehaviorVariables");
                BehaviorVariableScope bvs = bvT.GetValue<BehaviorVariableScope>();
                behaviorVariableValue = bvs.GetVariable(name);
                if (behaviorVariableValue != null)
                {
                    return behaviorVariableValue;
                }
            }

            UnitRole unitRole = bTree.unit.DynamicUnitRole;
            if (unitRole == UnitRole.Undefined)
            {
                unitRole = bTree.unit.StaticUnitRole;
            }

            BehaviorVariableScope scopeForRole = bTree.unit.Combat.BattleTechGame.BehaviorVariableScopeManager.GetScopeForRole(unitRole);
            if (scopeForRole != null)
            {
                behaviorVariableValue = scopeForRole.GetVariableWithMood(name, bTree.unit.BehaviorTree.mood);
                if (behaviorVariableValue != null)
                {
                    return behaviorVariableValue;
                }
            }

            if (bTree.unit.CanMoveAfterShooting)
            {
                BehaviorVariableScope scopeForAISkill = bTree.unit.Combat.BattleTechGame.BehaviorVariableScopeManager.GetScopeForAISkill(AISkillID.Reckless);
                if (scopeForAISkill != null)
                {
                    behaviorVariableValue = scopeForAISkill.GetVariableWithMood(name, bTree.unit.BehaviorTree.mood);
                    if (behaviorVariableValue != null)
                    {
                        return behaviorVariableValue;
                    }
                }
            }

            behaviorVariableValue = bTree.unit.Combat.BattleTechGame.BehaviorVariableScopeManager.GetGlobalScope().GetVariableWithMood(name, bTree.unit.BehaviorTree.mood);
            if (behaviorVariableValue != null)
            {
                return behaviorVariableValue;
            }

            return DefaultBehaviorVariableValue.GetSingleton();
        }
    }
}
