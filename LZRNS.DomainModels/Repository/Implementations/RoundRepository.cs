using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class RoundRepository : RepositoryBase<Round>, IRoundRepository
    {
        public RoundRepository(BasketballDbContext context) : base(context)
        {

            
        }
        /*
        public ICollection<Round> FindRoundByNameAndSeason(string roundName, Guid leagueSeasonId)
        {
            return _context.Rounds.Where(r => r.RoundName.Equals(roundName) && r.LeagueSeason.Id == leagueSeasonId).ToList();
        }*/
    }
}
