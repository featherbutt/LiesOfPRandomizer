using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UAssetAPI;
using UAssetAPI.UnrealTypes;
using UAssetAPI.Unversioned;

namespace LiesOfPRandomizer;

public interface AssetLoader
{
    UAsset LoadAsset(string name);
}

public class PakAssetLoader(FileStream pakFileStream, Usmap unrealMap, byte[] aesKey) : AssetLoader
{
    private PakReader pakFileReader = new PakBuilder()
        .Key(aesKey)
        .Reader(pakFileStream);    

    UAsset AssetLoader.LoadAsset(string name)
    {
        var uassetPath = Globals.assetPathInPak + name + ".uasset";
        byte[] assetBytes = pakFileReader.Get(pakFileStream, uassetPath);
        var uexpPath = Globals.assetPathInPak + name + ".uexp";
        byte[] uexpBytes = pakFileReader.Get(pakFileStream, uexpPath);
        MemoryStream assetStream = new(assetBytes.Concat(uexpBytes).ToArray());
        AssetBinaryReader assetReader = new(assetStream);
        return new UAsset(assetReader, EngineVersion.VER_UE4_27, unrealMap); 
    }
}

public class AssetFilesLoader(DirectoryInfo assetDirectory, Usmap unrealMap) : AssetLoader
{
    UAsset AssetLoader.LoadAsset(string name)
    {
        FileInfo? assetFile = assetDirectory.GetFiles(name + ".uasset", SearchOption.AllDirectories).FirstOrDefault();
        if (assetFile == null)
        {
            throw new FileNotFoundException("Asset file not found: " + name);
        }
        return new UAsset(assetFile.FullName, EngineVersion.VER_UE4_27, unrealMap);
    }
}