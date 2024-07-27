using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.CommandLine;

using UAssetAPI.Unversioned;
using LiesOfPRandomzier;
using UAssetAPI;

Option<FileInfo> usmapOption = new(
    name: "--usmap",
    description: "the .usmap file for your version of the game",
   getDefaultValue: () => new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Lies of P.usmap"))
);

Option<FileInfo> configOption = new(
    name: "--config",
    description: "the location of the config file that sets randomizer parameters",
    getDefaultValue: () => new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "config.json"))
);

Option<DirectoryInfo> inDirOption = new(
    name: "--inDir",
    description: "the base game directory, or a directory containing the game's InfoAsset .uasset files",
    getDefaultValue: () => new DirectoryInfo(Directory.GetCurrentDirectory())
);


Option<FileInfo?> outPakOption = new(
    name: "--outPak",
    description: "the location to write the new .pak file",
);

Option<DirectoryInfo?> outAssetDirOption = new(
    name: "--outAssetDir",
    description: "the location to write the new .uasset files"
);

Option<FileInfo> outMapOption = new(
    name: "--outMap",
    description: "the location to write the json file describing all the changes",
    getDefaultValue: () => new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "LiesOfPRandomizer.json"))
);

Option<string?> aesKeyOption = new(
    name: "--aesKey",
    description: "the game's decryption key. Required if loading from the base game directory",
);


RootCommand rootCommand = new() {
    usmapOption,
    configOption,
    inDirOption,
    outPakOption,
    aesKeyOption,
    outMapOption,
    outAssetDirOption,
};

rootCommand.SetHandler((usmapFile, configFile, inDir, outPakFile, aesKey, outMap, outAssetDir) =>
{
    if (outPakFile == null && outAssetDir == null)
    {
        Console.WriteLine("Either --outAssetDir or --outPakFile must be provided.");
        return;
    }

    string jsonString = configFile.OpenText().ReadToEnd();

    JsonSerializerOptions deserializeOptions = new()
    {
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
    };

    Config config = JsonSerializer.Deserialize<Config>(jsonString, deserializeOptions)!;

    Usmap usmap = new Usmap(usmapFile.FullName);
    AssetManager assetManager = AssetManager.Create(inDir, usmap, aesKey);
    Randomizer randomizer = new(config, assetManager);

    RandomizerMap map = randomizer.GenerateMap();
    randomizer.ApplyChanges(map, outPakFile, outAssetDir);

    map.Write(outMap.FullName);

    Console.WriteLine("All done!");
}, usmapOption, configOption, inDirOption, outPakOption, aesKeyOption, outMapOption, outAssetDirOption);

return await rootCommand.InvokeAsync(args);




