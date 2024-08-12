using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LiesOfPRandomizer;

public class WeaponModule(
    AssetManager assets,
    WeaponConfig config,
    KeyedProvider<WeaponModule, Random> randomWrapper) : Module<WeaponModule, WeaponConfig, WeaponMap>, Module {

    static string Module.name => "weapons";
   
    private System.Random random => randomWrapper;
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

    public override WeaponMap GenerateMap()
    {
        List<string[]> scalingOptions = new();
        if (config.randomize_scaling)
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

        List<string> handlesToShuffle;
        List<string> handlesToNotShuffle;
        List<string> bladesToShuffle;
        List<string> bladesToNotShuffle;
        switch (config.randomize_combinations) {
            case WeaponConfig.RandomizeWeaponCombinationOptions.ALL:
                handlesToShuffle = [.. GameData.WeaponHandles, .. GameData.BossWeaponHandles];
                handlesToNotShuffle = new();
                bladesToShuffle = [.. GameData.WeaponBlades, .. GameData.BossWeaponBlades];
                bladesToNotShuffle = new();
                break;
            case WeaponConfig.RandomizeWeaponCombinationOptions.NONE:
                handlesToShuffle = new();
                handlesToNotShuffle = [.. GameData.WeaponHandles, .. GameData.BossWeaponHandles];
                bladesToShuffle = new();
                bladesToNotShuffle = [.. GameData.WeaponBlades, .. GameData.BossWeaponBlades];
                break;
            case WeaponConfig.RandomizeWeaponCombinationOptions.NON_BOSS:
                handlesToShuffle = [.. GameData.WeaponHandles];
                handlesToNotShuffle = [.. GameData.BossWeaponHandles];
                bladesToShuffle = [.. GameData.WeaponBlades];
                bladesToNotShuffle = [.. GameData.BossWeaponBlades];
                break;
            default:
                throw new NotImplementedException();
        }

        bladesToShuffle.Shuffle(random);

        Dictionary<string, WeaponMap.Handle> weapons = new();
        for (var i = 0; i < handlesToShuffle.Count; i++)
        {
            WeaponMap.Handle handle = new()
            {
                bladeName = bladesToShuffle[i]
            };
            if (config.randomize_scaling)
            {
                handle.motivity = scalingOptions[i][0];
                handle.technique = scalingOptions[i][1];
                handle.advance = scalingOptions[i][2];
            }
            weapons[handlesToShuffle[i]] = handle;
        }
        for (var i = 0; i < handlesToNotShuffle.Count; i++)
        {
            WeaponMap.Handle handle = new()
            {
                bladeName = bladesToNotShuffle[i]
            };
            if (config.randomize_scaling)
            {
                handle.motivity = scalingOptions[i][0];
                handle.technique = scalingOptions[i][1];
                handle.advance = scalingOptions[i][2];
            }
            weapons[handlesToNotShuffle[i]] = handle;
        }
        return new WeaponMap(weapons);
    }
    
    public override void ApplyChanges(RandomizerMap randomizerMap) {
        WeaponMap weaponMap = randomizerMap.Get<WeaponMap>();

        StructProperty constantInfo = assets.openStruct("CommonConstantInfo");
        ArrayProperty constantInfoArray = constantInfo.getArrayProperty("_CommonConstant_array");

        List<string> weaponHandles = [..GameData.WeaponHandles, ..GameData.BossWeaponHandles];
        List<string> weaponBlades = [..GameData.WeaponBlades, ..GameData.BossWeaponBlades];
        
        if (config.randomize_starting_weapons)
        {
            for (int i = 1; i <= 3; i++)
            {
                var handleId = random.Next(weaponHandles.Count());
                var bladeId = random.Next(weaponBlades.Count());
                constantInfoArray.getStructProperty("PC_action_type_hnd_" + i.ToString()).getStringProperty("_value").Value = weaponHandles[handleId];
                constantInfoArray.getStructProperty("PC_action_type_bld_" + i.ToString()).getStringProperty("_value").Value = weaponBlades[bladeId];
            }
        }

        if (config.golden_lie_always_big)
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
            if (config.disassemble_weapons == WeaponConfig.DisassembleWeaponsOptions.ALWAYS)
            {
                handle.getProperty<bool>("_heroic_weapon").Value = false;
            } else if (config.disassemble_weapons == WeaponConfig.DisassembleWeaponsOptions.NEVER)
            {
                handle.getProperty<bool>("_heroic_weapon").Value = true;
            }
            var handleName = handle.getNameProperty("_code_name").Value!;
            if (!weaponMap.Contains(handleName))
            {
                continue;
                // This is for the Golden Lie, fix this later.
            }
            WeaponMap.Handle handleState = weaponMap[handleName];
            if (config.randomize_scaling)
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