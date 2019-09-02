using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using LZRNS.DomainModel.Context;
using LZRNS.ExcelLoader.Model;
using System.Reflection;
//using LZRNS.ExcelLoader.ExcelReader;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class ExcelAnalyzer : AbstractExcelLoader
    {

        #region Private Fields

        private const int _maxPlayerPerMatch = 12;

        private XLWorkbook _exApp;
        private IXLWorksheets _sheets;
        //private BasketballDbContext _db;

        // key represent player full name
        //private Dictionary<string, List<PlayerInfo>> _playerInfos;

        private Dictionary<string, Dictionary<string, List<PlayerInfo>>> _teamPlayerInfos;
        /*
        // key represent team name, list of player names
        private Dictionary<string, HashSet<string>> _teamAndPlayers;

        private string _seasonName;
        private string _leagueName;

        private MapperModel _mapper;

        #endregion Private Fields

        #region Properies 
       

        public Dictionary<string, HashSet<string>> TeamAndPlayers
        {
            get { return _teamAndPlayers; }
        }
        */

        //public string SeasonName { get => _seasonName; set => _seasonName = value; }
        //public string LeagueName { get => _leagueName; set => _leagueName = value; }

        //private MapperModel Mapper { get => _mapper; set => _mapper = value; }
        //private XLWorkbook ExApp { get => _exApp; set => _exApp = value; }
        //private IXLWorksheets Sheets { get => _sheets; set => _sheets = value; }
        //key is player name, value list of his infos
        /*
        public Dictionary<string, List<PlayerInfo>> PlayerInfos
        {
            get { return _playerInfos; }

        }
        */
        public Dictionary<string, Dictionary<string, List<PlayerInfo>>> TeamPlayerInfos
        {
            get { return _teamPlayerInfos; }
            set { _teamPlayerInfos = value; }
        }
        #endregion Properies



        #region Constructors
        public ExcelAnalyzer(string configPath) : base(configPath)
        {
            Loger.log.Debug("Start main proces");
            //_db = db;
            _teamPlayerInfos = new Dictionary<string, Dictionary<string, List<PlayerInfo>>>();
            // _teamAndPlayers = new Dictionary<string, HashSet<string>>();


            //Mapper = new MapperModel(configPath);

        }
        #endregion Constructors
        /*
        #region Public Methods
        public void ProcessFile(String path, string fileName)
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
        */



        #region Private Methods
        private void ProcessSheet(IXLWorksheet sheet, string teamName, out bool isEmptyPage)
        {
            //Loger.log.Debug("ProcessSheet started for table: " + sheet.Name);

            // Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            isEmptyPage = false;

            IXLRows rows = sheet.RowsUsed();

            //TeamScore teamScore = new TeamScore();
            //teamScore.RoundName = sheet.Name;
            int currentRowNo;

            IEnumerable<FieldItem> globalFields = Mapper.Fields.FindAll(i => i.GlobalField == true);
            currentRowNo = Mapper.Fields.First().RowIndex;

            // we are incrasing rowNo because currentRow reprenset row where is header placed!
            if (CheckIfPageIsEmty(rows, currentRowNo + 1, Mapper.Fields.First().ColumnIndex))
            {
                //Loger.log.Debug("ProcessSheet: Sheet: " + sheet.Name + ", is empty for Team: " + teamStatistic.TeamName);
                isEmptyPage = true;
                return;
            }

            // we are incrasing rowNo because currentRow reprenset row where is header placed!
            //PopulateModelField(teamScore, rows, globalFields, currentRowNo + 1);

            // When we start to load data for each player, we must take row number of headers and then increase it for 1
            currentRowNo = Mapper.Fields.First().RowIndex;
            IEnumerable<FieldItem> otherFields = Mapper.Fields.FindAll(i => i.GlobalField == false);

            int playerCount = CalculatePlayerCount(rows, currentRowNo, otherFields.First().ColumnIndex, _maxPlayerPerMatch);

            for (int i = 1; i < playerCount; i++)
            {
                currentRowNo++;
                PlayerInfo pi = new PlayerInfo();
                //tested
                PopulateModelField(pi, rows, otherFields, currentRowNo);
                /*contains list of all players score*/

                //teamScore.AddPlayerScore(pl);

                AddPlayerInfo(teamName, pi);
                //AddPlayerInTeam(teamName, pi.NameAndLastName);
            }

            //Loger.log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);

            //stopwatch.Stop();
            //teamStatistic.AddTeamScore(teamScore);



        }

        private void AddPlayerInfo(string teamName, PlayerInfo pi)
        {
            List<PlayerInfo> list;
            Dictionary<string, List<PlayerInfo>> infoDict;

            if (!pi.NameAndLastName.Equals(String.Empty))
            {
                if (TeamPlayerInfos.TryGetValue(teamName, out infoDict))
                {
                    //pi.NameAndLastName
                    if (infoDict.TryGetValue(pi.NameAndLastName, out list))
                    {
                        list.Add(pi);
                    }
                    else
                    {
                        TeamPlayerInfos[teamName].Add(pi.NameAndLastName,new List<PlayerInfo>() { pi });
                    }

                }
                else
                {
                    TeamPlayerInfos.Add(teamName,new Dictionary<string, List<PlayerInfo>>(){ { pi.NameAndLastName, new List<PlayerInfo>() } });
                }
            }


        }


        public override void Load(XLWorkbook exApp, string fileName)
        {
            //fileName should have the following format - teamName-seasonName-leagueName.xlsx (at least one file)
            string teamName = CheckFileStructure(exApp, fileName);
            int currentSheetNo = 0;

            //TeamStatistic teamStatistic = new TeamStatistic(teamName);
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

    }
}
