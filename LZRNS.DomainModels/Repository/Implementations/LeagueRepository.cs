using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModel.Context;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class LeagueRepository : RepositoryBase<League>, ILeagueRepository
    {
        public LeagueRepository(BasketballDbContext context) : base(context)
        {
        }

        public void GenerateSchedule(LeagueSeason leagueSeason)
        {
            throw new NotImplementedException();
        }
    }
}
