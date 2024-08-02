using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.UnrealTypes;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI;
using UAssetAPI.Unversioned;
using UAssetAPI.PropertyTypes.Structs;
using System.Xml.Linq;

using static LiesOfPRandomizer.ItemPriorityMethods;

namespace LiesOfPRandomizer;

public interface ItemLocation
{
    public ItemPriority priority { get; }

    public FName name { set; }

    public string nameString { set; }

    public bool canHoldWeapon { get; }

    public bool mustHoldWeapon { get; }

}

public class MapItemLocation : ItemLocation
{
    private ItemLocations locations;

    public StructProperty itemPackage;

    private ItemPriority priority;

    ItemPriority ItemLocation.priority => priority;

    bool ItemLocation.canHoldWeapon => true;

    bool ItemLocation.mustHoldWeapon => false;

    public MapItemLocation(ItemLocations locations, StructProperty itemPackage)
    {
        this.locations = locations;
        this.itemPackage = itemPackage;

        var weaponName = itemPackage.getProperty<FName>("_weapon_item_1_handle");
        var itemName = itemPackage.getNameProperty("_Item_1_code_name");
        if (weaponName.Value != null)
        {
            if (!locations.config.randomize_weapons_with_items || !locations.weaponConfig.randomize_locations)
            {
                this.priority = ItemPriority.MUST_BE_WEAPON;
            } else
            {
                this.priority = locations.config.getItemPriority(weaponName.Value.ToString());
            }
        } else if (itemName.Value != null)
        {
            this.priority = locations.config.getItemPriority(itemName.Value);
        } else
        {
            this.priority = ItemPriority.DONT_RANDOMIZE;
        }
    }

    public string nameString
    {
        set
        {
            name = FName.FromString(itemPackage.asset.asset, value);
        }
    }

    public FName name
    {
        set
        {
            if (GameData.IsWeaponHandle(value.ToString())) {
                itemPackage.getProperty<FName>("_weapon_item_1_handle").Value = value;
                itemPackage.getNameProperty("_weapon_item_1_blade").Value = locations.weaponMap.GetBladeForHandle(value.ToString());
                itemPackage.getNameProperty("_Item_1_code_name").Value = null;
                itemPackage.getProperty<int>("_Item_1_count").Value = 0;
                
            } else
            {
                itemPackage.getNameProperty("_weapon_item_1_handle").Value = null;
                itemPackage.getNameProperty("_weapon_item_1_blade").Value = null;
                itemPackage.getNameProperty("_Item_1_code_name").Value = value.ToString();
                itemPackage.getProperty<int>("_Item_1_count").Value = 1;
            }
            itemPackage.getNameProperty("_Item_2_code_name").Value = null;
            itemPackage.getProperty<int>("_Item_2_count").Value = 0;
            itemPackage.getNameProperty("_Item_3_code_name").Value = null;
            itemPackage.getProperty<int>("_Item_3_count").Value = 0;
            itemPackage.getNameProperty("_Item_4_code_name").Value = null;
            itemPackage.getProperty<int>("_Item_4_count").Value = 0;
            itemPackage.getNameProperty("_Item_5_code_name").Value = null;
            itemPackage.getProperty<int>("_Item_5_count").Value = 0;
            itemPackage.getNameProperty("_Item_6_code_name").Value = null;
            itemPackage.getProperty<int>("_Item_6_count").Value = 0;
        }
    }
}

public class DropItemLocation : ItemLocation
{
    private ItemLocations locations;
    public StructProperty itemDrop;

    private ItemPriority priority;
    ItemPriority ItemLocation.priority
    {
        get
        {
            return priority;
        }
    }

    bool ItemLocation.canHoldWeapon => false;

    bool ItemLocation.mustHoldWeapon => false;

    public DropItemLocation(ItemLocations locations, StructProperty itemDrop)
    {
        this.locations = locations;
        this.itemDrop = itemDrop;
        var itemName = itemDrop.getNameProperty("_item_code_name").Value;
        if (itemName != null)
        {
            // TODO: What about items with conditional drop rates?
            this.priority = locations.config.getItemPriority(itemName);
        } else
        {
            this.priority = ItemPriority.DONT_RANDOMIZE;
        }
        
    }
    public string nameString
    {
        set
        {
            name = FName.FromString(itemDrop.asset.asset, value);
        }
    }

    public FName name
    {
        set
        {
            itemDrop.getProperty<FName>("_item_code_name").Value = value;
            itemDrop.getProperty<int>("_item_count").Value = 1;
            
            if (itemDrop.getProperty<FName>("_Drop_Set_Code_name").Value != null)
            {
                Console.WriteLine(itemDrop.getNameProperty("_Drop_Set_Code_name").Value);
            }
        }
    }
}

public class QuestItemLocation : ItemLocation
{
    private ItemLocations locations;

    public StructProperty data;

    private ItemPriority priority;

    ItemPriority ItemLocation.priority => priority;

    bool ItemLocation.canHoldWeapon => false;

    bool ItemLocation.mustHoldWeapon => false;

    public QuestItemLocation(ItemLocations locations, StructProperty data)
    {
        this.locations = locations;
        this.data = data;
        this.priority = locations.config.getItemPriority(data.getNameProperty("_item_code_name").Value!);
    }

    FName ItemLocation.name { set => nameString = value.ToString(); }

    public string nameString
    {
        set
        {
            data.getNameProperty("_item_code_name").Value = value;
            data.getProperty<int>("_item_count").Value = 1;

        }
    }
}

public class EventItemLocation : ItemLocation
{
    private ItemLocations parent;

    public StructProperty data;

    private ItemPriority priority;

    ItemPriority ItemLocation.priority => priority;

    bool ItemLocation.canHoldWeapon => true;

    bool ItemLocation.mustHoldWeapon => false;

    public EventItemLocation(ItemLocations parent, StructProperty data)
    {
        this.parent = parent;
        this.data = data;
        this.priority = parent.config.getItemPriority(data.getStringProperty("_param1").Value);
    }

    public FName name { set { nameString = value.ToString(); } }

    public string nameString
    {
        set
        {
            data.getStringProperty("_param1").Value = value;
            if (GameData.IsWeaponHandle(value))
            {
                data.getStringProperty("_param2").Value = parent.weaponMap.GetBladeForHandle(value);
            } else
            {
                data.getStringProperty("_param2").Value = "1";
            }        
        }
    }
}

public class ShopItemLocation : ItemLocation
{
    private ItemLocations parent;

    public StructProperty data;

    private ItemPriority priority;

    ItemPriority ItemLocation.priority => priority;

    bool ItemLocation.canHoldWeapon => true;

    bool ItemLocation.mustHoldWeapon => false;

    public ShopItemLocation(ItemLocations parent, StructProperty data)
    {
        this.parent = parent;
        this.data = data;
        this.priority = parent.config.getItemPriority(data.getNameProperty("_item_code_name").Value!);
    }

    FName ItemLocation.name { set => nameString = value.ToString(); }

    public string nameString {
        set
        {
            data.getNameProperty("_item_code_name").Value = value;
            if (GameData.IsWeaponHandle(value))
            {
                data.getNameProperty("_item_code_name2").Value = parent.weaponMap.GetBladeForHandle(value);
            } else
            {
                data.getNameProperty("_item_code_name2").Value = null;
            }
        }
    }
}

public class GiftItemLocation : ItemLocation
{
    private ItemLocations parent;

    public StructProperty data;

    private ItemPriority priority;

    ItemPriority ItemLocation.priority => priority;

    bool ItemLocation.canHoldWeapon => false;

    bool ItemLocation.mustHoldWeapon => false;

    public GiftItemLocation(ItemLocations parent, StructProperty data)
    {
        this.parent = parent;
        this.data = data;
        this.priority = parent.config.getItemPriority(data.getNameProperty("_code_name").Value!);
    }

    FName ItemLocation.name { set => nameString = value.ToString(); }

    public string nameString { set => data.getNameProperty("_code_name").Value = value; }
}

public class GiftWeaponLocation : ItemLocation
{
    private ItemLocations parent;

    public StructProperty data;

    ItemPriority ItemLocation.priority => ItemPriority.MUST_BE_WEAPON;

    bool ItemLocation.canHoldWeapon => true;

    bool ItemLocation.mustHoldWeapon => true;

    public GiftWeaponLocation(ItemLocations parent, StructProperty data)
    {
        this.parent = parent;
        this.data = data;
    }

    FName ItemLocation.name { set => nameString = value.ToString(); }

    public string nameString
    {
        set
        {
            data.getNameProperty("_handle_code_name").Value = value;
            data.getNameProperty("_blade_code_name").Value = parent.weaponMap.GetBladeForHandle(value);
        }
    }
}

public record class ItemLocations(AssetManager assets, ItemConfig config, Random random, WeaponMap weaponMap, WeaponConfig weaponConfig)
{
    public int numLocations = 0;

    public bool randomizeGiftWeapon => weaponConfig.randomize_locations && config.patch_gift == ItemConfig.PatchGifts.Random;

    public IDictionary<ItemPriority, List<ItemLocation>> GetLocationsByPriority()
    {
        var result = new Dictionary<ItemPriority, List<ItemLocation>>();
        result[ItemPriority.MUST_BE_WEAPON] = new List<ItemLocation>();
        result[ItemPriority.HIGHEST] = new List<ItemLocation>();
        result[ItemPriority.MEDIUM] = new List<ItemLocation>();
        result[ItemPriority.LOWEST] = new List<ItemLocation>();
        result[ItemPriority.CHAFF] = new List<ItemLocation>();
        foreach (var location in GetLocations())
        {
            if (location.priority != ItemPriority.DONT_RANDOMIZE)
            {
                result[location.priority].Add(location);
                numLocations++;
            }
        }
        result[ItemPriority.HIGHEST].Shuffle(random);
        result[ItemPriority.MEDIUM].Shuffle(random);
        result[ItemPriority.LOWEST].Shuffle(random);
        result[ItemPriority.CHAFF].Shuffle(random);
        return result;
    }

    public IEnumerable<ItemLocation> GetLocations()
    {
        return GetMapItemLocations()
            .Concat(GetDropItemLocations())
            .Concat(GetMapItemLocations())
            .Concat(GetQuestItemLocations())
            .Concat(GetEventItemLocations())
            .Concat(GetShopItemLocations())
            .Concat(GetSpecialShopItemLocations())
            .Concat(GetGiftItemLocations());
    }

    public IEnumerable<ItemLocation> GetMapItemLocations()
    {
        StructProperty itemPackageInfo = assets.openStruct("ItemPackageInfo");
        ArrayProperty itemPackageArray = itemPackageInfo.getArrayProperty("_ItemPackage_array");
        foreach (var itemPackage in itemPackageArray)
        {
            if (!itemPackage.getProperty<bool>("_IsExist").Value)
            {
                continue;
            }
            if (itemPackage.getProperty<FName>("_code_name").Value.ToString().EndsWith("_R2"))
            {
                continue;
            }
            if (itemPackage.getProperty<FName>("_code_name").Value.ToString().EndsWith("_R3"))
            {
                continue;
            }
            // Every item package in the vanila game has either items or a weapon, but not both.
            if (itemPackage.getProperty<int>("_Item_1_count").Value > 0)
            {

                yield return new MapItemLocation(this, itemPackage);
            }
            
            if (itemPackage.getProperty<FName>("_weapon_item_1_handle").Value != null)
            {
                yield return new MapItemLocation(this, itemPackage);
            }
            
        }
    }
    
    public IEnumerable<ItemLocation> GetDropItemLocations()
    {
        StructProperty itemDropInfo = assets.openStruct("ItemDropInfo");
        ArrayProperty itemDropArray = itemDropInfo.getArrayProperty("_PackageConfigureInfo_array");
        foreach (var itemDrop in itemDropArray)
        {
            if (itemDrop.getProperty<int>("_NGP_round").Value != 1)
            {
                continue;
            }
            yield return new DropItemLocation(this, itemDrop);
        }
    }

    public IEnumerable<ItemLocation> GetQuestItemLocations()
    {
        StructProperty itemDropInfo = assets.openStruct("QuestInfo");
        ArrayProperty itemDropArray = itemDropInfo.getArrayProperty("_QuestStep_array");

        foreach (var questStep in itemDropArray)
        {
            foreach (var reward in questStep.getArrayProperty("_true_reward_item_array"))
            {
                yield return new QuestItemLocation(this, reward);
            }
            foreach (var reward in questStep.getArrayProperty("_false_reward_item_array"))
            {
                yield return new QuestItemLocation(this, reward);
            }
        }
    }

    public IEnumerable<ItemLocation> GetEventItemLocations()
    {
        StructProperty contentCommandInfo = assets.openStruct("ContentCommandInfo");
        ArrayProperty contentCommandArray = contentCommandInfo.getArrayProperty("_ContentCommand_array");

        foreach (var command in contentCommandArray)
        {
            var name = command.getNameProperty("_command_enum");
            if (name.Value == "E_ADD_ITEM" || name.Value == "E_ADD_WEAPON")
            {
                yield return new EventItemLocation(this, command);
            } 
        }
    }

    public IEnumerable<ItemLocation> GetShopItemLocations()
    {
        StructProperty shopStruct = assets.openStruct("ShopInfo");
        ArrayProperty shopArray = shopStruct.getArrayProperty("_Shop_array");

        // Todo: handle seeling price and stock limit. Make sure weird chapter 1 NPC isn't bugged.
        foreach (var shopItem in shopArray)
        {
            var condition = shopItem.getNameProperty("_condition");
            if (!condition.IsNull && (condition.Value == "NGP_Sell_Round1_Over" || condition.Value == "NGP_Sell_Round2" || condition.Value == "NGP_Sell_Round3_AndOver" || condition.Value!.Contains("NGP2") || condition.Value.Contains("NGP1"))  )
            {
                continue;
            }
            yield return new ShopItemLocation(this, shopItem);
        }
    }

    public IEnumerable<ItemLocation> GetSpecialShopItemLocations()
    {
        StructProperty shopStruct = assets.openStruct("ShopSpecialInfo");
        ArrayProperty shopArray = shopStruct.getArrayProperty("_ShopSpecial_array");

        // Todo: handle seeling price and stock limit. Make sure weird chapter 1 NPC isn't bugged.
        foreach (var shopItem in shopArray)
        {
            var condition = shopItem.getNameProperty("_condition");
            if (!condition.IsNull && (condition.Value == "NGP_Sell_Round1_Over" || condition.Value == "NGP_Sell_Round2" || condition.Value == "NGP_Sell_Round3_AndOver" || condition.Value!.Contains("NGP2") || condition.Value.Contains("NGP1")))
            {
                continue;
            }
            yield return new ShopItemLocation(this, shopItem);
        }
    }

    public IEnumerable<ItemLocation> GetGiftItemLocations()
    {
        StructProperty giftStruct = assets.openStruct("PatchRewardInfo");
        ArrayProperty giftArray = giftStruct.getArrayProperty("_PatchRewardInfo_array");

        foreach (var gift in giftArray)
        {
            var giftItems = gift.getArrayProperty("_item_list_array");
            foreach (var item in giftItems)
            {
                yield return new GiftItemLocation(this, item);
            }
            var giftWeapons = gift.getArrayProperty("_weapon_list_array");
            foreach (var weapon in giftWeapons)
            {
                yield return new GiftWeaponLocation(this, weapon);
            }
        }
    }
}
