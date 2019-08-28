using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface ILeagueRepository : IRepositoryBase<League>
    {
        void GenerateSchedule(LeagueSeason leagueSeason);
        League CreateLeague(string leagueName);
        IQueryable<League> GetLeaguesByName(string leagueName);
    }
}
