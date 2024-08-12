using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UAssetAPI.UnrealTypes;

namespace LiesOfPRandomizer;

public class RandomizerMap(IDictionary<string, object> maps)
{
    public void Write(string outputFile)
    {
        var jsonOptions = new JsonSerializerOptions {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        };
        string serializedMap = JsonSerializer.Serialize(maps, jsonOptions);
        File.WriteAllText(outputFile, serializedMap);
        Console.WriteLine($"Wrote randomizer map to {outputFile.ToString()}");
    }

    public T Get<T>() where T : Module.Map
    {
        return (T)maps[T.GetModuleName()];
    }
    
}
