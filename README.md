# Lies of P Randomizer
[Issues](https://github.com/featherbutt/LiesOfPRandomizer/issues) | [License](https://github.com/featherbutt/LiesOfPRandomizer/blob/master/LICENSE.md)

Note: This randomizer is still in Alpha. It's largely untested, some options may not work as expected, and some options might not work at all. If you notice anything not wortking properly, please file an [Issue](https://github.com/featherbutt/LiesOfPRandomizer/issues).

## Features
- Randomize many aspects of the game, including:
  - The locations of all items in the game, including weapons, armor, cosmetics, and upgrade materials
  - The abilities in the P-Organ upgrade tree
  - The scaling of weapons
- Add additional quality of life features to the game:
  - Allow boss weapons to be disassembled
  - Access levels 6 and 7 of the P-Organ upgrade tree without needing to enter NG+
  - Make the "Golden Lie" weapon always fully-grown.
  - And more!
- Highly configurable
- Set a seed for deterministic results.
- Print a human-readable mapping file that describes the changes made, ~~or apply a mapping file to an unmodded game~~ (Coming soon)
- No unpacking / repacking of assets required: just point the randomizer to the folder containing your game files and it does the rest, producing a pak file that can be dropped into the `~mods` folder in your game installation.
- (But it can also read and write unpacked asset files if you know what you're doing and want to combine this with other mods.)

The following features aren't supported yet, but may be added in the future:
- Randomized amulet abilities
- Randomized quest items
- Randomized fable arts

## OS Support

The randomizer can be run on both Windows and Mac. However, Mac users will have to unpack and repack manually. This is because of a dependency on https://github.com/trumank/repak, which does not have a prebuilt library for Apple Silicon. If you want to run this on Mac without unpacking and repacking assets, you'll need to install repak yourself, and build it from source if you're using ARM.

## Usage

To build, I recommend opening `LiesOfPRandomizer.sln` in Visual Studio or Visual Studio Code.

To run the randomizer, you'll need the .usmap file for the game. If you want the randomizer to read and write .pak files, you also need the game's encryption key. Neither is provided here.

You'll also need a config file sets the different parameters for randomization. An example config file is provided in `config.json`, feel free to modify it to your liking.

## Contributing

This randomizer uses a module system that makes it easy for anyone to add additional randomization elements. Looking at the existing modules should hopefully make it clear how they work.

Pull requests are welcome. If you have a feature request, please file an issue.

## License

LiesOfPRandomizer is distributed under the MIT license, which you can view in detail in the [LICENSE file](LICENSE).
