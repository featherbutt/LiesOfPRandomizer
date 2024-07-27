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

using static LiesOfPRandomzier.ExtensionMethods;

namespace LiesOfPRandomzier;
public class AssetManager(AssetLoader assetLoader)
{
    public static AssetManager Create(DirectoryInfo assetDirectory, Usmap unrealMap, string aesKey)
    {
        var inPakFile = assetDirectory
            .GetDirectories("LiesOfP").FirstOrDefault()
            ?.GetDirectories("Content").FirstOrDefault()
            ?.GetDirectories("Paks").FirstOrDefault()
            ?.GetFiles("pakchunk0_s4-WindowsNoEditor.pak").FirstOrDefault();
        if (inPakFile != null)
        {
            return new(assetLoader: new PakAssetLoader(inPakFile.OpenRead(), unrealMap, HexToBytes(aesKey)));
        } else
        {
            return new(assetLoader: new AssetFilesLoader(assetDirectory, unrealMap));
        }
    }

    private Dictionary<string, AssetWrapper> assets = new Dictionary<string, AssetWrapper>();

    private AssetWrapper openAsset(string name)
    {
        if (assets.ContainsKey(name)) { return assets[name]; }
        AssetWrapper newAsset = new(assetLoader.LoadAsset(name));
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

            }
        }

        assets.Clear();
    }
}

public record class AssetWrapper(UAsset asset)
{
    private bool dirty = false;

    public void FlushToFS(string name, DirectoryInfo path)
    {
        if (dirty)
        {
            Console.WriteLine("Writing %s to %s", name, path);
            asset.Write(Path.Combine(path.FullName, name + ".uasset"));
        }
    }
    public void FlushToPak(string name, PakWriter pakWriter)
    {
        if (dirty)
        {
            Console.WriteLine("Writing %s to the pak", name);

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
public class PropertyWrapper
{

}

public class StringProperty(PropertyData<FString> data, AssetWrapper asset)
{
    public string Value
    {
        get { return data.Value.ToString(); }
        set {
            asset.MarkDirty();
            data.Value = FString.FromString(value);
        }
    }
}

public class NameProperty(PropertyData<FName> data, AssetWrapper asset)
{
    public bool IsNull { get
        {
            return data.Value == null;
        } }
    public string? Value
    {
        get { if (IsNull)
            {
                return null;
            }
            return data.Value.ToString();
        }
        set
        {
            asset.MarkDirty();
            if (value == null)
            {
                data.Value = null;
                data.IsZero = true;
            }
            else
            {
                data.Value = FName.FromString(asset.asset, value);
            }
        }
    }
}

public record class StructProperty(StructPropertyData data, AssetWrapper asset)
{
    public StructProperty getStructProperty(string propertyName)
    {
        return new StructProperty((StructPropertyData)getProperty(propertyName), asset);
    }

    public ArrayProperty getArrayProperty(string propertyName)
    {
        return new ArrayProperty((ArrayPropertyData)getProperty(propertyName), asset);
    }

    public StringProperty getStringProperty(string propertyName)
    {
        return new StringProperty(getProperty<FString>(propertyName), asset);
    }

    public NameProperty getNameProperty(string propertyName)
    {
        return new NameProperty(getProperty<FName>(propertyName), asset);
    }

    public PropertyData getProperty(string propertyName)
    {
        foreach (var property in data.Value)
        {
            if (property.Name.ToString() == propertyName)
            {
                return property;
            }
        }
        throw new Exception("Struct property not found");
    }

    public PropertyData<T> getProperty<T>(string propertyName)
    {
        foreach (var property in data.Value)
        {
            if (property.Name.ToString() == propertyName)
            {
                return (PropertyData<T>)property;
            }
        }
        throw new Exception("Struct property not found");
    }
}

public class ArrayProperty(ArrayPropertyData data, AssetWrapper asset) : IEnumerable<StructProperty>
{
    public StructProperty getStructProperty(string propertyName)
    {
        foreach (var property in data.Value)
        {
            StructProperty structProperty = new StructProperty((StructPropertyData)property, asset);
            FName elementName = structProperty.getProperty<FName>("_code_name").Value;
            if (elementName.ToString() == propertyName)
            {
                return structProperty;
            }
        }
        throw new Exception("Array element not found");
    }

    public IEnumerator<StructProperty> GetEnumerator()
    {
        return
            (from property in data.Value select new StructProperty((StructPropertyData)property, asset)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
