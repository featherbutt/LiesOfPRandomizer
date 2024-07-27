using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiesOfPRandomzier
{
    public static class TieredShuffler<T> 
    {
        public static IEnumerable<T> Loop(IEnumerable<T> source) {
            while (true)
            {
                foreach (T t in source)
                {
                    yield return t;
                }
            }
        }

        public static IEnumerable<T> Create(IEnumerable<T> sources, int bufferSize, int totalSize)
        {
            var random = new Random();
            var enumerator = sources.GetEnumerator();
            enumerator.MoveNext();
            var buffer = new List<T>();
            while (buffer.Count < bufferSize)
            {
                buffer.Add(enumerator.Current);
                enumerator.MoveNext();
            }
            var returned = 0;
            while (returned < totalSize - bufferSize)
            {
                var selectedIndex = random.Next(bufferSize);
                var result = buffer[selectedIndex];
                buffer[selectedIndex] = enumerator.Current;
                enumerator.MoveNext();
                returned++;
                yield return result;
            }
            var remaining = buffer.ToArray();
            random.Shuffle(remaining);
            foreach ( var item in remaining )
            {
                yield return item;
            }
        }
    }
}
