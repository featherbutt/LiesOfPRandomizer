using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.DependencyInjection;

namespace LiesOfPRandomizer;

public record class KeyedProvider<Key, T>(T value) {
    public static implicit operator T(KeyedProvider<Key, T> p) => p.value;
}

public class Randomizer { 

    private AssetManager assets { get; init; }
    private Config config {  get; init; }

    private Dictionary<string, Type> modules = new();

    private ServiceCollection services = new();

    public Randomizer(Config config, AssetManager assetManager)
    {
        this.assets = assetManager;
        this.config = config;
        services.AddSingleton<Config>(config);
        services.AddSingleton<AssetManager>(assetManager);
    }

    public void AddModule<ModuleType>() where ModuleType : Module {
        ModuleType.Add(this);
    }

    public void AddModule<ModuleType, ConfigType, MapType>()
            where ModuleType : Module<ModuleType, ConfigType, MapType>, Module
            where MapType : Module.Map<ModuleType>, Module.Map
            where ConfigType : class, Module.Config, new() {
        int moduleRandomSeed = config.seed.GetHashCode() ^ ModuleType.name.GetHashCode();
        Random moduleRandom = new(moduleRandomSeed);

        services.AddSingleton<ModuleType>();
        services.AddSingleton<MapType>(static p => p.GetRequiredService<ModuleType>().GenerateMap());
        services.AddSingleton<KeyedProvider<ModuleType, Random>>(new KeyedProvider<ModuleType, Random>(moduleRandom));
        services.AddSingleton<ConfigType>(p => p.GetRequiredService<Config>().Get<ConfigType>(ModuleType.name));
        modules.Add(ModuleType.name, typeof(ModuleType));
    }

    internal void ApplyChanges(FileInfo? outPakFile, FileInfo? outMap, DirectoryInfo? outAssetDir)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        var map = new RandomizerMap(modules.ToDictionary(
            kvp => kvp.Key,
            kvp => provider.GetRequiredService(((Module) provider.GetRequiredService(kvp.Value)).MapType)
        ));
        foreach (var (moduleName, moduleType) in modules)
        {
            Module module = (Module) provider.GetRequiredService(moduleType);
            module.ApplyChanges(map);
        }
        if (outMap != null)
        {
            map.Write(outMap.FullName);
        }
        assets.FlushAssets(outPakFile, outAssetDir);
    }
}