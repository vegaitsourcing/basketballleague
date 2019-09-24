using ClosedXML.Excel;
using LZRNS.DomainModels.ExcelLoaderModels;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class CodingListLoader : AbstractExcelLoader
    {
        public CodingListLoader(string configPath) : base(configPath)
        {
            PlayerInfoList = new List<PlayerInfo>();
        }

        public List<PlayerInfo> PlayerInfoList { get; set; }

        public override void Load(XLWorkbook exApp, string fileName)
        {
            CheckFileStructureAndExtractTeamName(exApp, fileName);
            var sheet = Sheets.FirstOrDefault();
            ProcessSheet(sheet);
        }

        protected override int CalculatePlayerCount(IXLRows exlRange, int currentRowNo, int columnIndex, int maxPlayerCount)
        {
            int playersCount = 0;

            int i = 0;
            while (GetCellValue(exlRange, currentRowNo + i, columnIndex) != null)
            {
                i++;
                playersCount++;
            }

            return playersCount;
        }

        /// <summary>
        /// Loads players from the worksheet and adds them to the PlayerInfoList property.
        /// </summary>
        private void ProcessSheet(IXLWorksheet sheet)
        {
            var rows = sheet.RowsUsed();

            int currentRowNo = Mapper.Fields[0].RowIndex;
            int headerRowNo = currentRowNo + 1;

            if (CheckIfPageIsEmpty(rows, headerRowNo, Mapper.Fields[0].ColumnIndex))
            {
                return;
            }

            var otherFields = Mapper.Fields.FindAll(fieldItem => fieldItem?.GlobalField == false);

            int playerCount = CalculatePlayerCount(rows, currentRowNo, otherFields[0].ColumnIndex, MaxPlayerPerMatch);

            for (int i = 1; i < playerCount; i++)
            {
                currentRowNo++;
                var pi = new PlayerInfo();
                PopulateModelField(pi, rows, otherFields, currentRowNo);
                PlayerInfoList.Add(pi);
            }
        }
    }
}