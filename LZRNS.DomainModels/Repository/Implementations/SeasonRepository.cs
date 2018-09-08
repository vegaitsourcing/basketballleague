using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Models;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class SeasonRepository : RepositoryBase<Season>, ISeasonRepository
    {
        public SeasonRepository(BasketballDbContext context) : base(context)
        {
        }

        public LeagueSeason AddLeagueToSeason(League league, Season season)
        {
            var leagueSeason = new LeagueSeason() { League = league, Season = season };
            var entity = _context.LeagueSeasons.Add(leagueSeason);
            _context.SaveChanges();
            return entity;
        }
    }
}
