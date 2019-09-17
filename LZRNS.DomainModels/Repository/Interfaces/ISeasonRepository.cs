using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface ISeasonRepository : IRepositoryBase<Season>
    {
        Season GetSeasonByYear(int seasonStartYear);

        LeagueSeason AddLeagueToSeason(LeagueSeason leagueSeason);

        LeagueSeason GetLeagueSeasonById(Guid id);

        Season GetSeasonByName(string seasonName);

        IEnumerable<LeagueSeason> GetAllLeagueSeasons();

        LeagueSeason GetLeagueSeasonsBySeasonAndLeague(Guid seasonId, ICollection<Guid> leaguesIds);

        bool UpdateLeagueSeason(LeagueSeason leagueSeason);

        void DeleteLeagueSeason(LeagueSeason leagueSeason);
    }
}