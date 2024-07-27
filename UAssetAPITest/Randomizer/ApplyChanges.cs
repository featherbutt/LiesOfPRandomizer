using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UAssetAPI;
using UAssetAPI.UnrealTypes;
using UAssetAPI.PropertyTypes.Objects;

namespace LiesOfPRandomzier;

public partial class Randomizer
{
    public void ApplyChanges(RandomizerMap randomizerMap, FileInfo? outPakFile, DirectoryInfo? outAssetDir) {
        UpdateSkillTree(randomizerMap);
        UpdateWeapons(randomizerMap);
        assets.FlushAssets(outPakFile, outAssetDir);
    }

    public void UpdateSkillTree(RandomizerMap randomizerMap)
    {
        StructProperty contentInfo = assets.openStruct("CommonConstantInfo");
        ArrayProperty constantInfoArray = contentInfo.getArrayProperty("_CommonConstant_array");
        StructProperty conditionInfo = assets.openStruct("ContentConditionInfo");


        if (config.skills.remove_chapter_1_level_cap)
        {
            constantInfoArray.getStructProperty("MaxLevel_Station_Stargazer").getStringProperty("_value").Value = "999";
            // TODO: There are multiple elements of this array with the same _code_name
            // conditionArray.getStructProperty("Block_LvUp_UI_over_20lv_menu").getStringProperty("_value").Value = "999";
        }

        if (config.skills.max_organ_level == 6)
        {
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_0").getStringProperty("_value").Value = "6";

        } else if (config.skills.max_organ_level == 7)
        {
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_0").getStringProperty("_value").Value = "7";
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_1").getStringProperty("_value").Value = "7";
        }

        if (config.skills.skill_nodes == "randomize" || config.skills.skill_nodes == "shuffle")
        {
            ArrayProperty quartzEffectInfo = assets.openStruct("QuartzEffectInfo")
                .getArrayProperty("_QuartzEffectInfo_array");
            var i = 0;
            foreach (var effect in quartzEffectInfo)
            {
                if (i > randomizerMap.skills.slots.Length)
                {
                    break;
                }
                effect.getNameProperty("_special_buff_code_name").Value = randomizerMap.skills.slots[i];
                effect.getProperty<int>("_number_overlapping_effect").Value = 0;
                i++;
            }
        }

        if (config.skills.skill_nodes == "randomize" || config.skills.skill_nodes == "shuffle")
        {
            ArrayProperty quartzPocketInfo = assets.openStruct("QuartzPocketInfo")
                .getArrayProperty("_QuartzPocketInfo_array");
            var i = 0;
            foreach (var pocket in quartzPocketInfo)
            {
                if (i > randomizerMap.skills.nodes.Length)
                {
                    break;
                }
                pocket.getNameProperty("_synergy_special_buff_code_name").Value = randomizerMap.skills.nodes[i];
                pocket.getProperty<int>("_number_overlapping_synergy").Value = 0;
                var path = pocket.getProperty<FSoftObjectPath>("_image").Value;
                path.SubPathString = null;
                pocket.getProperty<FSoftObjectPath>("_image").Value = path;
                i++;
            }
        }

        if (config.skills.max_slots_per_node)
        {
            ArrayProperty quartzPocketInfo = assets.openStruct("QuartzPocketInfo")
                .getArrayProperty("_QuartzPocketInfo_array");
            foreach (var pocket in quartzPocketInfo)
            {
                pocket.getProperty<int>("_slot_count").Value = 4;
            }
        }
    }

    public void UpdateWeapons(RandomizerMap randomizerMap)
    {
        StructProperty constantInfo = assets.openStruct("CommonConstantInfo");
        ArrayProperty constantInfoArray = constantInfo.getArrayProperty("_CommonConstant_array");
        if (config.weapons.randomize_starting_weapons)
        {
            for (int i = 1; i <= 3; i++)
            {
                var r = new Random();
                var handleId = r.Next(41);
                var bladeId = r.Next(41);
                constantInfoArray.getStructProperty("PC_action_type_hnd_" + i.ToString()).getStringProperty("_value").Value = GameData.WeaponHandles[handleId];
                constantInfoArray.getStructProperty("PC_action_type_bld_" + i.ToString()).getStringProperty("_value").Value = GameData.WeaponBlades[bladeId];
            }
        }

        if (config.weapons.golden_lie_always_big)
        {
            constantInfoArray.getStructProperty("NoseStaff_Small_BLD").getStringProperty("_value").Value = "WP_PC_BLD_NoseStaff";
            constantInfoArray.getStructProperty("NoseStaff_Small_HND").getStringProperty("_value").Value = "WP_PC_HND_NoseStaff";
            constantInfoArray.getStructProperty("NoseStaff_Medium_BLD").getStringProperty("_value").Value = "WP_PC_BLD_NoseStaff";
            constantInfoArray.getStructProperty("NoseStaff_Medium_HND").getStringProperty("_value").Value = "WP_PC_HND_NoseStaff";
        }

        StructProperty itemInfo = assets.openStruct("ItemInfo");
        ArrayProperty handles = itemInfo.getArrayProperty("_ItemHandle_array");
        foreach (var handle in handles)
        {
            if (config.weapons.disassemble_weapons == "always")
            {
                handle.getProperty<bool>("_heroic_weapon").Value = false;
            } else if (config.weapons.disassemble_weapons == "never")
            {
                handle.getProperty<bool>("_heroic_weapon").Value = true;
            }
            var handleName = handle.getNameProperty("_code_name").Value;
            if (!randomizerMap.weapons.Contains(handleName))
            {
                continue;
                // This is for the Golden Lie, fix this later.
            }
            WeaponMap.Handle handleState = randomizerMap.weapons[handleName];
            if (config.weapons.randomize_scaling)
            {
                handle.getNameProperty("_correction_code_name_for_advance").Value = 
                    "correction_ratio_attack_advance_" + handleState.advance;
                handle.getNameProperty("_correction_code_name_for_motivity").Value =
                    "correction_ratio_attack_motivity_" + handleState.motivity;
                handle.getNameProperty("_correction_code_name_for_technique").Value =
                    "correction_ratio_attack_technique_" + handleState.technique;
            }
        }
    }
}
