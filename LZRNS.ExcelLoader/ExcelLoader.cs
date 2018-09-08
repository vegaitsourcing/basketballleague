using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace LZRNS.ExcelLoader
{
    class ExcelLoader
    {
        #region Private Fields
        private Excel.Application exApp;
        private Excel.Workbook xlWorkbook;
        private Excel.Sheets sheets;

        // key represent team name (I will change that later)
        private Dictionary<string, TeamStatistic> teams;

        private MapperModel mapper;
        private int maxPlayerPerMatch = 12;

        #endregion Private Fields

        #region Constructors
        public ExcelLoader() {
            this.exApp = new Excel.Application();
            this.teams = new Dictionary<string, TeamStatistic>();
            this.mapper = new MapperModel("../../TableMapper.config");

        }
        #endregion Constructors

        #region Public Methods
        public void ProcessFile (String filePath, String teamName)
        {
            try
            {
                int currentSheetNo = 0;
                exApp = new Excel.Application();
                xlWorkbook = exApp.Workbooks.Open(filePath);
                
                sheets = xlWorkbook.Sheets;
                
                //Only for first sheet we want to check validation for mapping fields configuration
                foreach (Excel.Worksheet sheet in sheets)
                {
                    CheckMappingValidation(mapper, sheet);
                    break;
                }

                TeamStatistic teamStatistic = new TeamStatistic(teamName);
            
                foreach (Excel.Worksheet sheet in sheets)
                {
                    // for odd sheet we want to skip loading
                    if (currentSheetNo % 2 == 0)
                    {
                        ProcessSheet(sheet, ref teamStatistic);
                    }
                    currentSheetNo++;
                }

            }
            finally
            {
                xlWorkbook.Close();
                Marshal.ReleaseComObject(sheets);
                Marshal.ReleaseComObject(xlWorkbook);
                
                Marshal.ReleaseComObject(exApp);
                sheets = null;
                xlWorkbook = null;
                exApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
          
        }
        
        #endregion Public Methods

        #region Private Methods

        private void CheckMappingValidation (MapperModel mapper, Excel.Worksheet sheet)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Excel.Range exlRange = sheet.UsedRange;
            foreach (FieldItem item in mapper.Fields)
            {
                if (item.CellName.Equals("")) continue;
              
                object value =  exlRange.Cells[item.RowIndex, item.ColumnIndex].Value;
                
                if (value == null || !value.Equals(item.CellName))
                {

                    Loger.log.Error("Mapping is invalid for sheet: " + sheet.Name);
                    throw new Exception();
                }
            }
            stopwatch.Stop();
            Loger.log.Debug("CheckMappingValidation Successfully completed for time: " + stopwatch.Elapsed);

        }

        private void ProcessSheet(Excel.Worksheet sheet, ref TeamStatistic teamStatistic)
        {
            Loger.log.Debug("ProcessSheet started for table: " + sheet.Name);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //worksheet.Name;
            Excel.Range exlRange = sheet.UsedRange;


            TeamScore teamScore = new TeamScore();
            teamScore.RoundName = sheet.Name;

            IEnumerable<FieldItem> globalFields = mapper.Fields.FindAll(i => i.GlobalField == true);
            
            int currentRowNo = mapper.Fields.First().RowIndex;

            PopulateModelField(teamScore, exlRange, globalFields, ++currentRowNo);


            // When we start to load data for each player, we must take row number of headers and then increase it for 1
            //currentRowNo = mapper.Fields.First().RowIndex;
            IEnumerable<FieldItem> otherFields = mapper.Fields.FindAll(i => i.GlobalField == false);
            int playerCount = CalculatePlayerCount(exlRange, currentRowNo, otherFields.First().ColumnIndex, maxPlayerPerMatch);

            for (int i = 0; i < playerCount; i++)
            {
                PlayerScore pl = new PlayerScore();
                PopulateModelField(pl, exlRange, otherFields, currentRowNo);
                teamScore.AddPlayerScore(pl);
                currentRowNo++;

            }

            Loger.log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);

            stopwatch.Stop();

        }

        public void PopulateModelField(Object modelObj, Excel.Range exlRange, IEnumerable<FieldItem> fields, int rowIndex = -1)
        {
            foreach(FieldItem fieldItem in fields)
            {
                PopulateModelValue(modelObj, exlRange, fieldItem, rowIndex);
            }
        }

        public void PopulateModelValue(Object obj, Excel.Range exlRange, FieldItem fieldItem, int rowIndex = -1)
        {
            var objProperty = obj.GetType().GetProperty(fieldItem.PropertyName, BindingFlags.Public | BindingFlags.Instance);
            //get the value of the property
            if (objProperty == null)
            {
                Loger.log.Error("PopulateModelValue: PropertyName: " + fieldItem.PropertyName + " not exist in model: " + obj);
                // We should log error (currently I will just throw exception to be aware that model and mapping are not matched)
                throw new Exception();
            }
            
            // If we do not explcitly send row index or data should be directly read from cell, then it should be used directly from configuration
            // Examlple: matchDate doesn not contain header
            if (fieldItem.DirectCellData == true || rowIndex == -1) {
                rowIndex = fieldItem.RowIndex;
            }

            Object value = GetCellValue(exlRange, rowIndex, fieldItem.ColumnIndex);
            if(value == null)
            {
                Loger.log.Error("PopulateModelValue - PropertyName: " + fieldItem.PropertyName + " in NULL");
                return;
            }

            objProperty.SetValue(obj, fieldItem.GetValueConverted(value));

        }

        public Object GetCellValue(Excel.Range exlRange, int rowIndex, int columnIndex)
        {
            Object obj = exlRange.Cells[columnIndex][rowIndex].Value;
            
            return obj;
        }

        /* This method used to calculate number of rows that are populated for players statistic */
        public int CalculatePlayerCount(Excel.Range exlRange, int currentRowNo, int columnIndex, int maxPlayerCount)
        {
            int playersCount = 0;
            for (int i = 0; i < maxPlayerCount; i++)
            {
                if (GetCellValue(exlRange, currentRowNo + i, columnIndex) == null)
                {
                    break;
                }

                playersCount++;
            }

            return playersCount;
            
        }


        #endregion Private Methods



    }
}
