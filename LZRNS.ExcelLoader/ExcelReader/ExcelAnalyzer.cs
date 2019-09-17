using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using LZRNS.ExcelLoader.Model;
using System.Diagnostics;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class ExcelAnalyzer : AbstractExcelLoader
    {

        #region Private Fields

        private const int _maxPlayerPerMatch = 12;

        private XLWorkbook _exApp;
        private IXLWorksheets _sheets;
        private Dictionary<string, Dictionary<string, List<PlayerInfo>>> _teamPlayerInfos;

        #endregion

        #region Properties
        public Dictionary<string, Dictionary<string, List<PlayerInfo>>> TeamPlayerInfos
        {
            get { return _teamPlayerInfos; }
            set { _teamPlayerInfos = value; }
        }
        #endregion Properties



        #region Constructors
        public ExcelAnalyzer(string configPath) : base(configPath)
        {
            Loger.log.Debug("Start main proces");
            _teamPlayerInfos = new Dictionary<string, Dictionary<string, List<PlayerInfo>>>();

        }
        #endregion Constructors

        #region Public Methods


        public override void Load(XLWorkbook exApp, string fileName)
        {
            //fileName should have the following format - teamName-seasonName-leagueName.xlsx (at least one file)
            string teamName = CheckFileStructureAndExtractTeamName(exApp, fileName);
            int currentSheetNo = 0;
            bool isPageEmpty;

            foreach (IXLWorksheet sheet in Sheets)
            {
                // for odd sheet we want to skip loading
                if (currentSheetNo % 2 == 0)
                {
                    ///process sheet by sheet
                    ProcessSheet(sheet, teamName, out isPageEmpty);

                }
                currentSheetNo++;
            }

        }

        #endregion

        #region Private Methods
        private void ProcessSheet(IXLWorksheet sheet, string teamName, out bool isEmptyPage)
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
            if (CheckIfPageIsEmpty(rows, currentRowNo + 1, Mapper.Fields.First().ColumnIndex))
            {
                isEmptyPage = true;
                return;
            }

            // When we start to load data for each player, we must take row number of headers and then increase it for 1
            currentRowNo = Mapper.Fields.First().RowIndex;
            IEnumerable<FieldItem> otherFields = Mapper.Fields.FindAll(i => i.GlobalField == false);

            int playerCount = CalculatePlayerCount(rows, currentRowNo, otherFields.First().ColumnIndex, _maxPlayerPerMatch);

            for (int i = 1; i < playerCount; i++)
            {
                currentRowNo++;
                PlayerInfo pi = new PlayerInfo();
                PopulateModelField(pi, rows, otherFields, currentRowNo);
                /*contains list of all players score*/
                AddPlayerInfo(teamName, pi);
            }
            Loger.log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);
            stopwatch.Stop();

        }

        private void AddPlayerInfo(string teamName, PlayerInfo pi)
        {
            List<PlayerInfo> list;
            Dictionary<string, List<PlayerInfo>> infoDict;

            if (!pi.NameAndLastName.Equals(String.Empty))
            {
                if (TeamPlayerInfos.TryGetValue(teamName, out infoDict))
                {
                    if (infoDict.TryGetValue(pi.NameAndLastName, out list))
                    {
                        list.Add(pi);
                    }
                    else
                    {
                        TeamPlayerInfos[teamName].Add(pi.NameAndLastName, new List<PlayerInfo>() { pi });
                    }

                }
                else
                {
                    TeamPlayerInfos.Add(teamName, new Dictionary<string, List<PlayerInfo>>() { { pi.NameAndLastName, new List<PlayerInfo>() } });
                }
            }


        }


        #endregion

    }
}
