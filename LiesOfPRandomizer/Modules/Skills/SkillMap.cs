namespace LiesOfPRandomizer;

public record class SkillMap(
    List<string> slots,
    List<string> nodes) : Module.Map<SkillModule> {}