using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BulletinBoardStyle
{
    internal class StopWordFilter : BoardMember
    {
        private List<string> _stopWords;

        public StopWordFilter(BulletinBoard board)
            :base(board)
        {
            Board.Subscribe("load", OnLoad);
            Board.Subscribe("word", OnWord);
        }

        private void OnWord(object sender, DynamicEventArgs e)
        {
            var word = e.Data as string;
            if(!_stopWords.Contains(word))
                Board.Publish("valid_word", new DynamicEventArgs(word));
        }

        private void OnLoad(object sender, DynamicEventArgs e)
        {
            _stopWords = File.ReadAllLines(@"..\..\..\..\Resources\stop_words.txt")
                             .AsParallel()
                             .SelectMany(s => s.Split(',')).ToList();
        }
    }
}