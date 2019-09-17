using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.TimetableModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TimeTableLoader.Converter
{
    public class Converter
    {
        private const string DateRegex = @"(\d{2}).(\d{2}).(\d{4})";
        private const string SeasonRegex = @"(\d{4})\/(\d{2})";
        private string _date = string.Empty;
        private string _liga = string.Empty;
        private bool _nextGame;

        public List<SingleGameModel> AllGamesList { get; } = new List<SingleGameModel>();

        public string Season { get; private set; } = string.Empty;

        public void Convert(MemoryStream memoryStream)
        {
            if (memoryStream == null)
            {
                throw new Exception("input stream is null");
            }

            var lines = ConvertToLines(memoryStream);

            var model = new SingleGameModel();
            foreach (string line in lines)
            {
                ProcessLine(line, ref model);
                if (_nextGame)
                {
                    AllGamesList.Add(model);
                    model = new SingleGameModel();
                    _nextGame = false;
                }
            }
        }

        public void SaveToDb()
        {
            if (AllGamesList.Count == 0 || _liga == string.Empty)
            {
                throw new Exception("empty games collection");
            }

            using (var dbContext = new BasketballDbContext())
            {
                //first remove all games for given league
                dbContext.Schedules.RemoveRange(dbContext.Schedules.Where(x => x.Liga.Equals(_liga)));
                dbContext.SaveChanges();
                dbContext.Schedules.AddRange(AllGamesList);
                dbContext.SaveChanges();
            }
        }

        private static IEnumerable<string> ConvertToLines(Stream memoryStream)
        {
            var rows = new List<string>();

            using (var reader = new StreamReader(memoryStream, Encoding.ASCII))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    rows.Add(line);
                }
            }

            if (rows.Count == 0)
            {
                throw new Exception("parsing failed, result is empty");
            }

            return rows;
        }

        private void ProcessLine(string line, ref SingleGameModel model)
        {
            if (line is null)
                return;

            if (line.Length == 0 || line.Trim().Length == 0)
            {
                return;
            }

            //ignore lines
            if (line.Contains("#") || line.Contains("(") || line.Contains(")"))
            {
                return;
            }

            //get league
            if (line.Contains("LIGA"))
            {
                _liga = line.Trim().Split(' ')[0];
                return;
            }

            //check if its season line!
            var pattern = new Regex(SeasonRegex);
            var match = pattern.Match(line);
            if (match.Success)
            {
                Season = match.Value;
                return;
            }

            //check date of next few matches
            var datePattern = new Regex(DateRegex);
            var dateMatch = datePattern.Match(line);
            if (dateMatch.Success)
            {
                _nextGame = !_date.Equals(dateMatch.Value);
                _date = dateMatch.Value;
                return;
            }

            string trimmedLine = line.Trim();

            int firstSpaceInd = trimmedLine.IndexOf(" ", StringComparison.Ordinal);

            string time = trimmedLine.Substring(0, firstSpaceInd);
            string games = trimmedLine.Substring(firstSpaceInd);
            var teams = games.Split(new[] { " vs " }, StringSplitOptions.None);

            string t1 = teams[0].Trim();
            string t2 = teams[1].Trim();

            model.Date = _date;
            model.Season = Season;
            model.TeamA = t1;
            model.TeamB = t2;
            model.Time = time;
            model.Liga = _liga;
        }
    }
}