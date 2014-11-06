using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitAsyncTest
{
    /// <summary>
    /// Soll zeigen das Await/Async nichts anderes ist, als CPS-Style. Await gibt nämlich den Rest der Methode als Continuation an den Task weiter.
    /// Zeigen:
    /// CPS-Style -> Task.Factory.StartNew().ContinueWith() -> await/async
    /// </summary>
    class Program
    {
        private static int _numberOfBooks;

        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            DoSomethingAsync();

            Console.WriteLine("Finished (Counter = {0})", _numberOfBooks);
 
            Console.ReadLine();
        }

        static async void DoSomethingAsync()
        {
            var t1 = GetBookFromService("Ulysses");
            var t2 = GetBookFromService("Das Curry-Buch");


            // Ev. andere, unabhängige Berechnungen 

            await t1;
            Console.WriteLine("Buch empfangen: " + t1.Result);
            await t2;
            Console.WriteLine("Buch empfangen: " + t2.Result);
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
    }
}
