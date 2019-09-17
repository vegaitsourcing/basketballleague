using ClosedXML.Excel;
using LZRNS.ExcelLoader.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class ExcelAnalyzer : AbstractExcelLoader
    {
        public Dictionary<string, Dictionary<string, List<PlayerInfo>>> TeamPlayerInfos { get; set; }

        public ExcelAnalyzer(string configPath) : base(configPath)
        {
            Log4NetLogger.Log.Debug("Start main process");
            TeamPlayerInfos = new Dictionary<string, Dictionary<string, List<PlayerInfo>>>();
        }

        public override void Load(XLWorkbook exApp, string fileName)
        {
            string teamName = CheckFileStructureAndExtractTeamName(exApp, fileName);
            int currentSheetNo = 0;

            foreach (var sheet in Sheets)
            {
                if (currentSheetNo % 2 == 0)
                {
                    ProcessSheet(sheet, teamName);
                }
                currentSheetNo++;
            }
        }

        private void ProcessSheet(IXLWorksheet sheet, string teamName)
        {
            Log4NetLogger.Log.Debug("ProcessSheet started for table: " + sheet.Name);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var rows = sheet.RowsUsed();

            int currentRowNo = Mapper.Fields[0].RowIndex;
            int headerRowNo = currentRowNo + 1;

            if (CheckIfPageIsEmpty(rows, headerRowNo, Mapper.Fields[0].ColumnIndex))
            {
                return;
            }

            IEnumerable<FieldItem> otherFields = Mapper.Fields.FindAll(i => i?.GlobalField == false);

            int playerCount = CalculatePlayerCount(rows, currentRowNo, otherFields.First().ColumnIndex, MaxPlayerPerMatch);

            for (int i = 1; i < playerCount; i++)
            {
                currentRowNo++;
                var pi = new PlayerInfo();
                PopulateModelField(pi, rows, otherFields, currentRowNo);
                AddPlayerInfo(teamName, pi);
            }

            stopwatch.Stop();

            Log4NetLogger.Log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);
        }

        private void AddPlayerInfo(string teamName, PlayerInfo pi)
        {
            if (pi.NameAndLastName.Equals(string.Empty))
                return;

            if (!TeamPlayerInfos.TryGetValue(teamName, out var infoDict))
            {
                TeamPlayerInfos.Add(teamName, new Dictionary<string, List<PlayerInfo>> { { pi.NameAndLastName, new List<PlayerInfo>() } });
                return;
            }

            if (infoDict.TryGetValue(pi.NameAndLastName, out var list))
            {
                list.Add(pi);
            }
            else
            {
                TeamPlayerInfos[teamName].Add(pi.NameAndLastName, new List<PlayerInfo> { pi });
            }
        }
    }
}