using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomzier
{
    public class Config
    {
        public string seed { get; set; } = "";
        public WeaponConfig weapons { get; set; } = new WeaponConfig();
        public ItemConfig items { get; set; } = new ItemConfig();
        public SkillConfig skills { get; set; } = new SkillConfig();
    }

    public class WeaponConfig
    {
        public bool randomize_locations { get; set; } = false;
        public bool randomize_combinations { get; set; } = false;
        public bool randomize_scaling { get; set; } = false;
        public string minimum_advance_scaling { get; set; } = "None";
        public string disassemble_weapons { get; set; } = "default";

        public bool randomize_starting_weapons { get; set; } = false;

        public bool golden_lie_always_big { get; set; } = false;
    }

    public class ItemConfig
    {
        public bool randomize_locations { get; set; } = false;
        public bool double_boss_ergo { get; set; } = false;
        public uint total_full_moonstones { get; set; } = 6;
        public uint total_full_covenant_moonstones { get; set; } = 8;
        public uint total_crescent_covenant_moonstones { get; set; } = 33;
        public uint total_quartz { get; set; } = 31;
        public uint total_motivity_cranks { get; set; } = 9;
        public uint total_technique_cranks { get; set; } = 9;
        public uint total_advance_cranks { get; set; } = 9;
        public uint total_balance_cranks { get; set; } = 12;
        public uint total_legion_caliber {  get; set; } = 17;

        public bool find_legion_arms {  get; set; } = false;
        public bool include_ngp_equipment {  get; set; } = false;

        public double chaos { get; set; } = 0.0;
        public bool randomize_default_costume {  get; set; } = false;

    }

    public class SkillConfig
    {
        public string skill_slots { get; set; } = "";
        public  string skill_nodes { get; set; } = "";
        public bool include_amulets {  get; set; } = false;
        public uint max_organ_level {  get; set; } = 7;

        public bool remove_chapter_1_level_cap { get; set; } = false;
        public bool max_slots_per_node { get; set; } = false;
    }
}
