using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveExtensionStyle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();

            var stopWords = File.ReadAllLines(@"..\..\..\..\Resources\stop_words.txt")
                                       .SelectMany(s => s.Split(','))
                                       .ToList();

            var statistics = new WordStatistics();

            Directory.EnumerateFiles(@"..\..\..\..\Resources\Books")
                        .AsParallel()
                        .ToObservable()
                        .Subscribe(file => new WordSource(file, stopWords)
                            .Subscribe(statistics.AddWord));


            new StatisticsPrinter(statistics).Print();

			Console.WriteLine(Environment.NewLine + stopwatch.Elapsed);
            Console.ReadLine();
        }
    }
}
