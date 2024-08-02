using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomizer;

public record class SkillMap(
    List<string> slots,
    List<string> nodes) : Module.Map<SkillModule> {}