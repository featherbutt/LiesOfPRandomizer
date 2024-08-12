using System;
using System.Collections.Generic;
using System.IO;
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

namespace LiesOfPRandomizer;

public class Property<T>(PropertyData<T> data, Asset asset)
{
    public T Value
    {
        get => data.Value; 
        set {
            asset.MarkDirty();
            data.Value = value;
        }
    }
}

public class StringProperty(PropertyData<FString> data, Asset asset)
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

public class NameProperty(PropertyData<FName> data, Asset asset)
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
                data.Value = null!;
                data.IsZero = true;
            }
            else
            {
                data.Value = FName.FromString(asset.asset, value);
            }
        }
    }
}

public record class StructProperty(StructPropertyData data, Asset asset)
{
    public StructProperty getStructProperty(string propertyName)
    {
        return new StructProperty((StructPropertyData)getPropertyData(propertyName), asset);
    }

    public ArrayProperty getArrayProperty(string propertyName)
    {
        return new ArrayProperty((ArrayPropertyData)getPropertyData(propertyName), asset);
    }

    public StringProperty getStringProperty(string propertyName)
    {
        return new StringProperty(getPropertyData<FString>(propertyName), asset);
    }

    public NameProperty getNameProperty(string propertyName)
    {
        return new NameProperty(getPropertyData<FName>(propertyName), asset);
    }

    public Property<T> getProperty<T>(string propertyName)
    {
        return new(getPropertyData<T>(propertyName), asset);
    }

    private PropertyData getPropertyData(string propertyName)
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

    private PropertyData<T> getPropertyData<T>(string propertyName)
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

public class ArrayProperty(ArrayPropertyData data, Asset asset) : IEnumerable<StructProperty>
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

    IEnumerator<StructProperty> IEnumerable<StructProperty>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
