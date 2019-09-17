using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace LZRNS.ExcelLoader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //string basePath = @"F:\2.Documents\LZRNS\PrevoiusSesions\2017\";
            //string basePath = @"F:\ForLoad\forUse\";

            List<string> seasons = new List<string> { "2016", "2017", "2018" };

            string basePath = @"F:\ForLoad\2016\orginal\";

            //string newDestination = @"F:\ForLoad\converted\";
            string newDestination = @"F:\ForLoad\2016\converted\";

            foreach (string season in seasons)
            {
                Console.WriteLine("Season: " + season);
                basePath = @"F:\ForLoad\" + season + @"\orginal\";
                newDestination = @"F:\ForLoad\" + season + @"\converted\";
                ConvertExtensions(basePath, newDestination);
            }

            LoadDocuments(newDestination);
        }

        public static void ConvertExtensions(string basePath, string newDestination)
        {
            IEnumerable<string> filesPaths = PathList(basePath, ".xls");

            Console.WriteLine("Starting process for loading data for: " + filesPaths.Count() + " teams");
            foreach (string s in filesPaths)
            {
                Excel.Application exApp = new Excel.Application();
                Excel.Workbook xlWorkbook = exApp.Workbooks.Open(basePath + s);

                string fileName = s.Substring(0, s.Length - 4);

                Console.WriteLine("Convert file: " + fileName);
                xlWorkbook.SaveAs(newDestination + fileName, Excel.XlFileFormat.xlOpenXMLWorkbook,
                        System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false,
                        Excel.XlSaveAsAccessMode.xlShared, false, false, System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);
                exApp = null;
                GC.Collect();
            }
        }

        public static void LoadDocuments(string basePath)
        {
            ExcelReader.ExcelLoader loader = new ExcelReader.ExcelLoader("./TableMapper.config");

            IEnumerable<string> filesPaths = PathList(basePath, ".xlsx");

            Log4NetLogger.Log.Debug("Starting process for loading data for: " + filesPaths.Count() + " teams");

            Console.WriteLine("Starting process for loading data for: " + filesPaths.Count() + " teams");
            foreach (string s in filesPaths)
            {
                Log4NetLogger.Log.Debug("#################################################################################################");
                Console.WriteLine("Process team: " + s);
                loader.ProcessFile(basePath + s, s);
            }
        }

        private static IEnumerable<string> PathList(string basePath, string extensionType)
        {
            IEnumerable<string> search = Directory.EnumerateFiles(basePath, "stats-teams-*" + extensionType)
                          .Where(file => Path.GetFileName(file).StartsWith("stats-teams-"))
                          .Select(path => Path.GetFileName(path))
                          .ToArray();

            if (search.Count() > 0)
            {
                return search.Where(x => !x.Contains("-playoff."));
            }
            return search;
        }
    }
}