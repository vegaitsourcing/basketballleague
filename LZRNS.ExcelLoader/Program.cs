using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            ExcelLoader loader = new ExcelLoader();

            Stopwatch stopwatch = new Stopwatch();
            
            // Begin timing.
            stopwatch.Start();
            loader.ProcessFile(@"F:\stats-teams-orlovi.xls", "Orlovi");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            Console.ReadLine();
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
            
        }
    }
}
