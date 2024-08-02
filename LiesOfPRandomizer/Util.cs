using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomizer;

public static class ExtensionMethods
{

    public static void Shuffle<T>(this List<T> input, System.Random random)
    {
        T[] values = input.ToArray();
        random.Shuffle(values);
        input.Clear();
        input.AddRange(values);
    }

    public static void AddMany<T>(this List<T> list, T value, uint count)
    {
        for (int i = 0; i < count; i++) { list.Add(value); }
    }

    public static byte[] HexToBytes(string hex)
    {
        var start = hex.Substring(0, 2) == "0x" ? 2 : 0;
        return Enumerable.Range(start, hex.Length-start)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }
}
