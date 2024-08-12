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

public record class Asset(UAsset asset)
{
    private bool dirty = false;

    public void FlushToFS(string name, DirectoryInfo path)
    {
        if (dirty)
        {
            Console.WriteLine($"Writing {name} to {path}/{name}.uasset", name, path);
            asset.Write(Path.Combine(path.FullName, name + ".uasset"));
        }
    }
    public void FlushToPak(string name, PakWriter pakWriter)
    {
        if (dirty)
        {
            Console.WriteLine($"Writing {name} to pak file");

            MemoryStream assetStream = asset.WriteData();
            byte[] assetBytes = assetStream.ToArray();
            var path = Globals.assetPathInPak + name + ".uasset";
            
            int breakingOffPoint = (int) asset.Exports[0].SerialOffset;
            byte[] assetPart = assetBytes.AsSpan().Slice(0, breakingOffPoint).ToArray();
            byte[] expPart = assetBytes.AsSpan().Slice(breakingOffPoint, assetBytes.Length - breakingOffPoint).ToArray();

            pakWriter.WriteFile(Globals.assetPathInPak + name + ".uasset", assetPart);
            pakWriter.WriteFile(Globals.assetPathInPak + name + ".uexp", expPart);
        }
    }

    public void MarkDirty()
    {
        dirty = true;
    }
}
