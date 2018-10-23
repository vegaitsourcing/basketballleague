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
        private string seasonRegex = @"(\d{4})\/(\d{2})";
        private string dateRegex = @"(\d{2}).(\d{2}).(\d{4})";
        string season = String.Empty;
        string date = String.Empty;
        string liga = String.Empty;
        List<SingleGameModel> games = new List<SingleGameModel>();
        private bool nextGame;

        public void Convert(MemoryStream memoryStream)
        {

            if (memoryStream == null)
            {
                throw new Exception("input stream is null");
            }

            var lines = ConvertToLines(memoryStream);

            SingleGameModel model = new SingleGameModel();
            foreach (var line in lines)
            {   
                ProcesLine(line,ref model);
                if(nextGame == true)
                {
                    games.Add(model);
                    model = new SingleGameModel();
                    nextGame = false;
                }
            }
        }

        private bool ProcesLine(string line,ref SingleGameModel model)
        {
            if(line == string.Empty || line.Trim() == string.Empty)
            {
                return true;
            }

            //ignore lines
            if(line.Contains("#") || line.Contains("(") || line.Contains(")"))
            {
                return true;
            }

            //get league
            if (line.Contains("LIGA"))
            {
                this.liga = line.Trim().Split(' ')[0];
                return true;
            }

            //check if its season line!
            Regex pattern = new Regex(seasonRegex);
            Match match = pattern.Match(line);
            if(match.Success)
            {
                this.season = match.Value;
                return true;
            }

            //check date of next few matches
            Regex datePattern = new Regex(dateRegex);
            Match dateMatch = datePattern.Match(line);
            if (dateMatch.Success)
            {
                this.nextGame = !this.date.Equals(dateMatch.Value);
                this.date = dateMatch.Value;
                return true;
            }

            var trimmedLine = line.Trim();

            var firstSpaceInd = trimmedLine.IndexOf(" ");

            var time = trimmedLine.Substring(0, firstSpaceInd);
            var games = trimmedLine.Substring(firstSpaceInd);
            var teams = games.Split(new string[] { " vs " }, StringSplitOptions.None);

            var t1 = teams[0].Trim();
            var t2 = teams[1].Trim();

            model.Date = date;
            model.Season = season;
            model.TeamA = t1;
            model.TeamB = t2;
            model.Time = time;
            model.Liga = liga;

            return true;


        }

        public void SaveToDb()
        {
            if(this.games.Count == 0 || this.liga == String.Empty)
            {
                throw new Exception("empty games collection");
            }

            using(var dbContext = new BasketballDbContext())
            {
                //first remove all games for given league
                dbContext.Schedules.RemoveRange(dbContext.Schedules.Where(x => x.Liga.Equals(this.liga)));
                dbContext.SaveChanges();
                dbContext.Schedules.AddRange(this.games);
                dbContext.SaveChanges();
            }
        }

        private List<string> ConvertToLines(MemoryStream memoryStream)
        {

            List<string> rows = new List<string>();

            using (var reader = new StreamReader(memoryStream, Encoding.ASCII))
            {
                string line= null;
                while ((line = reader.ReadLine()) != null){
                    rows.Add(line);
                }
            }

            if(rows.Count == 0)
            {
                throw new Exception("parsing failed, result is empty");
            }

            return rows;
        }
        
        public List<SingleGameModel> AllGamesList { get { return games; } }
        public string Season { get { return season; } }
    }
}
