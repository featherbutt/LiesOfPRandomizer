namespace LiesOfPRandomizer;

public class WeaponConfig : Module.Config
{
    public bool randomize_locations { get; set; } = false;

    public enum RandomizeWeaponCombinationOptions {
        ALL,
        NONE,
        NON_BOSS

    }
    public RandomizeWeaponCombinationOptions randomize_combinations { get; set; } = RandomizeWeaponCombinationOptions.NONE;
    public bool randomize_scaling { get; set; } = false;

    public enum Scaling {
        A,
        B,
        C,
        D,
        NULL
    }

    public Scaling minimum_advance_scaling { get; set; } = Scaling.NULL;
    
    public enum DisassembleWeaponsOptions {
        DEFAULT,
        ALWAYS,
        NEVER
    }

    public DisassembleWeaponsOptions disassemble_weapons { get; set; } = DisassembleWeaponsOptions.DEFAULT;

    public bool randomize_starting_weapons { get; set; } = false;

    public bool golden_lie_always_big { get; set; } = false;
}