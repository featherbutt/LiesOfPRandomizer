﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomizer;

public static class GameData
{
    public static string[] SkillNodeEffects = [
        "CL1_Group1_synergy",
        "CL1_Group2_synergy",
        "CL1_Group3_synergy",
        "CL1_Group4_synergy",
        "CL2_Group1_synergy",
        "CL2_Group2_synergy",
        "CL2_Group3_synergy",
        "CL2_Group4_synergy",
        "CL3_Group1_synergy",
        "CL3_Group2_synergy",
        "CL3_Group3_synergy",
        "CL3_Group4_synergy",
        "CL4_Group1_synergy",
        "CL4_Group2_synergy",
        "CL4_Group3_synergy",
        "CL4_Group4_synergy",
        "CL5_Group1_synergy",
        "CL5_Group2_synergy",
        "CL5_Group3_synergy",
        "CL5_Group4_synergy"
    ];
    public static string[] Level6SkillNodeEffects = [
        "CL6_Group1_synergy",
        "CL6_Group2_synergy",
        "CL6_Group3_synergy",
        "CL6_Group4_synergy"
    ];
    public static string[] Level7SkillNodeEffects = [
        "CL7_Group1_synergy",
        "CL7_Group2_synergy",
        "CL7_Group3_synergy",
        "CL7_Group4_synergy"
    ];

    public static string[] SkillSlotEffects = [
        "CL1_RED_1",
        "CL1_RED_2",
        "CL1_RED_3",
        "CL1_RED_4",
        "CL1_BLUE_1",
        "CL1_BLUE_2",
        "CL1_BLUE_3",
        "CL1_BLUE_4",
        "CL1_GREEN_1",
        "CL1_GREEN_2",
        "CL1_GREEN_3",
        "CL1_GREEN_4",
        "CL1_YELLOW_1",
        "CL1_YELLOW_2",
        "CL1_YELLOW_3",
        "CL1_YELLOW_4",
        "CL2_RED_1",
        "CL2_RED_2",
        "CL2_RED_3",
        "CL2_RED_4",
        "CL2_BLUE_1",
        "CL2_BLUE_2",
        "CL2_BLUE_3",
        "CL2_BLUE_4",
        "CL2_GREEN_1",
        "CL2_GREEN_2",
        "CL2_GREEN_3",
        "CL2_GREEN_4",
        "CL2_YELLOW_1",
        "CL2_YELLOW_2",
        "CL2_YELLOW_3",
        "CL2_YELLOW_4",
        "CL3_RED_1",
        "CL3_RED_2",
        "CL3_RED_3",
        "CL3_RED_4",
        "CL3_BLUE_1",
        "CL3_BLUE_2",
        "CL3_BLUE_3",
        "CL3_BLUE_4",
        "CL3_GREEN_1",
        "CL3_GREEN_2",
        "CL3_GREEN_3",
        "CL3_GREEN_4",
        "CL3_YELLOW_1",
        "CL3_YELLOW_2",
        "CL3_YELLOW_3",
        "CL3_YELLOW_4",
        "CL4_RED_1",
        "CL4_RED_2",
        "CL4_RED_3",
        "CL4_RED_4",
        "CL4_BLUE_1",
        "CL4_BLUE_2",
        "CL4_BLUE_3",
        "CL4_BLUE_4",
        "CL4_GREEN_1",
        "CL4_GREEN_2",
        "CL4_GREEN_3",
        "CL4_GREEN_4",
        "CL4_YELLOW_1",
        "CL4_YELLOW_2",
        "CL4_YELLOW_3",
        "CL4_YELLOW_4",
        "CL5_RED_1",
        "CL5_RED_2",
        "CL5_RED_3",
        "CL5_RED_4",
        "CL5_BLUE_1",
        "CL5_BLUE_2",
        "CL5_BLUE_3",
        "CL5_BLUE_4",
        "CL5_GREEN_1",
        "CL5_GREEN_2",
        "CL5_GREEN_3",
        "CL5_GREEN_4",
        "CL5_YELLOW_1",
        "CL5_YELLOW_2",
        "CL5_YELLOW_3",
        "CL5_YELLOW_4"
    ];

    public static string[] Level6SkillSlotEffects = [
        "CL6_RED_1",
        "CL6_RED_2",
        "CL6_RED_3",
        "CL6_RED_4",
        "CL6_BLUE_1",
        "CL6_BLUE_2",
        "CL6_BLUE_3",
        "CL6_BLUE_4",
        "CL6_GREEN_1",
        "CL6_GREEN_2",
        "CL6_GREEN_3",
        "CL6_GREEN_4",
        "CL6_YELLOW_1",
        "CL6_YELLOW_2",
        "CL6_YELLOW_3",
        "CL6_YELLOW_4",
    ];

    public static string[] Level7SkillSlotEffects = [
        "CL7_RED_1",
        "CL7_RED_2",
        "CL7_RED_3",
        "CL7_RED_4",
        "CL7_BLUE_1",
        "CL7_BLUE_2",
        "CL7_BLUE_3",
        "CL7_BLUE_4",
        "CL7_GREEN_1",
        "CL7_GREEN_2",
        "CL7_GREEN_3",
        "CL7_GREEN_4",
        "CL7_YELLOW_1",
        "CL7_YELLOW_2",
        "CL7_YELLOW_3",
        "CL7_YELLOW_4",
    ];

    internal static bool IsWeaponHandle(string name)
    {
        return name.StartsWith("WP_PC_HND_");
    }

    public static string[] WeaponHandles = [
        "WP_PC_HND_Kukri",
        "WP_PC_HND_Rapier",
        "WP_PC_HND_Shovel",
        "WP_PC_HND_FirePickaxe",
        "WP_PC_HND_ShieldSpear",
        "WP_PC_HND_ClockworkBlunt",
        "WP_PC_HND_GreatSpear",
        "WP_PC_HND_SwordLance",
        "WP_PC_HND_Cleaver",
        "WP_PC_HND_Shamshir",
        "WP_PC_HND_Glaive",
        "WP_PC_HND_KkabiClub",
        "WP_PC_HND_GreatSaw",
        "WP_PC_HND_CrystalSpear",
        "WP_PC_HND_AcidGreatsword",
        "WP_PC_HND_GreatAxe",
        "WP_PC_HND_FlameDagger",
        "WP_PC_HND_Bayonet",
        "WP_PC_HND_FireAxe",
        "WP_PC_HND_ClockSword",
        "WP_PC_HND_Hwando",
        "WP_PC_HND_ElectricCutter",
        "WP_PC_HND_CrystalAxe",
        "WP_PC_HND_ElectricHammer",
        "WP_PC_HND_Baton",
        "WP_PC_HND_Dagger",
        "WP_PC_HND_RockDrill",
        "WP_PC_HND_FlameSword",
    ];

    public static string[] BossWeaponHandles = [
        "WP_PC_HND_TransformGreatSword",
        "WP_PC_HND_Trident",
        "WP_PC_HND_CrystalSword",
        "WP_PC_HND_UmbrellaSword",
        "WP_PC_HND_Charkram",
        "WP_PC_HND_ChainScythe",
        "WP_PC_HND_DragonGlaive",
        "WP_PC_HND_ScissorSword",
        "WP_PC_HND_Saber",
        "WP_PC_HND_SevenSword",
        "WP_PC_HND_Halberd",
        "WP_PC_HND_CoilRod",
        "WP_PC_HND_NoseStaff",
    ];
    public static string[] WeaponBlades = [
        "WP_PC_BLD_Kukri",
        "WP_PC_BLD_Rapier",
        "WP_PC_BLD_Shovel",
        "WP_PC_BLD_FirePickaxe",
        "WP_PC_BLD_ShieldSpear",
        "WP_PC_BLD_ClockworkBlunt",
        "WP_PC_BLD_GreatSpear",
        "WP_PC_BLD_SwordLance",
        "WP_PC_BLD_Cleaver",
        "WP_PC_BLD_Shamshir",
        "WP_PC_BLD_Glaive",
        "WP_PC_BLD_KkabiClub",
        "WP_PC_BLD_GreatSaw",
        "WP_PC_BLD_CrystalSpear",
        "WP_PC_BLD_AcidGreatsword",
        "WP_PC_BLD_GreatAxe",
        "WP_PC_BLD_FlameDagger",
        "WP_PC_BLD_Bayonet",
        "WP_PC_BLD_FireAxe",
        "WP_PC_BLD_ClockSword",
        "WP_PC_BLD_Hwando",
        "WP_PC_BLD_ElectricCutter",
        "WP_PC_BLD_CrystalAxe",
        "WP_PC_BLD_ElectricHammer",
        "WP_PC_BLD_Baton",
        "WP_PC_BLD_Dagger",
        "WP_PC_BLD_RockDrill",
        "WP_PC_BLD_FlameSword",
    ];

    public static string[] BossWeaponBlades = [
        "WP_PC_BLD_TransformGreatSword",
        "WP_PC_BLD_Trident",
        "WP_PC_BLD_CrystalSword",
        "WP_PC_BLD_UmbrellaSword",
        "WP_PC_BLD_Charkram",
        "WP_PC_BLD_ChainScythe",
        "WP_PC_BLD_DragonGlaive",
        "WP_PC_BLD_ScissorSword",
        "WP_PC_BLD_Saber",
        "WP_PC_BLD_SevenSword",
        "WP_PC_BLD_Halberd",
        "WP_PC_BLD_CoilRod",
        "WP_PC_BLD_NoseStaff",
    ];

    public static string[] FindableLegionArms = [
        "SlaveArm_PileBunker",
        "SlaveArm_Aegis",
        "SlaveArm_AcidLuncher",
        "SlaveArm_SniperCannon",
        "SlaveArm_PuppetString",
        "SlaveArm_Fulminis",
        "SlaveArm_Flamberge"
    ];

    public static string[] Cosmetics = [
        "Costume_Naughty_Boy",
        "Costume_Stalker_Cat",
        "Costume_03",
        "Costume_Factory_Meister",
        "Costume_Stalker_Monster_Hunter",
        "Costume_Prince_Robes",
        "Costume_Someone_Memory",
        "Costume_Stalker_Madman",
        "Costume_Stalker_Weasel",
        "Costume_Stalker_Fox",
        "Costume_Stalker_Survivor",
        "Costume_Stalker_Pilgrim",
        "Costume_Alchemist_Cape",
        "Costume_Stalker_Army_Surgeon",
        "Costume_Venigni_Coat",
        "Costume_02",
        "Costume_01",
        "Costume_Stalker_WhiteLady",
        "Mask_Stalker_Cat",
        "Mask_Stalker_Madman",
        "Mask_Stalker_Weasel",
        "Mask_Stalker_Fox",
        "Mask_Stalker_Survivor",
        "Mask_Stalker_Pilgrim",
        "Mask_Stalker_ArmySurgeon",
        "Mask_Stalker_WhiteLady",
        "Head_Naughty_Boy",
        "Head_Glassess_Venigni",
        "HatCostume_GuanYu",
        "Costume_GuanYu",
        "Head_Glassess_RudolfNose",
        "HatCostume_ReindeerHorn",
        "HatCostume_RedHat",
        "Costume_Alidoro",
        "HatCostume_Alidoro",
        "Head_Glassess_Emerald",
        "HatCostume_AlchemistHat"
    ];

    public static string[] Amulets = [
        "AC_mgmt_stat_L2_7",
        "AC_def_L1_3",
        "AC_resist_L3_9",
        "AC_boss_L1_2",
        "AC_mgmt_equip_L1_2",
        "AC_boss_L1_5",
        "AC_boss_L1_6",
        "AC_mgmt_stat_L1_3",
        "AC_mgmt_stat_L3_4",
        "AC_mgmt_stat_L2_5",
        "AC_mgmt_equip_L1_3",
        "AC_resist_L2_1",
        "AC_mgmt_equip_L2_4",
        "AC_mgmt_stat_L1_11",
        "AC_atk_L2_2",
        "AC_mgmt_equip_L3_10",
        "AC_mgmt_stat_L1_1",
        "AC_mgmt_equip_L3_11",
        "AC_atk_L2_4",
        "AC_mgmt_equip_L3_5",
        "AC_boss_L1_3",
        "AC_mgmt_stat_L2_6",
        "AC_mgmt_stat_L2_9",
        "AC_atk_L2_3",
        "AC_mgmt_stat_L2_8",
        "AC_def_L3_1",
        "AC_boss_L1_4",
        "AC_boss_L1_1",
        "AC_atk_L2_1",
        "AC_mgmt_stat_L1_10",
        "AC_mgmt_equip_L1_1"
        ];

    public static string[] AmuletsNGP = [
        "AC_mgmt_equip_L1_2_1",
        "AC_mgmt_equip_L1_2_2",
        "AC_mgmt_stat_L3_4",
        "AC_mgmt_equip_L1_3_1",
        "AC_mgmt_equip_L1_3_2",
        "AC_resist_L2_1_1",
        "AC_mgmt_stat_L1_11_1",
        "AC_mgmt_stat_L3_2",
        "AC_mgmt_equip_L4_6",
        "AC_mgmt_stat_L2_8_1",
        "AC_mgmt_stat_L1_10_1",
        "AC_mgmt_stat_L1_10_2",
        "AC_mgmt_equip_L1_1_1",
        "AC_mgmt_equip_L1_1_2"
    ];

    public static string[] StartingArmor = [
        "part_bone_00",
        "part_underskin_0",
        "part_assembly_0"
        ];

    public static string[] Armor = [
        "part_bone_2",
        "part_bone_0",
        "part_bone_1",
        "part_bone_4",
        "part_bone_6",
        "part_bone_3",
        "part_bone_5",
        "part_skin_slash_1",
        "part_skin_strike_1",
        "part_skin_pierce_1",
        "part_skin_strike_0",
        "part_skin_slash_4",
        "part_skin_strike_4",
        "part_skin_pierce_4",
        "part_skin_slash_2",
        "part_skin_strike_2",
        "part_skin_pierce_2",
        "part_skin_slash_3",
        "part_skin_strike_3",
        "part_skin_pierce_3",
        "part_underskin_acid_1",
        "part_underskin_fire_1",
        "part_underskin_electric_1",
        "part_underskin_acid_3",
        "part_underskin_fire_3",
        "part_underskin_electric_3",
        "part_underskin_acid_2",
        "part_underskin_fire_2",
        "part_underskin_electric_2",
        "part_assembly_curse_3",
        "part_assembly_impact_3",
        "part_assembly_break_3",
        "part_assembly_curse_1",
        "part_assembly_impact_1",
        "part_assembly_break_1",
        "part_assembly_curse_2",
        "part_assembly_impact_2",
        "part_assembly_break_2"
    ];

    public static string[] ArmorNGP = [
        "part_bone_2_1",
        "part_bone_2_2",
        "part_bone_0_1",
        "part_bone_0_2",
        "part_bone_1_1",
        "part_bone_1_2",
        "part_bone_4_1",
        "part_bone_4_2",
        "part_bone_6_1",
        "part_bone_6_2",
        "part_bone_3_1",
        "part_bone_3_2",
        "part_bone_5_1",
        "part_bone_5_2",
        "part_skin_slash_1_1",
        "part_skin_slash_1_2",
        "part_skin_strike_1_1",
        "part_skin_strike_1_2",
        "part_skin_pierce_1_1",
        "part_skin_pierce_1_2",
        "part_skin_slash_4_1",
        "part_skin_slash_4_2",
        "part_skin_strike_4_1",
        "part_skin_strike_4_2",
        "part_skin_pierce_4_1",
        "part_skin_pierce_4_2",
        "part_skin_slash_2_1",
        "part_skin_slash_2_2",
        "part_skin_strike_2_1",
        "part_skin_strike_2_2",
        "part_skin_pierce_2_1",
        "part_skin_pierce_2_2",
        "part_skin_slash_3_1",
        "part_skin_slash_3_2",
        "part_skin_strike_3_1",
        "part_skin_strike_3_2",
        "part_skin_pierce_3_1",
        "part_skin_pierce_3_2",
        "part_underskin_acid_1_1",
        "part_underskin_acid_1_2",
        "part_underskin_fire_1_1",
        "part_underskin_fire_1_2",
        "part_underskin_electric_1_1",
        "part_underskin_electric_1_2",
        "part_underskin_acid_3_1",
        "part_underskin_acid_3_2",
        "part_underskin_fire_3_1",
        "part_underskin_fire_3_2",
        "part_underskin_electric_3_1",
        "part_underskin_electric_3_2",
        "part_underskin_acid_2_1",
        "part_underskin_acid_2_2",
        "part_underskin_fire_2_1",
        "part_underskin_fire_2_2",
        "part_underskin_electric_2_1",
        "part_underskin_electric_2_2",
        "part_assembly_curse_3_1",
        "part_assembly_curse_3_2",
        "part_assembly_impact_3_1",
        "part_assembly_impact_3_2",
        "part_assembly_break_3_1",
        "part_assembly_break_3_2",
        "part_assembly_curse_1_1",
        "part_assembly_curse_1_2",
        "part_assembly_impact_1_1",
        "part_assembly_impact_1_2",
        "part_assembly_break_1_1",
        "part_assembly_break_1_2",
        "part_assembly_curse_2_1",
        "part_assembly_curse_2_2",
        "part_assembly_impact_2_1",
        "part_assembly_impact_2_2",
        "part_assembly_break_2_1",
        "part_assembly_break_2_2"
    ];

    public static string[] BossErgo = [
        "CH00_Boss_Ergo",
        "CH01_Boss_Ergo",
        "CH02_Boss_Ergo",
        "CH03_Boss_Ergo",
        "CH04_Boss_Ergo",
        "CH06_Boss_Ergo",
        "CH07_Boss_Ergo",
        "CH08_Boss_Ergo",
        "CH09_Boss_Ergo",
        "CH10_Boss_Ergo",
        "CH12_Boss_Ergo",
        "CH13_Boss_Ergo"
    ];

    public static string[] Gestures = [
        "Gesture_Praise",
        "Gesture_Test_Beg1",
        "Gesture_Pray",
        "Gesture_Joy",
        "Gesture_Provoke",
        "Gesture_Fear",
        "Gesture_Interaction_Bottom",
        "Gesture_Clap",
        "Gesture_Anger",
        "Gesture_SwordSalute",
        "Gesture_Sad",
        "Gesture_Sitdown",
        "Gesture_Test_Beg2",
        "Gesture_Show_Cloth",
        "Gesture_Boast",
        "Gesture_Hi",
        "Gesture_Test_Apology",
    ];
}
