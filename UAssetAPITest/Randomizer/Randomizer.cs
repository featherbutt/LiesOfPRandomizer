using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.Unversioned;

namespace LiesOfPRandomzier;
public partial class Randomizer { 

    private AssetManager assets { get; init; }
    private Config config {  get; init; }
    private Random random { get; init; }

    public Randomizer(Config config, AssetManager assetManager)
    {
        int seed = (config.seed != "") ? config.seed.GetHashCode() : new Random().Next();

        random = new Random(seed);
        this.assets = assetManager;
        this.config = config;
    }

}