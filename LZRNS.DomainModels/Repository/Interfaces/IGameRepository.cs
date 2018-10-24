using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;

namespace LZRNS.DomainModels.Repository.Interfaces
{
	public interface IGameRepository : IRepositoryBase<Game>
	{
		Stats AddStatsForPlayerInGame(Stats stats);
		void UpdateStatsForPlayerInGame(Stats stats);
		void DeleteStatsForPlayerInGame(Guid gamePlayerId);
	}
}
