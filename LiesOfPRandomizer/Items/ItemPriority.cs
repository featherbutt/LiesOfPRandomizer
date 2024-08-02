using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomizer;

public enum ItemPriority
{
    MUST_BE_WEAPON, HIGHEST, MEDIUM, LOWEST, CHAFF, DONT_RANDOMIZE
}

internal static class ItemPriorityMethods
{
    public static string[] neverRandomize = [
        "Consume_Monard_Lamp",
        "Consume_giveup",
        "Consume_ReturnClockE",
        // "Consume_Rechargeable_1",
        // "Consume_Buff_sharpness_recovery",
    ];

    public static string[] allowableCollections = [
        "Collection_Core_Fire",
        "Collection_Core_Electronic",
    ];

    public static ItemPriority getItemPriority(this ItemConfig itemConfig, string itemName)
    {
        if (neverRandomize.Contains(itemName))
        {
            return ItemPriority.DONT_RANDOMIZE;
        }
        if (itemName.StartsWith("Collection_Record"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("WP_PC_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Handle_InfusionStone_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Costume_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.EndsWith("_Boss_Ergo"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Mask_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Head_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("AC_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("part_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Grinder_Unit_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("goldTree_Booster1"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Gesture_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Collection_Databox_Interpretation_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Collection_Dottedpaper_"))
        {
            return ItemPriority.HIGHEST;
        }


        if (itemName.StartsWith("Krat_BlackBox_"))
        {
            return ItemPriority.HIGHEST;
        }
        if (itemName.StartsWith("Venigni_BlackBox_2"))
        {
            return ItemPriority.HIGHEST;
        }


        if (itemName.StartsWith("Epic_"))
        {
            return ItemPriority.DONT_RANDOMIZE;
        }
        if (itemName.StartsWith("Key_"))
        {
            return ItemPriority.DONT_RANDOMIZE;
        }
        if (itemName.StartsWith("Consume_Instrument_01"))
        {
            return ItemPriority.HIGHEST;
        }

        if (itemName.StartsWith("SlaveArm_"))
        {
            if (itemConfig.find_legion_arms)
            {
                return ItemPriority.HIGHEST;
            }
            else
            {
                return ItemPriority.DONT_RANDOMIZE;
            }
        }

        string[] highestPriority = [
            "Reinforce_Hero_G2",
                "Reinforce_Blade_Common_G4",
                "Reinforce_SlaveArm_G1",
                "Exchange_SlaveArm_Parts_4",
                "quartz",
                "Consume_ProtectDropErgo_7L",
                "Consume_ProtectDropErgo_6L",
                "Consume_ProtectDropErgo_5L",
                "Consume_ProtectDropErgo_4L",
                "Consume_ProtectDropErgo_3L",
                "Consume_ProtectDropErgo_2L",
                "Consume_ProtectDropErgo_1L",
            ];


        if (highestPriority.Contains(itemName))
        {
            return ItemPriority.HIGHEST;
        }

        string[] middlePriority = [
            "Reinforce_Hero_G1",
                "Reinforce_Blade_Common_G1",
                "Reinforce_Blade_Common_G2",
                "Reinforce_Blade_Common_G3",
                "Consume_ProtectDropErgo_3M",
                "Consume_ProtectDropErgo_2M",
                "Consume_ProtectDropErgo_1M",
                "Consume_ProtectDropErgo_1MP",
            ];

        if (middlePriority.Contains(itemName))
        {
            return ItemPriority.HIGHEST;
        }

        string[] lowPriority = [
            "Reinforce_Hero_G1",
                "Reinforce_Blade_Common_G1",
                "Reinforce_Blade_Common_G2",
                "Reinforce_Blade_Common_G3",
                "Consume_ProtectDropErgo_1",
                "Consume_ProtectDropErgo_2",
                "Consume_ProtectDropErgo_3",
                "Consume_ProtectDropErgo_4",
            ];

        if (lowPriority.Contains(itemName))
        {
            return ItemPriority.LOWEST;
        }

        if (itemName.StartsWith("Collection_Letter_"))
        {
            return ItemPriority.MEDIUM;
        }

        return ItemPriority.CHAFF;
    }

}
