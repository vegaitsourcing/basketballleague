using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Helper
{
    public class TeamHistoryItem
    {
        public Season Season { get; set; }

        public int Place
        {
            get
            {
                int place = 0;

                ICollection<Team> orderedTeams = League.LeagueSeasons.FirstOrDefault(ls => ls.SeasonId == Season.Id).
                    Teams.OrderByDescending(t => t.Points).ToList();

                foreach (Team team in orderedTeams)
                {
                    place++;
                    if (team.Id == Team.Id)
                    {
                        break;
                    }
                }
                return place;
            }
        }

        public Team Team { get; set; }

        public League League
        {
            get
            {
                return Team.LeagueSeason.League;
            }
        }
    }
}
