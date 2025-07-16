namespace LiesOfPRandomizer;

public class ItemModule(
    AssetManager assets,
    ItemConfig config,
    CoreConfig coreconfig,
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

        ItemLocations itemLocations = new ItemLocations(assets, config, random, weaponMap, weaponConfig, coreconfig);

        var itemLocationsByPriority = itemLocations.GetLocationsByPriority();

        var weapons = (
            from weapon in GameData.Weapons
            select weapon.Blade
        ).ToArray();
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

        // High value items are the ones that most meaningful to the player. We want to assign these first, and put them in high value locations.
        var highValueItems = new List<string>();
        IDictionary<string, uint> override_amounts = new Dictionary<string, uint>();
        foreach (var material in GameData.Materials)
        {
            uint amountToAdd = override_amounts.ContainsKey(material.Name)
                ? override_amounts[material.Name]
                : material.AmountInBaseGame;
            if (coreconfig.include_dlc)
                amountToAdd += material.AmountInDlc;
            if (config.double_boss_ergo)
                amountToAdd += material.ExtraAmount;
            highValueItems.AddMany(material.Name, amountToAdd);
        }
        highValueItems.AddRange(from weapon in GameData.Weapons select weapon.Handle);
        if (config.find_legion_arms)
        {
            highValueItems.AddRange(from arm in GameData.FindableLegionArms select arm.Name);
        }
        else
        {
            // TODO: Either add the items to unlock Flamberge and Fulminis, or change the shop so they require legion plugs.
            highValueItems.AddMany("Exchange_SlaveArm_Parts_4", 7);
        }
        highValueItems.AddRange(from cosmetic in GameData.Cosmetics select cosmetic.Name);
        highValueItems.AddRange(from gesture in GameData.Gestures select gesture.Name);
        highValueItems.AddRange(from item in GameData.UpgradeKeyItems select item.Name);
        highValueItems.AddRange(from item in GameData.ExpandShopItems select item.Name);
        highValueItems.AddRange(from item in GameData.UsefulItems select item.Name);
        highValueItems.AddRange(from item in GameData.QuestItems select item.Name);
        highValueItems.AddRange(from item in GameData.Records select item.Name);
        highValueItems.AddRange(from item in GameData.UniqueItems select item.Name);
        highValueItems.AddRange(
            from amulet in GameData.AmuletBuffs
            where !amulet.isDlc || coreconfig.include_dlc
            where amulet.ngp == 0 || config.include_ngp_equipment
            select amulet.name);
        highValueItems.AddRange(GameData.Armor);
        if (config.include_ngp_equipment)
        {
            highValueItems.AddRange(GameData.ArmorNGP);
        }

        var highValueItemsArray = highValueItems.ToArray();
        random.Shuffle(highValueItemsArray);


        // TODO: These numbers are arbitrary.
        List<string> mediumValueItems = new();
        mediumValueItems.AddMany("Reinforce_Blade_Common_G3", 70);
        mediumValueItems.AddMany("Reinforce_Blade_Common_G2", 70);
        mediumValueItems.AddMany("Reinforce_Blade_Common_G1", 70);
        mediumValueItems.AddMany("Consume_Buff_sharpness_Fire", 20);
        mediumValueItems.AddMany("Consume_Buff_sharpness_Acid", 20);
        mediumValueItems.AddMany("Consume_Buff_sharpness_Elec", 20);
        mediumValueItems.AddMany("Consume_Throw_shotput", 20);
        mediumValueItems.AddRange(from item in GameData.UselessItems select item.Name);

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
            "Consume_Buff_stamina_regain",
            "Consume_Buff_Frenzy",
            "Helpmate_Material",
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