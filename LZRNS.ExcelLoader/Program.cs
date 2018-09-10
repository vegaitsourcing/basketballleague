using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace LZRNS.ExcelLoader
{
    class Program
    {
            
        static void Main(string[] args)
        {

            //string basePath = @"F:\2.Documents\LZRNS\PrevoiusSesions\2017\";
            //string basePath = @"F:\ForLoad\forUse\";

            List<string> seasons = new List<string> { "2016", "2017", "2018" };

            string basePath = @"F:\ForLoad\2016\orginal\";

            //string newDestination = @"F:\ForLoad\converted\";
            string newDestination = @"F:\ForLoad\2016\converted\";

            foreach(String season in seasons)
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
            foreach (String s in filesPaths)
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
            ExcelLoader loader = new ExcelLoader("./TableMapper.config");

            IEnumerable<string> filesPaths = PathList(basePath, ".xlsx");
           
            Loger.log.Debug("Starting process for loading data for: " + filesPaths.Count() + " teams");

            Console.WriteLine("Starting process for loading data for: " + filesPaths.Count() + " teams");
            foreach (string s in filesPaths)
            {
                Loger.log.Debug("#################################################################################################");
                Console.WriteLine("Process team: " + s);
                loader.ProcessFile(basePath + s, s);
            }
        }


        static IEnumerable<string> PathList (string basePath, string extensionType)
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
