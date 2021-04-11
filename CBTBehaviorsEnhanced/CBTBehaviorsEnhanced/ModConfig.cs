﻿

using BattleTech;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBTBehaviorsEnhanced
{

    public class ModConfig
    {

        public bool Debug = false;
        public bool Trace = false;

        // Movement - should be a +3 per BT Manual pg. 28
        public int ToHitSelfJumped = 3;

        public DevOpts Developer = new DevOpts();
        public FeatureList Features = new FeatureList();
        public CustomCategoryOpts CustomCategories = new CustomCategoryOpts();
        public HeatOptions Heat = new HeatOptions();
        public MeleeOptions Melee = new MeleeOptions();
        public MoveOptions Move = new MoveOptions();
        public PilotingOptions Piloting = new PilotingOptions();
        public SkillCheckOptions SkillChecks = new SkillCheckOptions();
        public BiomeBreachOptions Breaches = new BiomeBreachOptions();

        public void LogConfig()
        {
            Mod.Log.Info?.Write("=== MOD CONFIG BEGIN ===");
            Mod.Log.Info?.Write($"  Debug: {this.Debug} Trace: {this.Trace}");
            Mod.Log.Info?.Write($"  FEATURES => BiomeBreaches: {this.Features.BiomeBreaches}  " +
                $"PermanentEvasion: {this.Features.PermanentEvasion}  " +
                $"StartupChecks: {this.Features.StartupChecks}");
            Mod.Log.Info?.Write($"  ToHitSelfJumped: {this.ToHitSelfJumped}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("=== CUSTOM CATEGORY OPTIONS ===");
            string shoulderCats = String.Join(",", this.CustomCategories.ShoulderActuatorCategoryId);
            string upperArmCats = String.Join(",", this.CustomCategories.UpperArmActuatorCategoryId);
            string lowerArmCats = String.Join(",", this.CustomCategories.LowerArmActuatorCategoryId);
            string handCats = String.Join(",", this.CustomCategories.HandActuatorCategoryId);
            Mod.Log.Info?.Write($"  ShoulderActuatorCategoryId: '{shoulderCats}'  UpperArmActuatorCategoryId: '{upperArmCats}'  " +
                $"LowerArmActuatorCategoryId: '{lowerArmCats}'  HandActuatorCategoryId: '{handCats}'  ");

            string hipCats = String.Join(",", this.CustomCategories.HipActuatorCategoryId);
            string upperLegCats = String.Join(",", this.CustomCategories.UpperLegActuatorCategoryId);
            string lowerLegCats = String.Join(",", this.CustomCategories.LowerLegActuatorCategoryId);
            string footCats = String.Join(",", this.CustomCategories.FootActuatorCategoryId);
            Mod.Log.Info?.Write($"  HipActuatorCategoryId: '{hipCats}'  UpperLegActuatorCategoryId: '{upperLegCats}'  " +
                $"LowerLegActuatorCategoryId: '{lowerLegCats}'  FootActuatorCategoryId: '{footCats}'  ");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("=== HEAT OPTIONS ===");
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, int> kvp in this.Heat.Movement)
            {
                sb.Append($"{kvp.Key} : {kvp.Value}, ");
            }
            Mod.Log.Info?.Write($"  Movement mods: {sb}");
            sb.Clear();

            foreach (KeyValuePair<int, int> kvp in this.Heat.Firing)
            {
                sb.Append($"{kvp.Key} : {kvp.Value}, ");
            }
            Mod.Log.Info?.Write($"  Firing mods: {sb}");
            sb.Clear();

            foreach (KeyValuePair<int, float> kvp in this.Heat.Shutdown)
            {
                sb.Append($"{kvp.Key} : {kvp.Value}, ");
            }
            Mod.Log.Info?.Write($"  Shutdown chances: {sb}");
            sb.Clear();

            foreach (KeyValuePair<int, float> kvp in this.Heat.Explosion)
            {
                sb.Append($"{kvp.Key} : {kvp.Value}, ");
            }
            Mod.Log.Info?.Write($"  Ammo Explosion chances: {sb}");
            sb.Clear();

            foreach (KeyValuePair<int, float> kvp in this.Heat.PilotInjury)
            {
                sb.Append($"{kvp.Key} : {kvp.Value}, ");
            }
            Mod.Log.Info?.Write($"  Pilot Injury chances: {sb}");
            sb.Clear();

            foreach (KeyValuePair<int, float> kvp in this.Heat.SystemFailures)
            {
                sb.Append($"{kvp.Key} : {kvp.Value}, ");
            }
            Mod.Log.Info?.Write($"  System Failure chances: {sb}");
            sb.Clear();

            Mod.Log.Info?.Write($"  ShowLowOverheatAnim: {this.Heat.ShowLowOverheatAnim}  ShowExtremeOverheatAnim: {this.Heat.ShowExtremeOverheatAnim}");
            Mod.Log.Info?.Write($"  ShutdownFallThreshold: {this.Heat.ShutdownFallThreshold}");
            Mod.Log.Info?.Write($"  MaxHeat: {this.Heat.MaxHeat}  WarnAtHeat: {this.Heat.WarnAtHeat}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("=== MELEE OPTIONS ===");
            Mod.Log.Info?.Write($"  ExtraHitsAverageAllDamage: {this.Melee.ExtraHitsAverageAllDamage}  ProneTargetAttackModifier: {this.Melee.ProneTargetAttackModifier}  " +
                $"FilterCanUseInMeleeWeaponsByAttack: {this.Melee.FilterCanUseInMeleeWeaponsByAttack}");

            Mod.Log.Info?.Write("  -- CHARGE OPTIONS --");
            Mod.Log.Info?.Write($"  AttackerDamagePerTargetTon: {this.Melee.Charge.AttackerDamagePerTargetTon}  AttackerInstabilityPerTargetTon: {this.Melee.DFA.AttackerInstabilityPerTargetTon}");
            Mod.Log.Info?.Write($"  TargetDamagePerAttackerTon: {this.Melee.Charge.TargetDamagePerAttackerTon}  TargetInstabilityPerAttackerTon: {this.Melee.DFA.TargetInstabilityPerAttackerTon}");
            Mod.Log.Info?.Write($"  DamageClusterDivisor: {this.Melee.Charge.DamageClusterDivisor}");
            Mod.Log.Info?.Write($"  Unsteady => AttackerOnHit: {this.Melee.Charge.UnsteadyAttackerOnHit}  AttackerOnMiss: {this.Melee.Charge.UnsteadyAttackerOnMiss}  TargetOnHit: {this.Melee.Charge.UnsteadyTargetOnHit}");
            Mod.Log.Info?.Write($"  TargetVehicleEvasionPipsRemoved: {this.Melee.Charge.TargetVehicleEvasionPipsRemoved}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("  -- DFA OPTIONS --");
            Mod.Log.Info?.Write($"  AttackerDamagePerTargetTon: {this.Melee.DFA.AttackerDamagePerTargetTon}  AttackerInstabilityPerTargetTon: {this.Melee.DFA.AttackerInstabilityPerTargetTon}");
            Mod.Log.Info?.Write($"  TargetDamagePerAttackerTon: {this.Melee.DFA.TargetDamagePerAttackerTon}  TargetInstabilityPerAttackerTon: {this.Melee.DFA.TargetInstabilityPerAttackerTon}");
            Mod.Log.Info?.Write($"  DamageClusterDivisor: {this.Melee.DFA.DamageClusterDivisor}");
            Mod.Log.Info?.Write($"  Unsteady => AttackerOnHit: {this.Melee.DFA.UnsteadyAttackerOnHit}  AttackerOnMiss: {this.Melee.DFA.UnsteadyAttackerOnMiss}  TargetOnHit: {this.Melee.DFA.UnsteadyTargetOnHit}");
            Mod.Log.Info?.Write($"  TargetVehicleEvasionPipsRemoved: {this.Melee.DFA.TargetVehicleEvasionPipsRemoved}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("  -- KICK OPTIONS --");
            Mod.Log.Info?.Write($"  BaseAttackBonus: {this.Melee.Kick.BaseAttackBonus}  LegActuatorDamageMalus: {this.Melee.Kick.LegActuatorDamageMalus}  FootActuatorDamageMalus: {this.Melee.Kick.FootActuatorDamageMalus}");
            Mod.Log.Info?.Write($"  TargetDamagePerAttackerTon: {this.Melee.Kick.TargetDamagePerAttackerTon}  TargetInstabilityPerAttackerTon: {this.Melee.Kick.TargetInstabilityPerAttackerTon}");
            Mod.Log.Info?.Write($"  LegActuatorDamageReduction: {this.Melee.Kick.LegActuatorDamageReduction}");
            Mod.Log.Info?.Write($"  Unsteady => AttackerOnHit: {this.Melee.Kick.UnsteadyAttackerOnHit}  AttackerOnMiss: {this.Melee.Kick.UnsteadyAttackerOnMiss}  TargetOnHit: {this.Melee.Kick.UnsteadyTargetOnHit}");
            Mod.Log.Info?.Write($"  TargetVehicleEvasionPipsRemoved: {this.Melee.Kick.TargetVehicleEvasionPipsRemoved}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("  -- PHYSICAL WEAPON OPTIONS --");
            Mod.Log.Info?.Write($"  DefaultDamagePerAttackTon: {this.Melee.PhysicalWeapon.DefaultDamagePerAttackerTon}  DefaultInstabilityPerAttackerTon: {this.Melee.PhysicalWeapon.DefaultInstabilityPerAttackerTon}");
            Mod.Log.Info?.Write($"  ArmActuatorDamageMalus: {this.Melee.PhysicalWeapon.ArmActuatorDamageMalus}");
            Mod.Log.Info?.Write($"  Unsteady Default => AttackerOnHit: {this.Melee.PhysicalWeapon.DefaultUnsteadyAttackerOnHit}  AttackerOnMiss: {this.Melee.PhysicalWeapon.DefaultUnsteadyAttackerOnMiss}  TargetOnHit: {this.Melee.PhysicalWeapon.DefaultUnsteadyTargetOnHit}");
            Mod.Log.Info?.Write($"  TargetVehicleEvasionPipsRemoved: {this.Melee.PhysicalWeapon.TargetVehicleEvasionPipsRemoved}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("  -- PUNCH OPTIONS --");
            Mod.Log.Info?.Write($"  TargetDamagePerAttackerTon: {this.Melee.Punch.TargetDamagePerAttackerTon}  TargetInstabilityPerAttackerTon: {this.Melee.Punch.TargetInstabilityPerAttackerTon}");
            Mod.Log.Info?.Write($"  ArmActuatorDamageMalus: {this.Melee.Punch.ArmActuatorDamageMalus}  HandActuatorDamageMalus: {this.Melee.Punch.HandActuatorDamageMalus}");
            Mod.Log.Info?.Write($"  ArmActuatorDamageReduction: {this.Melee.Punch.ArmActuatorDamageReduction}");
            Mod.Log.Info?.Write($"  Unsteady => AttackerOnHit: {this.Melee.Punch.UnsteadyAttackerOnHit}  AttackerOnMiss: {this.Melee.Punch.UnsteadyAttackerOnMiss}  TargetOnHit: {this.Melee.Punch.UnsteadyTargetOnHit}");
            Mod.Log.Info?.Write($"  TargetVehicleEvasionPipsRemoved: {this.Melee.Punch.TargetVehicleEvasionPipsRemoved}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("  -- SWARM OPTIONS --");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("=== MOVE OPTIONS ===");
            Mod.Log.Info?.Write($"  MinimumMove: {this.Move.MinimumMove}m  HeatMovePenalty: {this.Move.HeatMovePenalty}m  RunMulti: x{this.Move.RunMulti}");
            Mod.Log.Info?.Write($"  FallAfterChances =>   Jump: {this.Move.FallAfterJumpChance}  Run: {this.Move.FallAfterRunChance}");
            Mod.Log.Info?.Write($"  MPMetersPerHex: {this.Move.MPMetersPerHex}m");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("=== PILOTING OPTIONS ===");
            Mod.Log.Info?.Write($"  StabilityCheck: {this.Piloting.StabilityCheck}");
            Mod.Log.Info?.Write($"  DFAReductionMulti: x{this.Piloting.DFAReductionMulti}  FallingDamagePerTenTons: {this.Piloting.FallingDamagePerTenTons}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("=== SKILL CHECKS OPTIONS ===");
            Mod.Log.Info?.Write($"  PilotingSkillMulti: x{this.SkillChecks.ModPerPointOfPiloting}  GutsSkillMulti: {this.SkillChecks.ModPerPointOfGuts}");


            Mod.Log.Info?.Write("=== BREACHES OPTIONS ===");
            Mod.Log.Info?.Write($"  ThinAtmoCheck: {this.Breaches.ThinAtmoCheck}  VacuumCheck: {this.Breaches.VacuumCheck}");
            Mod.Log.Info?.Write("");

            Mod.Log.Info?.Write("=== MOD CONFIG END ===");
        }

        // Newtonsoft seems to merge values into existing dictionaries instead of replacing them entirely. So instead
        //   populate default values in dictionaries through this call instead
        public void InitUnsetValues()
        {
            // == Init Heat ==

            // 5:-1, 10:-2, 15:-3, 20:-4, 25:-5, 31:-6, 37:-7, 43:-8, 49:-9
            if (this.Heat.Movement.Count == 0)
            {
                this.Heat.Movement = new SortedDictionary<int, int>
                {
                    { 15, -1 }, { 30, -2 }, { 45, -3 }, { 60, -4 }, { 75, -5 },
                    { 93, -6 }, { 111, -7 }, { 129, -8 }, { 147, -9 }
                };
            }

            // 8:-1, 13:-2, 17:-3, 24:-4, 33:-5, 41:-6, 48:-7
            if (this.Heat.Firing.Count == 0)
            {
                this.Heat.Firing = new SortedDictionary<int, int>
                {
                    { 24, 1 }, { 39 , 2 }, { 51, 3 }, { 72, 4 }, { 99, 5 },
                    { 123, 6 }, { 144, 7 }
                };
            }

            // 14:4+, 18:6+, 22:8+, 26:10+, 30:12+, 34:14+, 38:16+, 42:18+, 46:20+, 50:INFINITY
            // If shutdown, needing piloting skill roll or fall over; roll has a +3 modifier
            if (this.Heat.Shutdown.Count == 0)
            {
                this.Heat.Shutdown = new SortedDictionary<int, float>
                {
                    { 42, 0.1f }, { 54, 0.3f }, { 66, 0.6f}, { 78, 0.8f }, { 90, 0.9f },
                    { 102, 1.0f }, { 114, 1.1f }, { 126, 1.2f }, { 138, 1.3f }, { 150, -1f }
                };
            }

            // 19:4+, 23:6+, 28:8+, 35:10+, 40:12+, 45:INFINITY
            // Explosion should impact most damaging ammo first
            // Inferno weapons require a 2nd roll in addition to the first 
            // Any ammo explosion automatically causes 2 points of pilot damage and forces a conciousness roll
            if (this.Heat.Explosion.Count == 0)
            {
                this.Heat.Explosion = new SortedDictionary<int, float>
                {
                    {  57, 0.1f },
                    {  69, 0.3f },
                    {  84, 0.5f },
                    { 105, 0.8f },
                    { 120, 0.95f },
                    { 135, -1f },
                };
            }

            // 32:8+, 39:10+, 47:12+
            // If life support damaged, can't be avoided and is in addition to normal damage
            if (this.Heat.PilotInjury.Count == 0)
            {
                this.Heat.PilotInjury = new SortedDictionary<int, float>
                {
                    { 84, 0.3f }, { 117, 0.6f}, { 141, 0.8f }
                };
            }

            // 36:8+, 44:10+
            // If roll fails, roll a hit location on the front column of mech critical hit table and apply single critical hit to location
            if (this.Heat.SystemFailures.Count == 0)
            {
                this.Heat.SystemFailures = new SortedDictionary<int, float>
                {
                    { 108, 0.3f }, { 132, 0.6f}
                };
            }

            // == Init Custom Category ==
            if (this.CustomCategories.HipActuatorCategoryId.Length == 0)
                this.CustomCategories.HipActuatorCategoryId = new string[] { "LegHip" };
            if (this.CustomCategories.UpperLegActuatorCategoryId.Length == 0)
                this.CustomCategories.UpperLegActuatorCategoryId = new string[] { "LegUpperActuator" };
            if (this.CustomCategories.LowerLegActuatorCategoryId.Length == 0)
                this.CustomCategories.LowerLegActuatorCategoryId = new string[] { "LegLowerActuator" };
            if (this.CustomCategories.FootActuatorCategoryId.Length == 0)
                this.CustomCategories.FootActuatorCategoryId = new string[] { "LegFootActuator" };

            if (this.CustomCategories.ShoulderActuatorCategoryId.Length == 0)
                this.CustomCategories.ShoulderActuatorCategoryId = new string[] { "ArmShoulder" };
            if (this.CustomCategories.UpperArmActuatorCategoryId.Length == 0)
                this.CustomCategories.UpperArmActuatorCategoryId = new string[] { "ArmUpperActuator" };
            if (this.CustomCategories.LowerArmActuatorCategoryId.Length == 0)
                this.CustomCategories.LowerArmActuatorCategoryId = new string[] { "ArmLowerActuator" };
            if (this.CustomCategories.HandActuatorCategoryId.Length == 0)
                this.CustomCategories.HandActuatorCategoryId = new string[] { "ArmHandActuator" };

            // == Init Swarm hit locations
            if (this.Melee.Swarm.MechWeights.Count == 0)
            {
                // Skip the weight dictionary and populate the locations dict
                this.Melee.Swarm.MechLocations = new Dictionary<ArmorLocation, int>()
                {
                    { ArmorLocation.Head, 8 },
                    { ArmorLocation.CenterTorso, 16 },
                    { ArmorLocation.CenterTorsoRear, 16 },
                    { ArmorLocation.LeftTorso, 16 },
                    { ArmorLocation.LeftTorsoRear, 16 },
                    { ArmorLocation.RightTorso, 16 },
                    { ArmorLocation.RightTorsoRear, 16 }
                };
                this.Melee.Swarm.MechLocationsTotalWeight = 8 + 16 + 16 + 16 + 16 + 16 + 16;
            }
            else
            {
                
                foreach (KeyValuePair<string, int> kvp in this.Melee.Swarm.MechWeights)
                {
                    if (string.Equals("Head", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.Head, kvp.Value);
                    else if (string.Equals("CenterTorso", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.CenterTorso, kvp.Value);
                    else if (string.Equals("CenterTorsoRear", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.CenterTorsoRear, kvp.Value);
                    else if (string.Equals("LeftTorso", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.LeftTorso, kvp.Value);
                    else if (string.Equals("LeftTorsoRear", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.LeftTorsoRear, kvp.Value);
                    else if (string.Equals("RightTorso", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.RightTorso, kvp.Value);
                    else if (string.Equals("RightTorsoRear", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.RightTorsoRear, kvp.Value);
                    else if (string.Equals("LeftArm", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.LeftArm, kvp.Value);
                    else if (string.Equals("RightArm", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.RightArm, kvp.Value);
                    else if (string.Equals("LeftLeg", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.LeftLeg, kvp.Value);
                    else if (string.Equals("RightLeg", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.MechLocations.Add(ArmorLocation.RightLeg, kvp.Value);
                    else
                        Mod.Log.Error?.Write($"Key {kvp.Key} is invalid! Mech swarm locations are misconfigured and will likely error out!");

                    this.Melee.Swarm.MechLocationsTotalWeight += kvp.Value;
                }
            }

            if (this.Melee.Swarm.VehicleLocations.Count == 0)
            {
                // Skip the weight dictionary and populate the locations dict
                this.Melee.Swarm.VehicleLocations = new Dictionary<VehicleChassisLocations, int>()
                {
                    { VehicleChassisLocations.Turret, 40 },
                    { VehicleChassisLocations.Left, 16 },
                    { VehicleChassisLocations.Right, 16 },
                    { VehicleChassisLocations.Rear, 40 }
                };
                this.Melee.Swarm.MechLocationsTotalWeight = 40 + 16 + 16 + 40;
            }
            else
            {
                foreach (KeyValuePair<string, int> kvp in this.Melee.Swarm.VehicleWeights)
                {
                    if (string.Equals("Turret", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.VehicleLocations.Add(VehicleChassisLocations.Turret, kvp.Value);
                    else if (string.Equals("Front", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.VehicleLocations.Add(VehicleChassisLocations.Front, kvp.Value);
                    else if (string.Equals("Left", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.VehicleLocations.Add(VehicleChassisLocations.Left, kvp.Value);
                    else if (string.Equals("Right", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.VehicleLocations.Add(VehicleChassisLocations.Right, kvp.Value);
                    else if (string.Equals("Rear", kvp.Key, StringComparison.InvariantCultureIgnoreCase))
                        this.Melee.Swarm.VehicleLocations.Add(VehicleChassisLocations.Rear, kvp.Value);
                    else
                        Mod.Log.Error?.Write($"Key {kvp.Key} is invalid! Vehicle swarm locations are misconfigured and will likely error out!");

                    this.Melee.Swarm.VehicleLocationsTotalWeight += kvp.Value;

                }
            }
        }
    }

    public class DevOpts
    {
        public bool ForceFallAfterJump = false; // If true, always enable a fall after a jump as if the mech had component damage
        public bool ForceInvalidateAllMeleeAttacks = false; // If true, all melee attacks are considered invalid
        public int DebugHeatToAdd = 250;
    }

    public class FeatureList
    {
        // If true, hull breaches will be allowed in certain biomes
        public bool BiomeBreaches = true;

        // If true, evasion won't be removed by attacks
        public bool PermanentEvasion = true;

        // If true, mechs must make a piloting skill roll (PSR) to restart. On a failure, they remain shutdown.
        public bool StartupChecks = true;
    }

    public class CustomCategoryOpts
    {
        public string[] HipActuatorCategoryId = new string[] { };
        public string[] UpperLegActuatorCategoryId = new string[] { };
        public string[] LowerLegActuatorCategoryId = new string[] { };
        public string[] FootActuatorCategoryId = new string[] { };

        public string[] ShoulderActuatorCategoryId = new string[] { };
        public string[] UpperArmActuatorCategoryId = new string[] { };
        public string[] LowerArmActuatorCategoryId = new string[] { };
        public string[] HandActuatorCategoryId = new string[] { };

    }

    // 4+ => 91.66%, 6+ => 72.22%, 8+ => 41.67%, 10+ => 16.67%, 12+ => 2.78%
    // https://github.com/Bohica/BattletechCombatMachine/wiki/HEAT or Tactical Operations pg. 105
    public class HeatOptions
    {
        // 5:-1, 10:-2, 15:-3, 20:-4, 25:-5, 31:-6, 37:-7, 43:-8, 49:-9
        public SortedDictionary<int, int> Movement = new SortedDictionary<int, int>();

        // 8:-1, 13:-2, 17:-3, 24:-4, 33:-5, 41:-6, 48:-7
        public SortedDictionary<int, int> Firing = new SortedDictionary<int, int>();

        // 14:4+, 18:6+, 22:8+, 26:10+, 30:12+, 34:14+, 38:16+, 42:18+, 46:20+, 50:INFINITY
        // If shutdown, needing piloting skill roll or fall over; roll has a +3 modifier
        public SortedDictionary<int, float> Shutdown = new SortedDictionary<int, float>();

        // 19:4+, 23:6+, 28:8+, 35:10+, 40:12+, 45:INFINITY
        // Explosion should impact most damaging ammo first
        // Inferno weapons require a 2nd roll in addition to the first 
        // Any ammo explosion automatically causes 2 points of pilot damage and forces a conciousness roll
        public SortedDictionary<int, float> Explosion = new SortedDictionary<int, float>();

        // 32:8+, 39:10+, 47:12+
        // If life support damaged, can't be avoided and is in addition to normal damage
        public SortedDictionary<int, float> PilotInjury = new SortedDictionary<int, float>();

        // 36:8+, 44:10+
        // If roll fails, roll a hit location on the front column of mech critical hit table and apply single critical hit to location
        public SortedDictionary<int, float> SystemFailures = new SortedDictionary<int, float>();

        // 1:0.05, 2:0.1, 3:0.15, 4:0.2, 5:0.25, 6:0.3, 7:0.35, 8:0.4, 9:0.45, 10:0.5
        public int ShowLowOverheatAnim = 42; // When to show as steaming
        public int ShowExtremeOverheatAnim = 90; // When to show as glowing hot
        public float ShutdownFallThreshold = 0.75f;

        // When to show the shutdown warning and when to where to place the 'overheated' bar
        public int MaxHeat = 150;
        public int WarnAtHeat = 42;
    }

    public class AIMeleeOpts
    {
        // Bonus virtual damage - applied to an attack for stripping evasion
        public float EvasionPipRemovedUtility = 10f;

        // Negative virtual damage - applied when the attacker will lose evasion
        public float EvasionPipLostUtility = 5f;

        // Bonus virtual damage - add CT armor + structure, multiplied by this value, as virtual damage
        public float PilotInjuryMultiUtility = 1.0f;

        // TODO: New
        public float MaxModifiedShutdownChance = 0.7f;
        public float MaxEVForAmmoExplosionDamage = 60f;

    }

    public class ChargeMeleeOpts
    {
        // TT => 1 point / 10, HBS => 5 points / 10 == 0.5 points per ton
        public float AttackerDamagePerTargetTon = 0.5f;
        public float TargetDamagePerAttackerTon = 0.5f;

        public float AttackerInstabilityPerTargetTon = 0.5f;
        public float TargetInstabilityPerAttackerTon = 0.5f;

        // When an attack does damage, it will be split into N groups of no more than this value 
        public float DamageClusterDivisor = 25.0f;

        // If true, make the attack apply unsteady before applying instability
        public bool UnsteadyAttackerOnHit = false;
        public bool UnsteadyAttackerOnMiss = false;
        public bool UnsteadyTargetOnHit = false;

        // If true use the delta of the piloting skills between the attacker and target for
        // a bonus/penalty
        public bool UsePilotingDelta = true;

        // The number of pips to remove from vehicles when hit by this attack
        public int TargetVehicleEvasionPipsRemoved = 4;
    }

    // BT Manual pg.37 
    public class DFAMeleeOpts
    {
        // TT => 1 point / 10, HBS => 5 points / 10 == 0.5 points per ton, x3 for DFA
        public float AttackerDamagePerTargetTon = 0.5f;
        public float TargetDamagePerAttackerTon = 1.5f;

        public float AttackerInstabilityPerTargetTon = 0.5f;
        public float TargetInstabilityPerAttackerTon = 0.5f;

        // When an attack does damage, it will be split into N groups of no more than this value 
        public float DamageClusterDivisor = 25.0f;

        // If true, make the attack apply unsteady before applying instability
        public bool UnsteadyAttackerOnHit = false;
        public bool UnsteadyAttackerOnMiss = false;
        public bool UnsteadyTargetOnHit = false;

        // If true use the delta of the piloting skills between the attacker and target for
        // a bonus/penalty
        public bool UsePilotingDelta = true;

        // The number of pips to remove from vehicles when hit by this attack
        public int TargetVehicleEvasionPipsRemoved = 4;
    }

    public class KickMeleeOps
    {
        // The base bonus applied for a kick 
        public int BaseAttackBonus = -2;
        public int LegActuatorDamageMalus = 2;
        public int FootActuatorDamageMalus = 1;

        // TT => 1 point / 5, HBS => 5 points / 5 == 1 points per ton
        public float TargetDamagePerAttackerTon = 1;
        public float TargetInstabilityPerAttackerTon = 0.5f;

        public float LegActuatorDamageReduction = 0.5f;

        // If true, make the attack apply unsteady before applying instability
        public bool UnsteadyAttackerOnHit = false;
        public bool UnsteadyAttackerOnMiss = false;
        public bool UnsteadyTargetOnHit = false;

        // The number of pips to remove from vehicles when hit by this attack
        public int TargetVehicleEvasionPipsRemoved = 4;
    }

    public class PhysicalWeaponMeleeOps
    {
        public int ArmActuatorDamageMalus = 2;

        public float DefaultDamagePerAttackerTon = 2;
        public float DefaultInstabilityPerAttackerTon = 1f;

        public bool DefaultUnsteadyAttackerOnHit = false;
        public bool DefaultUnsteadyAttackerOnMiss = false;
        public bool DefaultUnsteadyTargetOnHit = false;

        // The number of pips to remove from vehicles when hit by this attack
        public int TargetVehicleEvasionPipsRemoved = 0;
    }

    public class PunchMeleeOps
    {
        public int ArmActuatorDamageMalus = 2;
        public int HandActuatorDamageMalus = 1;

        // TT => 1 point / 10, HBS => 5 points / 10 == 0.5 points per ton
        public float TargetDamagePerAttackerTon = 0.5f;
        public float TargetInstabilityPerAttackerTon = 0.5f;

        public float ArmActuatorDamageReduction = 0.5f;

        // If true, make the attack apply unsteady before applying instability
        public bool UnsteadyAttackerOnHit = false;
        public bool UnsteadyAttackerOnMiss = false;
        public bool UnsteadyTargetOnHit = false;

        // The number of pips to remove from vehicles when hit by this attack
        public int TargetVehicleEvasionPipsRemoved = 0;
    }

    public class SwarmWeights
    {
        public int LeftTorso = 34;
        public int CenterTorso = 34;
        public int RightTorso = 34;
        public int Head = 16;
    }

    public class TurretOpts
    {
        public float DefaultTonnage = 100;
        public float LightTonnage = 60;
        public float MediumTonnage = 80;
        public float HeavyTonnage = 100;
    }

    public class SwarmOpts
    {
        public Dictionary<string, int> MechWeights = new Dictionary<string, int>();
        [JsonIgnore]
        public Dictionary<ArmorLocation, int> MechLocations = new Dictionary<ArmorLocation, int>();
        [JsonIgnore]
        public int MechLocationsTotalWeight = 0;
        
        public Dictionary<string, int> VehicleWeights = new Dictionary<string, int>();
        [JsonIgnore]
        public Dictionary<VehicleChassisLocations, int> VehicleLocations = new Dictionary<VehicleChassisLocations, int>();
        [JsonIgnore]
        public int VehicleLocationsTotalWeight = 0;

    }

    // 4+ => 91.66%, 6+ => 72.22%, 8+ => 41.67%, 10+ => 16.67%, 12+ => 2.78%
    public class MeleeOptions
    {
        // If true, all hits will average the base damage.
        public bool ExtraHitsAverageAllDamage = false;

        // Prone target modifier
        public int ProneTargetAttackModifier = -2;

        // Should weapons by filtered by location
        public bool FilterCanUseInMeleeWeaponsByAttack = false;

        public AIMeleeOpts AI = new AIMeleeOpts();
        public ChargeMeleeOpts Charge = new ChargeMeleeOpts();
        public DFAMeleeOpts DFA = new DFAMeleeOpts();
        public KickMeleeOps Kick = new KickMeleeOps();
        public PhysicalWeaponMeleeOps PhysicalWeapon = new PhysicalWeaponMeleeOps();
        public PunchMeleeOps Punch = new PunchMeleeOps();
        public TurretOpts Turrets = new TurretOpts();
        public SwarmOpts Swarm = new SwarmOpts();
    }

    // 4+ => 91.66%, 6+ => 72.22%, 8+ => 41.67%, 10+ => 16.67%, 12+ => 2.78%
    public class PilotingOptions
    {
        public float StabilityCheck = 0.30f;
        public float DFAReductionMulti = 0.05f;

        // How many damage points 
        public int FallingDamagePerTenTons = 5;
    }

    public class SkillCheckOptions
    {
        public float ModPerPointOfPiloting = 0.05f;
        public float ModPerPointOfGuts = 0.05f;
    }

    public class BiomeBreachOptions
    {
        public float VacuumCheck = 0.17f;
        public float ThinAtmoCheck = 0.03f;
    }

    public class MoveOptions
    {
        // This is set to 40m, because it should the minimum required to move across one 'hex' no 
        //   matter the penalties on that hex.
        public float MinimumMove = 40f;

        // How much walk distance is removed for each point of heat penalty
        public float HeatMovePenalty = -24f;

        // When calculating RunSpeed, multiply the current WalkSpeed by this amount. 
        public float RunMulti = 1.5f;

        // If you have leg damage and run, you can fall
        public float FallAfterRunChance = 0.30f;

        // If you have leg damage and jump, you can fall
        public float FallAfterJumpChance = 0.30f;

        //   This is set to 24m, because both ME and HexGrid.HexWidth reply upon it. However, it should likely be larger, as designMasks and vertical distances
        //   could prevent a unit from moving *at all* if this value is too low. A value like 40m should ensure a unit can always move, even through designMasks 
        //   with 0.8 movement mods and with a 0.8 elevation pitch.
        public float MPMetersPerHex = 24f;

    }
}
