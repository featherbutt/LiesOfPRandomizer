using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomizer;

public interface Module {
    Type MapType { get; }
    
    public static abstract void Add(Randomizer randomizer);

    public static abstract string name { get; }

    public void ApplyChanges(RandomizerMap randomizerMap);

    public interface Map {
        public static virtual string GetModuleName() => throw new NotImplementedException();
    }

    public abstract record class Map<ModuleType> : Map
            where ModuleType : Module {
        public static string GetModuleName() => ModuleType.name;
        
    }

    public interface Config {}
}

public abstract class Module<ModuleType, ConfigType, MapType_> : Module
        where ModuleType : Module<ModuleType, ConfigType, MapType_>, Module
        where MapType_ : Module.Map<ModuleType>, Module.Map
        where ConfigType : class, Module.Config, new() {

    public abstract MapType_ GenerateMap();

    public Type MapType => typeof(MapType_);

    // public static string name => ModuleType.name;
    public static string name { get {
        // Console.WriteLine(typeof(ModuleType));
        throw new NotImplementedException();
        // return ModuleType.name;
    }}

    public abstract void ApplyChanges(RandomizerMap randomizerMap);

    public static void Add(Randomizer randomizer)
    {
        randomizer.AddModule<ModuleType, ConfigType, MapType_>();
    }
}