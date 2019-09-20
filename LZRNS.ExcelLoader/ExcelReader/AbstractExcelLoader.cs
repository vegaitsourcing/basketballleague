using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public abstract class AbstractExcelLoader
    {
        protected AbstractExcelLoader(string configPath)
        {
            Log4NetLogger.Log.Debug("Start main process");
            TeamAndPlayers = new Dictionary<string, HashSet<string>>();
            Mapper = new MapperModel(configPath);
        }

        public string LeagueName { get; set; }

        public string SeasonName { get; set; }

        public Dictionary<string, HashSet<string>> TeamAndPlayers { get; } // key represents team name, list of player names

        protected XLWorkbook ExApp { get; set; }

        protected MapperModel Mapper { get; set; }

        protected int MaxPlayerPerMatch { get; } = 12;
        protected IXLWorksheets Sheets { get; set; }

        public abstract void Load(XLWorkbook exApp, string fileName);

        public void ProcessFile(string path, string fileName)
        {
            try
            {
                using (ExApp = new XLWorkbook(path))
                {
                    Load(ExApp, fileName);
                }
            }
            finally
            {
                Sheets = null;
                ExApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void ProcessFile(MemoryStream memoryStream, string fileName)
        {
            try
            {
                using (ExApp = new XLWorkbook(memoryStream))
                {
                    Load(ExApp, fileName);
                }
            }
            finally
            {
                Sheets = null;
                ExApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        protected void AddPlayerInTeam(string team, string playerName)
        {
            if (TeamAndPlayers.TryGetValue(team, out var players))
            {
                players.Add(playerName);
            }
            else
            {
                TeamAndPlayers[team] = new HashSet<string>() { playerName };
            }
        }

        protected virtual int CalculatePlayerCount(IXLRows exlRange, int currentRowNo, int columnIndex, int maxPlayerCount)
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

        /// <summary>
        /// Throws exception if file is in invalid format. If valid will extract and return teamName.
        /// In case @fileName contains Season and League name information, properties `SeasonName` and `LeagueName` will be set.
        /// </summary>
        protected string CheckFileStructureAndExtractTeamName(XLWorkbook exApp, string fileName)
        {
            var nameParts = fileName.Split('-');
            string teamName = nameParts[2];

            if (teamName.Contains(".xlsx"))
            {
                teamName = teamName.Substring(0, teamName.Length - 5);
            }

            if (nameParts.Length == 5 && SeasonName == null && LeagueName == null)
            {
                SeasonName = nameParts[3];
                LeagueName = nameParts[4].Substring(0, nameParts[4].Length - 5);
            }

            Log4NetLogger.Log.Debug("Loading data for team: " + teamName);

            Sheets = exApp.Worksheets;

            var sheet = Sheets.FirstOrDefault();
            if (sheet != null)
            {
                CheckMappingValidation(Mapper, sheet);
            }

            return teamName;
        }

        protected bool CheckIfPageIsEmpty(IXLRows rows, int rowIndex, int columnIndex)
        {
            return GetCellValue(rows, rowIndex, columnIndex) == null;
        }

        protected void CheckMappingValidation(MapperModel mapper, IXLWorksheet sheet)
        {
            var rows = sheet.RowsUsed();
            int currentRowIndex = -1;
            IXLRow row = null;

            foreach (var item in mapper.Fields.Where(item => !item.CellName.Equals("")))
            {
                if (currentRowIndex != item.RowIndex)
                {
                    row = rows.ElementAt(item.RowIndex);
                    currentRowIndex = item.RowIndex;
                }

                var value = row?.Cell(item.ColumnIndex).Value;

                if (value?.Equals(item.CellName) != true)
                {
                    string errorMessage = $"{row}:Expected: {item.CellName} but got {value}";
                    Log4NetLogger.Log.Error(errorMessage);
                    throw new Exception(errorMessage);
                }
            }
        }

        protected object GetCellValue(IXLRows rows, int rowIndex, int columnIndex)
        {
            if (rows.Count() <= rowIndex)
            {
                return null;
            }
            var row = rows.ElementAt(rowIndex);
            return row.Cell(columnIndex)?.Value;
        }

        protected void PopulateModelField(object modelObj, IXLRows exlRange, IEnumerable<FieldItem> fields, int rowIndex = -1)
        {
            foreach (var fieldItem in fields)
            {
                PopulateModelValue(modelObj, exlRange, fieldItem, rowIndex);
            }
        }

        private void PopulateModelValue(object obj, IXLRows exlRange, FieldItem fieldItem, int rowIndex = -1)
        {
            var objProperty = obj.GetType().GetProperty(fieldItem.PropertyName, BindingFlags.Public | BindingFlags.Instance);

            if (objProperty == null)
            {
                Log4NetLogger.Log.Error("PopulateModelValue: PropertyName: " + fieldItem.PropertyName + " not exist in model: " + obj);
                return;
            }

            if (fieldItem.DirectCellData || rowIndex == -1)
            {
                rowIndex = fieldItem.RowIndex;
            }

            var value = GetCellValue(exlRange, rowIndex, fieldItem.ColumnIndex);
            if (value == null)
            {
                Log4NetLogger.Log.Error("PopulateModelValue - PropertyName: " + fieldItem.PropertyName + " in NULL");
                return;
            }

            objProperty.SetValue(obj, fieldItem.GetValueConverted(value));
        }
    }
}