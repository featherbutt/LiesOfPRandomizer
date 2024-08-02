using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomizer;

public class SkillConfig : Module.Config
{
    public Config.ShuffleMode skill_slots { get; set; } = Config.ShuffleMode.DONT_RANDOMIZE;
    public  Config.ShuffleMode skill_nodes { get; set; } = Config.ShuffleMode.DONT_RANDOMIZE;
    public bool include_amulets {  get; set; } = false;
    public uint max_organ_level {  get; set; } = 7;

    public bool remove_chapter_1_level_cap { get; set; } = false;
    public bool max_slots_per_node { get; set; } = false;
}
