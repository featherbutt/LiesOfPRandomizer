using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace LiesOfPRandomizer;

public class Config(JsonDocument doc)
{
    public enum ShuffleMode {
        DONT_RANDOMIZE,
        WITH_SAME,
        WITH_OTHERS
    }

    public static Config FromJson(string json) {
        JsonDocumentOptions jsonOptions = new()
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };
        JsonDocument doc = JsonDocument.Parse(json, jsonOptions);
        JsonElement seedProperty;
        bool hasSeed = doc.RootElement.TryGetProperty("seed", out seedProperty);
        string seed = hasSeed ? seedProperty.GetString()! : Guid.NewGuid().ToString();
        return new(doc) {
            seed = seed,
        };
    }
    public string seed { get; private init; } = "";

    public T Get<T>(string name) where T : new() {
        JsonElement moduleProperty;
        bool hasModule = doc.RootElement.TryGetProperty(name, out moduleProperty);
        if (!hasModule) {
            // If no config was provided, use default values.
            return new();
        }
        JsonSerializerOptions options = new();
        options.Converters.Add(new JsonStringEnumConverter());
        return moduleProperty.Deserialize<T>(options)!;
    }
}
