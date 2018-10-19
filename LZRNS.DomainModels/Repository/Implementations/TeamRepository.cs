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
	public class TeamRepository : RepositoryBase<Team>, ITeamRepository
	{
		public TeamRepository(BasketballDbContext context) : base(context)
		{
		}

		public PlayerPerTeam AddPlayerToTeam(Guid playerId, Guid teamId)
		{

			var playerInTeam = new PlayerPerTeam();
			playerInTeam.Id = Guid.NewGuid();
			playerInTeam.Player = _context.Players.Find(playerId);
			playerInTeam.Team = _context.Teams.Find(teamId);

			var entity = _context.PlayersPerTeam.Add(playerInTeam);

			_context.SaveChanges();
			return entity;
		}

		public void DeletePlayerFromTeam(Guid teamMemberId)
		{

			var playerInTeam = _context.PlayersPerTeam.Find(teamMemberId);

			_context.PlayersPerTeam.Remove(playerInTeam);

			_context.SaveChanges();
		}

		public Team FindTeam(string teamName, string seasonName)
		{
			Team team = GetAll().Where(t => t.TeamName.Equals(teamName) && t.LeagueSeason.Season.Name.Equals(seasonName)).FirstOrDefault();

			return team;
		}
	}
}
