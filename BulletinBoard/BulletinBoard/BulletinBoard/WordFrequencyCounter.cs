using System;
using System.Collections.Concurrent;
using System.Linq;

namespace BulletinBoardStyle
{
    internal class WordFrequencyCounter : BoardMember
    {
        private ConcurrentDictionary<string, int> _wordFrequency = new ConcurrentDictionary<string, int>();  

        public WordFrequencyCounter(BulletinBoard board)
            :base(board)
        {
            Board.Subscribe("valid_word", OnValidWord);
            Board.Subscribe("print", OnPrint);
        }

        private void OnPrint(object sender, DynamicEventArgs args)
        {
            var frequencies = _wordFrequency.OrderByDescending(f => f.Value).Take(25);
            foreach (var frequency in frequencies)
            {
                Console.WriteLine("{0}, {1}", frequency.Key, frequency.Value);
            }
        }

        private void OnValidWord(object sender, DynamicEventArgs args)
        {
            var word = (string)args.Data;
            _wordFrequency.AddOrUpdate(word, 1, (_, count) => ++count);
        }
    }
}