using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UAssetAPI.UnrealTypes;
using UAssetAPI;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.ExportTypes;
using UAssetAPI.Unversioned;
using System.Collections;

using static LiesOfPRandomizer.ExtensionMethods;

namespace LiesOfPRandomizer;

public class AssetManager(AssetLoader assetLoader)
{
    public static AssetManager Create(DirectoryInfo assetDirectory, Usmap unrealMap, string? aesKey)
    {
        var inPakFile = assetDirectory
            .GetDirectories("LiesOfP").FirstOrDefault()
            ?.GetDirectories("Content").FirstOrDefault()
            ?.GetDirectories("Paks").FirstOrDefault()
            ?.GetFiles(Globals.pakFile).FirstOrDefault();
        if (inPakFile != null)
        {
            return new(assetLoader: new PakAssetLoader(inPakFile.OpenRead(), unrealMap, HexToBytes(aesKey!)));
        } else
        {
            return new(assetLoader: new AssetFilesLoader(assetDirectory, unrealMap));
        }
    }

    private Dictionary<string, Asset> assets = new Dictionary<string, Asset>();

    private Asset openAsset(string name)
    {
        if (assets.ContainsKey(name)) { return assets[name]; }
        Asset newAsset = new(assetLoader.LoadAsset(name));
        assets.Add(name, newAsset);
        return newAsset;
    }

    public StructProperty openStruct(string name)
    {
        var asset = openAsset(name);
        var export = (NormalExport)asset.asset.Exports[0];
        return new StructProperty((StructPropertyData)export["ContentInfoDB"], asset);
    }

    public void FlushAssets(FileInfo? outPakFile, DirectoryInfo? outAssetDir)
    {
        if (outAssetDir != null)
        {
            foreach (var asset in assets)
            {
                asset.Value.FlushToFS(asset.Key, outAssetDir);
            }
        }

        if (outPakFile != null)
        {
            using (FileStream stream = outPakFile.OpenWrite())
            {
                stream.SetLength(0);
                var builder = new PakBuilder();
                var pakWriter = builder.Writer(stream, mountPoint: "../../../LiesofP/Content/");
                foreach (var asset in assets)
                {
                    asset.Value.FlushToPak(asset.Key, pakWriter);
                }
                pakWriter.WriteIndex();
                pakWriter.Close();
                Console.WriteLine($"Wrote pak file {outPakFile.ToString()}");
            }
        }

        assets.Clear();
    }
}
