using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace LiesOfPRandomizer;

public class ItemModule(
    AssetManager assets,
    ItemConfig config,
    WeaponConfig weaponConfig,
    KeyedProvider<ItemModule, Random> random_,
    WeaponMap weaponMap) : Module<ItemModule, ItemConfig, ItemMap>, Module {

    static string Module.name => "items";

    private Random random => random_;

    public override ItemMap GenerateMap()
    {
        if (!config.randomize_locations)
        {
            return new ItemMap();
        }

        ItemLocations itemLocations = new ItemLocations(assets, config, random, weaponMap, weaponConfig);

        var itemLocationsByPriority = itemLocations.GetLocationsByPriority();

        var weapons = (string[]) GameData.WeaponHandles.Clone();
        random.Shuffle(weapons);
        IEnumerable<string> weaponsEnumerable = weapons.AsEnumerable();
        // Some items must be weapons, so we assign those first.
        foreach (var item in itemLocationsByPriority[ItemPriority.MUST_BE_WEAPON])
        {
            item.nameString = weaponsEnumerable.First();
            weaponsEnumerable = weaponsEnumerable.Skip(1);
        }

        var totalLocations = itemLocations.numLocations;
        var bufferSize = (int)(totalLocations * (1.0 - config.chaos));

        var highValueItems = new List<string>();
        highValueItems.AddMany("quartz", config.total_quartz);
        highValueItems.AddMany("Handle_InfusionStone_Type1", config.total_motivity_cranks);
        highValueItems.AddMany("Handle_InfusionStone_Type2", config.total_technique_cranks);
        highValueItems.AddMany("Handle_InfusionStone_Type3", config.total_advance_cranks);
        highValueItems.AddMany("Handle_InfusionStone_Type4", config.total_balance_cranks);
        highValueItems.AddMany("Reinforce_SlaveArm_G1", config.total_legion_caliber);
        highValueItems.AddMany("Reinforce_Blade_Common_G4", config.total_full_moonstones);
        highValueItems.AddMany("Reinforce_Hero_G2", config.total_full_covenant_moonstones);
        highValueItems.AddMany("Reinforce_SlaveArm_G1", config.total_legion_caliber);
        highValueItems.AddRange(GameData.WeaponHandles);
        if (config.find_legion_arms)
        {
            highValueItems.AddRange(GameData.FindableLegionArms);
        } else
        {
            // TODO: Eithwr add the items to unlock Flamberge and Fulminis, or change the shop so they require legion plugs.
            highValueItems.AddMany("Exchange_SlaveArm_Parts_4", 7);
        }
        highValueItems.AddRange(GameData.Cosmetics);
        highValueItems.AddRange(GameData.Gestures);
        highValueItems.AddRange(GameData.BossErgo);
        if (config.double_boss_ergo)
        {
            highValueItems.AddRange(GameData.BossErgo);
        }
        highValueItems.AddRange(GameData.Amulets);
        highValueItems.AddRange(GameData.Armor);
        if (config.include_ngp_equipment)
        {
            highValueItems.AddRange(GameData.AmuletsNGP);
            highValueItems.AddRange(GameData.ArmorNGP);
        }

        var highValueItemsArray = highValueItems.ToArray();
        random.Shuffle(highValueItemsArray);


        // TODO: These numbers are arbitrary.
        List<string> mediumValueItems = new();
        mediumValueItems.AddMany("Reinforce_Hero_G1", config.total_crescent_covenant_moonstones);
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


        IEnumerable<string> producer = TieredShuffler<string>.Create(random, itemsQueue, bufferSize, totalLocations);

        var itemProducer = producer.GetEnumerator();
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

        return new();;
    }

    public override void ApplyChanges(RandomizerMap randomizerMap) {
        // This module is a bit wonky: changes get applied in GenerateMap.
    }
}