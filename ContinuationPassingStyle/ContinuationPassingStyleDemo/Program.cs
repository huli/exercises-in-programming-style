using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPSTest
{
    /// <summary>
    /// Soll die Verwendung von CPS-Style im Task-Namespace von .NET 4.5 verdeutlichen. (Schritt CPS-Syle -> Task.Factory.StartNew().ContinueWith())
    /// </summary>
    class Program
    {
        private static int _numberOfBooks = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Start Main");

            Action<string> continuation = bookName =>
            {

                _numberOfBooks++;
                WriteBook(bookName);
            };

            // CPS-Style
            GetBookFromServiceCPSStyle("Design Patterns for Dummies", continuation);

            // CPS-Style in TPL
            GetBookFromServiceTPLStyle("Ein endlose geflochtene Band", continuation);

            // Await/Async
            GetBookFromServiceAwaitAsyncStyle("Das Curry-Buch");

            Console.WriteLine("End Main (Counter = {0})", _numberOfBooks);

            Console.ReadLine();
        }

        private static void WriteBook(string bookName)
        {
            Console.WriteLine("Buch empfangen: {0}", bookName);
            Console.WriteLine("(Counter = {0})", _numberOfBooks);
        }

        private static async void GetBookFromServiceAwaitAsyncStyle(string bookName)
        {
            var t1 = GetBookFromService(bookName);
            
            // Ev. andere, unabhängige Berechnungen 

            await t1;
            WriteBook(t1.Result);
        }

        static Task<string> GetBookFromService(string bookName) 
        {
            return Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    _numberOfBooks++;
                    return bookName;
                });
        }

        static void GetBookFromServiceCPSStyle(string buchName, Action<string> continuation)
        {
            Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    continuation(buchName);
                });
        }

        static void GetBookFromServiceTPLStyle(string bookName, Action<string> continuation)
        {
            Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    return bookName;
                }).ContinueWith(t => continuation(t.Result));  // promise pipelining
        }
    }
}
