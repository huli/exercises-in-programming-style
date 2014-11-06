using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BulletinBoardStyle
{
    internal class BoardMember
    {
        public BulletinBoard Board { get; set; }

        public BoardMember(BulletinBoard board)
        {
            Board = board;
        }
    }

    internal class DataStorage : BoardMember
    {
        private IEnumerable<string> _words;

        public DataStorage(BulletinBoard board) 
            :base(board)
        {
            Board.Subscribe("load", OnLoad);
            Board.Subscribe("start", OnCreateWords);
        }

        private void OnCreateWords(object sender, DynamicEventArgs e)
        {
            foreach (var word in _words)
            {
                Board.Publish("word", new DynamicEventArgs(word));
            }
            Board.Publish("eof", DynamicEventArgs.Empty);
        }

        private void OnLoad(object sender, DynamicEventArgs e)
        {
            _words  = Directory.EnumerateFiles(@"..\..\..\..\Resources\Books")
                .SelectMany(File.ReadAllLines)
                .Select(line => new string(line.Select(c =>
                    {
                        if (char.IsLetter(c))
                            return char.ToLower(c);
                        return ' ';
                    }).ToArray()))
                .SelectMany(line => line.Split(' ')).Where(word => !string.IsNullOrEmpty(word));
        }
    }
}