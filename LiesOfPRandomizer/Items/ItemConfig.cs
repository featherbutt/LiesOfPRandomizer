namespace LiesOfPRandomizer;

public class ItemConfig : Module.Config
{

    public enum PatchGifts
    {
        Default, // Use the default gifts from the game.
        Random, // If randomize_locations is true, include the gift items in the pool. Otherwise, behaves the same as "default"
        None // Remove the gifts.
    }
    public PatchGifts patch_gift { get; set; } = PatchGifts.Default;

    // Randomize the locations of items.
    public bool randomize_locations { get; set; } = false;

    // If true, items and weapons will be shuffled together.
    // If false, weapons will only replace other weapons.
    // This flag does nothing unless both items and weapons are randomized.
    public bool randomize_weapons_with_items { get; set; } = true;

    // Include two sets of boss ergo, so that both rewards can be claimed from Alidoro.
    public bool double_boss_ergo { get; set; } = false;

    // The total number of full moonstones to appear in the game (unmodded game has 6)
    public uint total_full_moonstones { get; set; } = 6;

    // The total number of full moonstones of the covenant to appear in the game (unmodded game has 8)
    public uint total_full_covenant_moonstones { get; set; } = 8;

    // The total number of non-random crescent moonstones of the covenant to appear in the game (unmodded game has 33)
    public uint total_crescent_covenant_moonstones { get; set; } = 33;

    // The total number of quartz to appear in the game (unmodded game has 31)
    public uint total_quartz { get; set; } = 31;

    // The total number of motivity cranks to appear in the game (unmodded game has 9)
    public uint total_motivity_cranks { get; set; } = 9;

    // The total number of technique cranks to appear in the game (unmodded game has 9)
    public uint total_technique_cranks { get; set; } = 9;

    // The total number of advance cranks to appear in the game (unmodded game has 9)
    public uint total_advance_cranks { get; set; } = 9;

    // The total number of balance cranks to appear in the game (unmodded game has 12)
    public uint total_balance_cranks { get; set; } = 12;

    // The total number of legion caliber to appear in the game (unmodded game has 17)
    // This only does anything if find_legion_arms is false.
    public uint total_legion_caliber {  get; set; } = 17;

    // If true, the Legion Plugs in the item pool will be replaced with the individual Legion Arms
    public bool find_legion_arms {  get; set; } = false;

    // If true, equipment that usually only appears in NG+ can appear.
    public bool include_ngp_equipment {  get; set; } = false;

    // A measure of how much item drops can change from base game, in terms of value.
    // For example:
    // - A chaos value of 0.0 means that whenver possible, high value items (weapons, quartz, etc)
    //   replace other high value items.
    // - A chaos value of 1.0 means that all items are equally likely to replace each other.
    public double chaos { get; set; } = 0.0;

    // If true, randomizes the starting outfit.
    public bool randomize_default_costume {  get; set; } = false;

}
