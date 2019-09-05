using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using LZRNS.ExcelLoader.Model;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class CodingListLoader : AbstractExcelLoader
    {
        #region Private Fields
        private List<PlayerInfo> _playerInfoList;
        #endregion

        #region Properties
        public List<PlayerInfo> PlayerInfoList { get; set; }

        #endregion
        #region Constructors
        public CodingListLoader(string configPath) : base(configPath)
        {
            PlayerInfoList = new List<PlayerInfo>();
        }
        #endregion
        public override void Load(XLWorkbook exApp, string fileName)
        {
            string teamName = CheckFileStructure(exApp, fileName);
            bool isPageEmpty;
            foreach (IXLWorksheet sheet in Sheets)
            {

                ProcessSheet(sheet, out isPageEmpty);
                break;

            }

        }

        protected int CalculatePlayerCount(IXLRows exlRange, int currentRowNo, int columnIndex, int maxPlayerCount)
        {
            int playersCount = 0;
            int i = 0;
            while (true)
            {
                if (GetCellValue(exlRange, currentRowNo + i, columnIndex) == null)
                {
                    break;
                }
                i++;
                playersCount++;
            }

            return playersCount;

        }

        private void ProcessSheet(IXLWorksheet sheet, out bool isEmptyPage)
        {
            Loger.log.Debug("ProcessSheet started for table: " + sheet.Name);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            isEmptyPage = false;

            IXLRows rows = sheet.RowsUsed();
            int currentRowNo;

            IEnumerable<FieldItem> globalFields = Mapper.Fields.FindAll(i => i.GlobalField == true);
            currentRowNo = Mapper.Fields.First().RowIndex;

            // we are incrasing rowNo because currentRow reprenset row where is header placed!
            if (CheckIfPageIsEmty(rows, currentRowNo + 1, Mapper.Fields.First().ColumnIndex))
            {
                //check this
                isEmptyPage = true;
                return;
            }


            // When we start to load data for each player, we must take row number of headers and then increase it for 1
            currentRowNo = Mapper.Fields.First().RowIndex;
            IEnumerable<FieldItem> otherFields = Mapper.Fields.FindAll(i => i.GlobalField == false);
            int playerCount = CalculatePlayerCount(rows, currentRowNo, otherFields.First().ColumnIndex, MaxPlayerPerMatch);

            for (int i = 1; i < playerCount; i++)
            {
                currentRowNo++;
                PlayerInfo pi = new PlayerInfo();
                PopulateModelField(pi, rows, otherFields, currentRowNo);
                PlayerInfoList.Add(pi);
            }

            Loger.log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);

            stopwatch.Stop();
        }



    }
}
