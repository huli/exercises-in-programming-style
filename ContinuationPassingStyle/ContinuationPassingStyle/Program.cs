using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContinuationPassingStyle
{
    internal class Program
    {
        private static Stopwatch _stopWatch;

        private static void Main(string[] args)
        {
            _stopWatch = Stopwatch.StartNew();

            ReadFile(
                allWords => FilterStopWords(allWords,
                    relevantWords => CalculateStatistics(relevantWords,
                                                            statistics => PrintStatistics(statistics, 
                                                                () => Console.ReadLine()))));
        }


        public static void ReadFile(Action<IEnumerable<string>> continuation)
        {
            var words = Directory.EnumerateFiles(@"..\..\..\..\Resources\Books")
                .SelectMany(File.ReadAllLines)
                .AsParallel()
                .Select(line => new string(line.Select(c =>
                    {
                        if (char.IsLetter(c))
                            return char.ToLower(c);
                        return ' ';
                    }).ToArray()))
                .SelectMany(line => line.Split(' ')).Where(word => !string.IsNullOrEmpty(word));

            continuation(words);
        }

        public static void FilterStopWords(IEnumerable<string> words, Action<IEnumerable<string>> continuation)
        {
            var stopWords = File.ReadAllLines(@"..\..\..\..\Resources\stop_words.txt")
                .SelectMany(s => s.Split(',')).ToList();
            var allWordsWithoutStopWords = words.Where(w => !stopWords.Any(sw => string.Equals(sw, w))).Select(w => w.ToLowerInvariant());

            continuation(allWordsWithoutStopWords);
        }

        public static void CalculateStatistics(IEnumerable<string> allWords,
                                        Action<IDictionary<string, int>> continuation)
        {
            var dictionary = new ConcurrentDictionary<string, int>();
            var allWordsEvaluated = allWords.ToList();

            allWordsEvaluated.AsParallel().ForAll(
                 word => dictionary.AddOrUpdate(word, 1, (_, count) => ++count));

            continuation(dictionary);
        }

        public static void PrintStatistics(IDictionary<string, int> heuristics, Action anyAction)
        {
            foreach (var keyValuePair in heuristics.OrderByDescending(e => e.Value).Take(25))
            {
                Console.WriteLine("{0}, {1}", keyValuePair.Key, keyValuePair.Value);
            }

            Console.WriteLine(Environment.NewLine + _stopWatch.Elapsed);

            anyAction();
        }
    }
}
