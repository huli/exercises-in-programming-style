using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;

namespace ReactiveExtensionStyle
{
    public class WordSource : IObservable<string>
    {
        private readonly string _fileName;
        private readonly List<string> _stopWords;

        public WordSource(string fileName, List<string> stopWords)
        {
            _fileName = fileName;
            _stopWords = stopWords;
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            File.ReadAllLines(_fileName)
                .AsParallel()
                .Select(line => new string(line.Select(c =>
                    {
                        if (char.IsLetter(c))
                            return char.ToLower(c);
                        return ' ';
                    }).ToArray()))
                .SelectMany(line => line.Split(' '))
                .Where(word => !string.IsNullOrEmpty(word))
                .Where(word => !_stopWords.Contains(word))
                .ForAll(observer.OnNext);

            return Disposable.Empty;
        }
    }
}