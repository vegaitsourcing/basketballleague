using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader
{
    class Program
    {
            
        static void Main(string[] args)
        {
           
            Console.WriteLine("Processing is started");
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            ExcelLoader loader = new ExcelLoader();
            Loger.log.Debug("Start main proces");

            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();
            //IEnumerable<string> filesPaths = PathList();
            string path = @"F:\2.Documents\stats - teams - airdjevrek.xlsx";
            List<string> filesPaths = new List<string>() { path };
            //string basePath = @"F:\2.Documents\LZRNS\PrevoiusSesions\2018\";
            string basePath = "";
            Loger.log.Debug("Starting process for loading data for: " + filesPaths.Count() + " teams");
            foreach (string s in filesPaths)
            {
                
                string fileName  = s.Split(new string[] { "stats -teams-" }, StringSplitOptions.None).Last();
                string teamName = fileName.Substring(0, fileName.Length - 4);

                Loger.log.Debug("Start with processing team: " + teamName + ", file: " + basePath + s);
                Console.WriteLine("ProcessFile: Start with processing team: " + teamName + ", file: " + basePath + s);
                loader.ProcessFile(basePath + s, teamName);
                
            }
            stopwatch.Stop();

            Loger.log.Debug("Compleated loading data for: " + filesPaths.Count() + " teams");

           
            Loger.log.Debug("End main proces. Time elapsed: " + stopwatch.Elapsed);

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
            
        }

        static IEnumerable<string> PathList ()
        {
           IEnumerable<string> search = Directory.EnumerateFiles(@"F:\2.Documents\LZRNS\PrevoiusSesions\2018\", "stats-teams-*.xls")
                         .Where(file => Path.GetFileName(file).StartsWith("stats-teams-"))
                         .Select(path => Path.GetFileName(path))
                         .ToArray();

            
           

            if (search.Count() > 0)
            {
                return search.Where(x => !x.Contains("-playoff."));
               // return search.FindAll(n => !n.Contains("-playoff."));
            }
            return search;
            
        }
    }
}
