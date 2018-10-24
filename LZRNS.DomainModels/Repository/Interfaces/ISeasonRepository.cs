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
		IEnumerable<LeagueSeason> GetAllLeagueSeasons();
		bool UpdateLeagueSeason(LeagueSeason leagueSeason);
		void DeleteLeagueSeason(LeagueSeason leagueSeason);
	}
}
