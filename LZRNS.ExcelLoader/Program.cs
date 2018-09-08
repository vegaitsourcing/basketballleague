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
            string basePath = @"F:\ForLoad\forUse\";
            
            string newDestination = @"F:\ForLoad\converted\";

            //ConvertExtensions(basePath, newDestination);
            LoadDocuments(newDestination);


        }

        public static void ConvertExtensions(string basePath, string newDestination)
        {
            IEnumerable<string> filesPaths = PathList(basePath, ".xls");
            
            Loger.log.Debug("Starting process for loading data for: " + filesPaths.Count() + " teams");
            foreach (String s in filesPaths)
            {
                //FileInfo fInfo = new FileInfo(basePath + s);

                //File.Move(basePath + s, Path.ChangeExtension(basePath + s, ".xlsx"));
                Excel.Application exApp = new Excel.Application();

                Excel.Workbook xlWorkbook = exApp.Workbooks.Open(basePath + s);

                string fileName = s.Substring(0, s.Length - 4);



                //xlWorkbook.SaveAs(basePath + "Milos.xlsx");


                Loger.log.Debug("Start main proces");
                xlWorkbook.SaveAs(newDestination + fileName, Excel.XlFileFormat.xlOpenXMLWorkbook,
                        System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false,
                        Excel.XlSaveAsAccessMode.xlShared, false, false, System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                //Workbook.LoadFromFile(basePath + s);
                //Workbook.SaveToFile("Output.xlsx", ExcelVersion.Version2013);

                //fInfo.MoveTo(Path.ChangeExtension(basePath + s, ".xlsx"));
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);
                exApp = null;
                GC.Collect();


            }
        }





        public static void LoadDocuments(string basePath)
        {
            ExcelLoader loader = new ExcelLoader();

            IEnumerable<string> filesPaths = PathList(basePath, ".xlsx");
           
            Loger.log.Debug("Starting process for loading data for: " + filesPaths.Count() + " teams");
            foreach (string s in filesPaths)
            {

                string fileName = s.Split(new string[] { "stats -teams-" }, StringSplitOptions.None).Last();
                string teamName = fileName.Substring(0, fileName.Length - 5);

                Loger.log.Debug("Start with processing team: " + teamName + ", file: " + basePath + s);
                Console.WriteLine("ProcessFile: Start with processing team: " + teamName + ", file: " + basePath + s);
                loader.ProcessFile(basePath + s, teamName);

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
