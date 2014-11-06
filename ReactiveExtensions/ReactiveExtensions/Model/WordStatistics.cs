using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ReactiveExtensionStyle
{
    public class WordStatistics : IEnumerable<KeyValuePair<string, int>>
    {
        ConcurrentDictionary<string, int> _statistics = new ConcurrentDictionary<string, int>();
 
        public void AddWord(string word)
        {
            _statistics.AddOrUpdate(word, 1, (_, counter) => ++counter);
        }

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return new List<KeyValuePair<string, int>>(_statistics).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}