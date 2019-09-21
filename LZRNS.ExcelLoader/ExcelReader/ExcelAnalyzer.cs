using ClosedXML.Excel;
using LZRNS.ExcelLoader.Model;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class ExcelAnalyzer : AbstractExcelLoader
    {
        public Dictionary<string, Dictionary<string, List<PlayerInfo>>> PlayerInfoListByPlayerNameAndLastNameByTeamName { get; set; }

        public ExcelAnalyzer(string configPath) : base(configPath)
        {
            PlayerInfoListByPlayerNameAndLastNameByTeamName = new Dictionary<string, Dictionary<string, List<PlayerInfo>>>();
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
        }

        private void AddPlayerInfo(string teamName, PlayerInfo pi)
        {
            if (pi.NameAndLastName.Equals(string.Empty))
                return;

            if (!PlayerInfoListByPlayerNameAndLastNameByTeamName.TryGetValue(teamName, out var infoDict))
            {
                PlayerInfoListByPlayerNameAndLastNameByTeamName.Add(teamName, new Dictionary<string, List<PlayerInfo>> { { pi.NameAndLastName, new List<PlayerInfo>() } });
                return;
            }

            if (infoDict.TryGetValue(pi.NameAndLastName, out var list))
            {
                list.Add(pi);
            }
            else
            {
                PlayerInfoListByPlayerNameAndLastNameByTeamName[teamName].Add(pi.NameAndLastName, new List<PlayerInfo> { pi });
            }
        }
    }
}