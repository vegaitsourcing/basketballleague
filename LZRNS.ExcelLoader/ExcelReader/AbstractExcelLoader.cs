using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;



namespace LZRNS.ExcelLoader.ExcelReader
{
    public abstract class AbstractExcelLoader
    {
        #region Private Fields
        private XLWorkbook _exApp;
        private IXLWorksheets _sheets;

        // key represent team name (I will change that later)
        //private Dictionary<string, TeamStatistic> teams;

        // key represent team name, list of player names
        private Dictionary<string, HashSet<string>> _teamAndPlayers;

        private string _seasonName;
        private string _leagueName;

        private MapperModel _mapper;

        private int _maxPlayerPerMatch = 12;

        #endregion Private Fields

        #region Properies 
        public Dictionary<string, HashSet<string>> TeamAndPlayers
        {
            get { return _teamAndPlayers; }
        }

        protected int MaxPlayerPerMatch
        { get => _maxPlayerPerMatch; }

        public string SeasonName { get => _seasonName; set => _seasonName = value; }
        public string LeagueName { get => _leagueName; set => _leagueName = value; }


        protected MapperModel Mapper { get => _mapper; set => _mapper = value; }
        protected XLWorkbook ExApp { get => _exApp; set => _exApp = value; }
        protected IXLWorksheets Sheets { get => _sheets; set => _sheets = value; }
        #endregion Properies

        #region Constructors
        public AbstractExcelLoader(string configPath)
        {
            Loger.log.Debug("Start main proces");
            //_teams = new Dictionary<string, TeamStatistic>();
            //this.playerStores = new Dictionary<string, List<PlayerScore>>();
            _teamAndPlayers = new Dictionary<string, HashSet<string>>();
            Mapper = new MapperModel(configPath);

        }
        #endregion
        #region Protected methods
        protected string CheckFileStructure(XLWorkbook exApp, string fileName)
        {
            //fileName should have the following format - teamName-seasonName-leagueName.xlsx (at least one file)
            int currentSheetNo = 0;

            //string nameWithExtension = fileName.Split(new string[] { "stats-teams-" }, StringSplitOptions.None).Last();

            string[] nameParts = fileName.Split('-');

            string teamName = nameParts[2];
            if (teamName.Contains(".xlsx"))
            {
                teamName = teamName.Substring(0, teamName.Length - 5);
            }
            //only one file will determine SeasonName and LeagueName - first with appropriate filename structure
            if (nameParts.Length == 5 && SeasonName == null && LeagueName == null)
            {
                SeasonName = nameParts[3];
                LeagueName = nameParts[4].Substring(0, nameParts[4].Length - 5);
            }

            Loger.log.Debug("Loading data for team: " + teamName);

            Sheets = exApp.Worksheets;

            //Only for first sheet we want to check validation for mapping fields configuration
            foreach (IXLWorksheet sheet in Sheets)
            {
                //check file format/mapping
                CheckMappingValidation(Mapper, sheet);
                break;
            }
            return teamName;
        }

        //tested
        protected void CheckMappingValidation(MapperModel mapper, IXLWorksheet sheet)
        {


            IXLRows rows = sheet.RowsUsed();
            int currentRowIndex = -1;
            IXLRow row = null;

            foreach (FieldItem item in mapper.Fields)
            {
                if (item.CellName.Equals("")) continue;

                if (currentRowIndex != item.RowIndex)
                {
                    row = rows.ElementAt(item.RowIndex);
                    currentRowIndex = item.RowIndex;
                }

                object value = row.Cell(item.ColumnIndex).Value;

                if (value == null || !value.Equals(item.CellName))
                {

                    Loger.log.Error("Mapping is invalid for sheet: " + sheet.Name);
                    throw new Exception();
                }
            }

        }

        protected void PopulateModelField(Object modelObj, IXLRows exlRange, IEnumerable<FieldItem> fields, int rowIndex = -1)
        {
            foreach (FieldItem fieldItem in fields)
            {
                PopulateModelValue(modelObj, exlRange, fieldItem, rowIndex);
            }
        }

        private void PopulateModelValue(Object obj, IXLRows exlRange, FieldItem fieldItem, int rowIndex = -1)
        {
            var objProperty = obj.GetType().GetProperty(fieldItem.PropertyName, BindingFlags.Public | BindingFlags.Instance);
            //get the value of the property
            if (objProperty == null)
            {
                Loger.log.Error("PopulateModelValue: PropertyName: " + fieldItem.PropertyName + " not exist in model: " + obj);
                // We should log error (currently I will just throw exception to be aware that model and mapping are not matched)
                //throw new Exception();
                return;
            }

            // If we do not explcitly send row index or data should be directly read from cell, then it should be used directly from configuration
            // Examlple: matchDate doesn not contain header
            if (fieldItem.DirectCellData == true || rowIndex == -1)
            {
                rowIndex = fieldItem.RowIndex;
            }

            Object value = GetCellValue(exlRange, rowIndex, fieldItem.ColumnIndex);
            if (value == null)
            {
                Loger.log.Error("PopulateModelValue - PropertyName: " + fieldItem.PropertyName + " in NULL");
                return;
            }

            objProperty.SetValue(obj, fieldItem.GetValueConverted(value));

        }

        protected Object GetCellValue(IXLRows rows, int rowIndex, int columnIndex)
        {
            if (rows.Count() <= rowIndex)
            {
                return null;
            }
            IXLRow row = rows.ElementAt(rowIndex);
            Object obj = row.Cell(columnIndex).Value;

            return obj;
        }

        /* This method used to calculate number of rows that are populated for players statistic */
        protected int CalculatePlayerCount(IXLRows exlRange, int currentRowNo, int columnIndex, int maxPlayerCount)
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

        protected bool CheckIfPageIsEmty(IXLRows rows, int rowIndex, int colunmIndex)
        {
            bool isEmpty = false;

            if (GetCellValue(rows, rowIndex, colunmIndex) == null)
            {
                isEmpty = true;
            }

            return isEmpty;
        }

        protected void AddPlayerInTeam(string team, string playerName)
        {
            HashSet<string> players;

            if (_teamAndPlayers.TryGetValue(team, out players))
            {
                players.Add(playerName);
            }
            else
            {
                _teamAndPlayers[team] = new HashSet<string>() { playerName };
            }

        }

        #endregion
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

        public abstract void Load(XLWorkbook exApp, string fileName);
       
        #endregion

    }
}
