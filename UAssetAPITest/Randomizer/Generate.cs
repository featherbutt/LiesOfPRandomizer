using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomzier;


public partial class Randomizer {

    private const string randomize = "randomize";
    private const string shuffle = "shuffle";

    public RandomizerMap GenerateMap()
    {
        WeaponMap weaponState = GetWeaponState();
        return new RandomizerMap(
            skills: GenerateSkillState(),
            items: GenerateItemState(weaponState),
            weapons: weaponState);
    }

    public SkillMap GenerateSkillState()
    {
        List<string> sharedSkillPool = new();

        List<string> skillSlots = new();

        skillSlots.AddRange(GameData.SkillSlotEffects);
        if (config.skills.max_organ_level >= 6)
        {
            skillSlots.AddRange(GameData.Level6SkillSlotEffects);
        }
        if (config.skills.max_organ_level >= 7)
        {
            skillSlots.AddRange(GameData.Level7SkillSlotEffects);
        }

        if (config.skills.skill_slots == shuffle)
        {
            skillSlots.Shuffle(random);
        }
        if (config.skills.skill_slots == randomize)
        {
            sharedSkillPool.AddRange(skillSlots);
        }

        List<string> skillNodes = new();

        skillNodes.AddRange(GameData.SkillNodeEffects);
        if (config.skills.max_organ_level >= 6)
        {
            skillNodes.AddRange(GameData.Level6SkillNodeEffects);
        }
        if (config.skills.max_organ_level >= 7)
        {
            skillNodes.AddRange(GameData.Level7SkillNodeEffects);
        }

        if (config.skills.skill_nodes == shuffle)
        {
            skillNodes.Shuffle(random);
        }
        if (config.skills.skill_nodes == randomize)
        {
            sharedSkillPool.AddRange(skillNodes);
        }



        sharedSkillPool.Shuffle(random);
        IEnumerable<string> sharedSkillPoolEnumerable = sharedSkillPool;

        if (config.skills.skill_slots == randomize)
        {
            var count = skillSlots.Count();
            skillSlots = sharedSkillPoolEnumerable.Take(count).ToList();
            sharedSkillPoolEnumerable = sharedSkillPoolEnumerable.Skip(count);
        }

        if (config.skills.skill_nodes == randomize)
        {
            var count = skillNodes.Count();
            skillNodes = sharedSkillPoolEnumerable.Take(count).ToList();
            sharedSkillPoolEnumerable = sharedSkillPoolEnumerable.Skip(count);
        }

        return new SkillMap(
            slots: (config.skills.skill_slots != "") ? skillSlots.ToArray() : null,
            nodes: (config.skills.skill_nodes != "") ? skillNodes.ToArray() : null
        );
    }

    private string[][] rareScalingOptions = [
        ["B", "B", "NULL"],
        ["B", "NULL", "B"],
        ["NULL", "B", "B"],
        ["A", "D", "D"],
        ["D", "A", "D"],
        ["NULL", "C", "A"],
        ["C", "NULL", "A"],
        ["A", "NULL", "C"],
        ["NULL", "A", "C"],
    ];

    private string[][] uncommonScalingOptions = [
        ["B", "C", "D"],
        ["C", "B", "D"],
        ["D", "D", "A"],
    ];

    private string[][] commonScalingOptions = [
        ["C", "C", "C"],
        ["B", "D", "C"],
        ["D", "B", "C"],
        ["D", "C", "B"],
        ["C", "D", "B"],
    ];

    public WeaponMap GetWeaponState()
    {
        List<string[]> scalingOptions = new();
        if (config.weapons.randomize_scaling)
        {
            
            scalingOptions.AddRange(rareScalingOptions);
            scalingOptions.AddRange(rareScalingOptions);
            scalingOptions.AddRange(uncommonScalingOptions);
            scalingOptions.AddRange(uncommonScalingOptions);
            scalingOptions.AddRange(uncommonScalingOptions);
            scalingOptions.AddRange(commonScalingOptions);
            scalingOptions.AddRange(commonScalingOptions);
            scalingOptions.AddRange(commonScalingOptions);
            scalingOptions.AddRange(commonScalingOptions);
            scalingOptions.Shuffle(random);
        }
        var blades = GameData.WeaponBlades.ToList();
        if (config.weapons.randomize_combinations)
        {
            blades.Shuffle(random);
        }

        return new WeaponMap(Enumerable.Range(0, 41).ToDictionary(
            i => GameData.WeaponHandles[i],
            i =>
            {
                WeaponMap.Handle handle = new();
                handle.bladeName = blades[i];
                if (config.weapons.randomize_scaling)
                {
                    handle.motivity = scalingOptions[i][0];
                    handle.technique = scalingOptions[i][1];
                    handle.advance = scalingOptions[i][2];

                }
                return handle;
            }));
    }
    
    public ItemMap GenerateItemState(WeaponMap weaponState)
    {
        if (!config.items.randomize_locations)
        {
            return new ItemMap();
        }
        ItemLocations itemLocations = new ItemLocations(assets, config, random, weaponState);
        var itemLocationsByPriority = itemLocations.GetLocationsByPriority();
        var totalLocations = itemLocations.numLocations;
        var bufferSize = (int)(totalLocations * (1.0 - config.items.chaos));

        var highValueItems = new List<string>();
        highValueItems.AddMany("quartz", config.items.total_quartz);
        highValueItems.AddMany("Handle_InfusionStone_Type1", config.items.total_motivity_cranks);
        highValueItems.AddMany("Handle_InfusionStone_Type2", config.items.total_technique_cranks);
        highValueItems.AddMany("Handle_InfusionStone_Type3", config.items.total_advance_cranks);
        highValueItems.AddMany("Handle_InfusionStone_Type4", config.items.total_balance_cranks);
        highValueItems.AddMany("Reinforce_SlaveArm_G1", config.items.total_legion_caliber);
        highValueItems.AddMany("Reinforce_Blade_Common_G4", config.items.total_full_moonstones);
        highValueItems.AddMany("Reinforce_Hero_G2", config.items.total_full_covenant_moonstones);
        highValueItems.AddMany("Reinforce_SlaveArm_G1", config.items.total_legion_caliber);
        highValueItems.AddRange(GameData.WeaponHandles);
        if (config.items.find_legion_arms)
        {
            highValueItems.AddRange(GameData.FindableLegionArms);
        } else
        {
            highValueItems.AddMany("Exchange_SlaveArm_Parts_4", 7);
        }
        highValueItems.AddRange(GameData.Cosmetics);
        highValueItems.AddRange(GameData.Gestures);
        highValueItems.AddRange(GameData.BossErgo);
        if (config.items.double_boss_ergo)
        {
            highValueItems.AddRange(GameData.BossErgo);
        }
        highValueItems.AddRange(GameData.Amulets);
        highValueItems.AddRange(GameData.Armor);
        if (config.items.include_ngp_equipment)
        {
            highValueItems.AddRange(GameData.AmuletsNGP);
            highValueItems.AddRange(GameData.ArmorNGP);
        }

        var highValueItemsArray = highValueItems.ToArray();
        random.Shuffle(highValueItemsArray);


        List<string> mediumValueItems = new();
        mediumValueItems.AddMany("Reinforce_Hero_G1", config.items.total_crescent_covenant_moonstones);
        mediumValueItems.AddMany("Reinforce_Blade_Common_G3", 70);
        mediumValueItems.AddMany("Reinforce_Blade_Common_G2", 70);
        mediumValueItems.AddMany("Reinforce_Blade_Common_G1", 70);
        mediumValueItems.AddMany("Consume_Buff_sharpness_Fire", 10);
        mediumValueItems.AddMany("Consume_Buff_sharpness_Acid", 10);
        mediumValueItems.AddMany("Consume_Buff_sharpness_Elec", 10);
        mediumValueItems.AddMany("Consume_Throw_shotput", 10);

        var mediumValueItemsArray = mediumValueItems.ToArray();
        random.Shuffle(mediumValueItemsArray);

        List<string> lowValueItems = new();

        var lowValueItemsArray = lowValueItems.ToArray();
        random.Shuffle(lowValueItemsArray);

        string[] chaffItems = [
            "Consume_Throw_bignail",
            "Consume_Throw_sawtooth",
            "Consume_Throw_toughness_break",
            "Consume_Throw_Granade_Cluster",
            "Consume_Throw_Granade_Acid",
            "Consume_Throw_Granade_Fire",
            "Consume_Throw_Granade_Elec",
            "Consume_Area_Fire",
            "Consume_Area_acid",
            "Consume_Area_Elec",
            "Consume_cat_dust",
            "Consume_Buff_Elemental",
            "Consume_Cancel_Elemental",
            "Consume_Buff_Special",
            "Consume_Cancel_Special",
            "Consume_Drop_Ergo_save",
            "Consume_Buff_sharpness_regain",
            "Consume_Buff_SlaveMagazine",
            "Consume_Buff_SlaveMagazine",
            "Consume_Buff_stamina_regain",
            "Consume_Buff_Frenzy",
            "Consume_Buff_Frenzy",
            "Helpmate_Material",
            "Consume_Rechargeable_1",
            "goldTree_Booster1",
            "goldTree_Booster2",
            "goldTree_Booster3"

        ];

        random.Shuffle(chaffItems);

        IEnumerable<string> itemsQueue = highValueItemsArray
            .Concat(mediumValueItemsArray)
            .Concat(lowValueItemsArray)
            .Concat(TieredShuffler<string>.Loop(chaffItems));


        ItemMap result = new();
        result.producer = TieredShuffler<string>.Create(itemsQueue, bufferSize, totalLocations);

        var itemProducer = result.producer.GetEnumerator();
        itemProducer.MoveNext();

        foreach (var item in itemLocationsByPriority[ItemPriority.HIGHEST])
        {
            item.nameString = itemProducer.Current;
            itemProducer.MoveNext();
        }
        foreach (var item in itemLocationsByPriority[ItemPriority.MEDIUM])
        {
            item.nameString = itemProducer.Current;
            itemProducer.MoveNext();
        }
        foreach (var item in itemLocationsByPriority[ItemPriority.LOWEST])
        {
            item.nameString = itemProducer.Current;
            itemProducer.MoveNext();
        }
        foreach (var item in itemLocationsByPriority[ItemPriority.CHAFF])
        {
            item.nameString = itemProducer.Current;
            itemProducer.MoveNext();
        }

        return result;
    }


}
