using UAssetAPI.PropertyTypes.Objects;

namespace LiesOfPRandomizer;

public class SkillModule(
    AssetManager assets,
    SkillConfig config,
    CoreConfig coreConfig,
    KeyedProvider<SkillModule, Random> random) : Module<SkillModule, SkillConfig, SkillMap>, Module {

    static string Module.name => "skills";

       public override SkillMap GenerateMap() {
        
        List<string> sharedSkillPool = [];

        List<string> skillSlots = (
            from effect in GameData.QuartzSlotBuffs
            where effect.level <= coreConfig.max_organ_level
            select effect.name
        ).ToList();

        if (config.skill_slots == Config.ShuffleMode.WITH_SAME)
        {
            skillSlots.Shuffle(random);
        }
        if (config.skill_slots == Config.ShuffleMode.WITH_OTHERS)
        {
            sharedSkillPool.AddRange(skillSlots);
        }

        List<string> skillNodes = (
            from effect in GameData.QuartzNodeBuffs
            where effect.level <= coreConfig.max_organ_level
            select effect.name
        ).ToList();

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