using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveExtensionStyle
{
    internal class StatisticsPrinter
    {
        private readonly IEnumerable<KeyValuePair<string, int>> _statistics;

        public StatisticsPrinter(IEnumerable<KeyValuePair<string, int>> statistics)
        {
            _statistics = statistics;
        }

        public void Print()
        {
            foreach (var word in _statistics.OrderByDescending(stats => stats.Value).Take(25))
            {
                Console.WriteLine("{0}: {1}", word.Key, word.Value);
            }
        }
    }
}