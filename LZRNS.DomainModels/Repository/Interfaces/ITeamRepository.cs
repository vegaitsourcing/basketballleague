﻿using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;

namespace LZRNS.DomainModels.Repository.Interfaces
{
	public interface ITeamRepository : IRepositoryBase<Team>
	{
		PlayerPerTeam AddPlayerToTeam(Guid playerId, Guid teamId);
		void DeletePlayerFromTeam(Guid teamMemberId);
		Team FindTeam(string teamName, string seasonName);
        IEnumerable<Team> GetTeamsByLeagueSeasonId(Guid leagueSeasonId);
    }
}
