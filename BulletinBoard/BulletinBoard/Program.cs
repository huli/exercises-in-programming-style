using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoardStyle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
	        var stopwatch = new Stopwatch();
			stopwatch.Start();

            var board = new BulletinBoard();

            new WordFrequencyApplication(board);
            new DataStorage(board);
            new StopWordFilter(board);
            new WordFrequencyCounter(board);

            board.Publish("run", DynamicEventArgs.Empty);

			stopwatch.Stop();
			Console.WriteLine(Environment.NewLine + stopwatch.Elapsed);

            Console.ReadLine();
        }
    }
}
