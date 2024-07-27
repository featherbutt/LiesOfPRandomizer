using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UAssetAPI.UnrealTypes;

namespace LiesOfPRandomzier;

public record class RandomizerMap(SkillMap skills, ItemMap items, WeaponMap weapons)
{
    public void Write(string outputFile)
    {
        var jsonOptions = new JsonSerializerOptions {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        };
        string serializedMap = JsonSerializer.Serialize(this, jsonOptions);
        File.WriteAllText(outputFile, serializedMap);
    }
}

public record class SkillMap(string[]? slots, string[]? nodes);

public record class WeaponMap(IDictionary<string, WeaponMap.Handle> handles)
{
    public record struct Handle(string bladeName, string? motivity, string? technique, string? advance);

    public Handle this[string handleName] => handles[handleName];
    internal string GetBladeForHandle(string name)
    {
        if (handles.ContainsKey(name)) {
            return handles[name].bladeName;
        }
        return name.Replace("_HND_", "_BLD_");
    }

    internal bool Contains(string name)
    {
        return handles.ContainsKey(name);
    }
}

public class ItemMap
{
    public IEnumerable<string> producer;
}
