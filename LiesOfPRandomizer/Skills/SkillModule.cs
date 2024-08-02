using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UAssetAPI;
using UAssetAPI.UnrealTypes;
using UAssetAPI.PropertyTypes.Objects;

namespace LiesOfPRandomizer;

public class SkillModule(
    AssetManager assets,
    SkillConfig config,
    KeyedProvider<SkillModule, Random> random) : Module<SkillModule, SkillConfig, SkillMap>, Module {

    static string Module.name => "skills";

    public override SkillMap GenerateMap() {
        
        List<string> sharedSkillPool = [];

        List<string> skillSlots = [.. GameData.SkillSlotEffects];
        if (config.max_organ_level >= 6)
        {
            skillSlots.AddRange(GameData.Level6SkillSlotEffects);
        }
        if (config.max_organ_level >= 7)
        {
            skillSlots.AddRange(GameData.Level7SkillSlotEffects);
        }

        if (config.skill_slots == Config.ShuffleMode.WITH_SAME)
        {
            skillSlots.Shuffle(random);
        }
        if (config.skill_slots == Config.ShuffleMode.WITH_OTHERS)
        {
            sharedSkillPool.AddRange(skillSlots);
        }

        List<string> skillNodes = [.. GameData.SkillNodeEffects];
        if (config.max_organ_level >= 6)
        {
            skillNodes.AddRange(GameData.Level6SkillNodeEffects);
        }
        if (config.max_organ_level >= 7)
        {
            skillNodes.AddRange(GameData.Level7SkillNodeEffects);
        }

        if (config.skill_nodes == Config.ShuffleMode.WITH_SAME)
        {
            skillNodes.Shuffle(random);
        }
        if (config.skill_nodes == Config.ShuffleMode.WITH_OTHERS)
        {
            sharedSkillPool.AddRange(skillNodes);
        }

        sharedSkillPool.Shuffle(random);
        IEnumerable<string> sharedSkillPoolEnumerable = sharedSkillPool;

        if (config.skill_slots == Config.ShuffleMode.WITH_OTHERS)
        {
            var count = skillSlots.Count();
            skillSlots = sharedSkillPoolEnumerable.Take(count).ToList();
            sharedSkillPoolEnumerable = sharedSkillPoolEnumerable.Skip(count);
        }

        if (config.skill_nodes == Config.ShuffleMode.WITH_OTHERS)
        {
            var count = skillNodes.Count();
            skillNodes = sharedSkillPoolEnumerable.Take(count).ToList();
            sharedSkillPoolEnumerable = sharedSkillPoolEnumerable.Skip(count);
        }

        return new SkillMap(
            slots: skillSlots,
            nodes: skillNodes
        );
    }

    public override void ApplyChanges(RandomizerMap randomizerMap)
    {
        SkillMap skillMap = randomizerMap.Get<SkillMap>();

        StructProperty contentInfo = assets.openStruct("CommonConstantInfo");
        ArrayProperty constantInfoArray = contentInfo.getArrayProperty("_CommonConstant_array");
        StructProperty conditionInfo = assets.openStruct("ContentConditionInfo");


        if (config.remove_chapter_1_level_cap)
        {
            constantInfoArray.getStructProperty("MaxLevel_Station_Stargazer").getStringProperty("_value").Value = "999";
            // TODO: There are multiple elements of this array with the same _code_name
            // conditionArray.getStructProperty("Block_LvUp_UI_over_20lv_menu").getStringProperty("_value").Value = "999";
        }

        if (config.max_organ_level == 6)
        {
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_0").getStringProperty("_value").Value = "6";

        } else if (config.max_organ_level == 7)
        {
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_0").getStringProperty("_value").Value = "7";
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_1").getStringProperty("_value").Value = "7";
        }

        if (skillMap.slots.Count > 0)
        {
            ArrayProperty quartzEffectInfo = assets.openStruct("QuartzEffectInfo")
                .getArrayProperty("_QuartzEffectInfo_array");
            var i = 0;
            foreach (var effect in quartzEffectInfo)
            {
                if (i > skillMap.slots.Count)
                {
                    break;
                }
                effect.getNameProperty("_special_buff_code_name").Value = skillMap.slots[i];
                effect.getProperty<int>("_number_overlapping_effect").Value = 0;
                i++;
            }
        }
        if (skillMap.nodes.Count > 0)
        {
            ArrayProperty quartzPocketInfo = assets.openStruct("QuartzPocketInfo")
                .getArrayProperty("_QuartzPocketInfo_array");
            var i = 0;
            foreach (var pocket in quartzPocketInfo)
            {
                if (i > skillMap.nodes.Count)
                {
                    break;
                }
                pocket.getNameProperty("_synergy_special_buff_code_name").Value = skillMap.nodes[i];
                pocket.getProperty<int>("_number_overlapping_synergy").Value = 0;
                var path = pocket.getProperty<FSoftObjectPath>("_image").Value;
                path.SubPathString = null;
                pocket.getProperty<FSoftObjectPath>("_image").Value = path;
                i++;
            }
        }

        if (config.max_slots_per_node)
        {
            ArrayProperty quartzPocketInfo = assets.openStruct("QuartzPocketInfo")
                .getArrayProperty("_QuartzPocketInfo_array");
            foreach (var pocket in quartzPocketInfo)
            {
                pocket.getProperty<int>("_slot_count").Value = 4;
            }
        }
    }

}