using UAssetAPI.PropertyTypes.Objects;

namespace LiesOfPRandomizer;

public class CoreModule(
    AssetManager assets,
    CoreConfig coreConfig) : Module<CoreModule, CoreConfig, CoreMap>, Module {

    static string Module.name => "core";

    public override CoreMap GenerateMap() {
        return new CoreMap();
    }

    public override void ApplyChanges(RandomizerMap randomizerMap)
    {
        StructProperty contentInfo = assets.openStruct("CommonConstantInfo");
        ArrayProperty constantInfoArray = contentInfo.getArrayProperty("_CommonConstant_array");

        if (coreConfig.remove_chapter_1_level_cap)
        {
            constantInfoArray.getStructProperty("MaxLevel_Station_Stargazer").getStringProperty("_value").Value = "999";
            // TODO: There are multiple elements of this array with the same _code_name
            // conditionArray.getStructProperty("Block_LvUp_UI_over_20lv_menu").getStringProperty("_value").Value = "999";
        }

        if (coreConfig.max_organ_level == 6)
        {
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_0").getStringProperty("_value").Value = "6";

        } else if (coreConfig.max_organ_level == 7)
        {
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_0").getStringProperty("_value").Value = "7";
            constantInfoArray.getStructProperty("P_system_level_limit_NewGamePlus_1").getStringProperty("_value").Value = "7";
        }
    }
}