﻿using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader
{
    public class ExcelLoader
    {
        #region Private Fields
        private XLWorkbook exApp;
        private IXLWorksheets sheets;

        // key represent team name (I will change that later)
        private Dictionary<string, TeamStatistic> teams;

        private MapperModel mapper;
        private int maxPlayerPerMatch = 12;

        #endregion Private Fields

        #region Constructors
        public ExcelLoader() {
            Loger.log.Debug("Start main proces");
            this.teams = new Dictionary<string, TeamStatistic>();

            string hardCodedPath = @"F:\1.Code\6.Hackaton\1.Code\basketballleague\LZRNS.ExcelLoader\TableMapper.config";
            this.mapper = new MapperModel(hardCodedPath);

        }
        #endregion Constructors

        #region Public Methods
        public void ProcessFile(String path, string fileName)
        {
            try
            {
                using (exApp = new XLWorkbook(path))
                {
                    Load(exApp, fileName);
                }
            }
            finally
            {
                sheets = null;
                exApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void ProcessFile (MemoryStream memoryStream, string fileName)
        {
            try
            {
                using (exApp = new XLWorkbook(memoryStream))
                {
                    Load(exApp, fileName);
                }
            }
            finally
            {
                sheets = null;
                exApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void Load(XLWorkbook exApp, string fileName)
        {
            int currentSheetNo = 0;
            string nameWithExtension = fileName.Split(new string[] { "stats -teams-" }, StringSplitOptions.None).Last();
            string teamName = nameWithExtension.Substring(0, nameWithExtension.Length - 5);

            sheets = exApp.Worksheets;

            //Only for first sheet we want to check validation for mapping fields configuration
            foreach (IXLWorksheet sheet in sheets)
            {
                CheckMappingValidation(mapper, sheet);
                break;
            }

            TeamStatistic teamStatistic = new TeamStatistic(teamName);

            foreach (IXLWorksheet sheet in sheets)
            {
                // for odd sheet we want to skip loading
                if (currentSheetNo % 2 == 0)
                {
                    ProcessSheet(sheet, ref teamStatistic);
                }
                currentSheetNo++;
            }

            teams[teamStatistic.TeamName] = teamStatistic;
        }

        
        #endregion Public Methods

        #region Private Methods

        private void CheckMappingValidation (MapperModel mapper, IXLWorksheet sheet)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IXLRows rows = sheet.RowsUsed();
            int currentRowIndex = -1;
            IXLRow row = null;

            foreach (FieldItem item in mapper.Fields)
            {
                if (item.CellName.Equals("")) continue;

                if (currentRowIndex != item.RowIndex )
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
            stopwatch.Stop();
            Loger.log.Debug("CheckMappingValidation Successfully completed for time: " + stopwatch.Elapsed);

        }

        private void ProcessSheet(IXLWorksheet sheet, ref TeamStatistic teamStatistic)
        {
            Loger.log.Debug("ProcessSheet started for table: " + sheet.Name);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            IXLRows rows = sheet.RowsUsed();

            TeamScore teamScore = new TeamScore();
            teamScore.RoundName = sheet.Name;

            // When we start to load data for each player, we must take row number of headers and then increase it for 1
            int currentRowNo = mapper.Fields.First().RowIndex;
            IEnumerable<FieldItem> otherFields = mapper.Fields.FindAll(i => i.GlobalField == false);
            int playerCount = CalculatePlayerCount(rows, currentRowNo, otherFields.First().ColumnIndex, maxPlayerPerMatch);

            for (int i = 0; i < playerCount; i++)
            {
                currentRowNo++;
                PlayerScore pl = new PlayerScore();
                PopulateModelField(pl, rows, otherFields, currentRowNo);
                teamScore.AddPlayerScore(pl);
            }


            IEnumerable<FieldItem> globalFields = mapper.Fields.FindAll(i => i.GlobalField == true);
            currentRowNo = mapper.Fields.First().RowIndex;
            PopulateModelField(teamScore, rows, globalFields, ++currentRowNo);
            
            Loger.log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);

            stopwatch.Stop();
            teamStatistic.AddTeamScore(teamScore);

        }

        private void PopulateModelField(Object modelObj, IXLRows exlRange, IEnumerable<FieldItem> fields, int rowIndex = -1)
        {
            foreach(FieldItem fieldItem in fields)
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

        public Object GetCellValue(IXLRows rows, int rowIndex, int columnIndex)
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
        public int CalculatePlayerCount(IXLRows exlRange, int currentRowNo, int columnIndex, int maxPlayerCount)
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
