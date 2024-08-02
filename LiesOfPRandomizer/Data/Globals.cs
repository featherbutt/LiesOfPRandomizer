using UAssetAPI.Unversioned;

namespace LiesOfPRandomizer;

static class Globals
{
    public static string assetPathInPak = "ContentInfo/InfoAsset/";

    #if _WINDOWS
    public static string pakFile = "pakchunk0_s4-WindowsNoEditor.pak";
    #else
    public static string pakFile = "pakchunk0_s4-MacNoEditor.pak";
    #endif


}
