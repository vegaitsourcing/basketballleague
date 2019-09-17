using LZRNS.DomainModel.Models;
using System.Collections.Generic;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface IPlayerRepository : IRepositoryBase<Player>
    {
        Player GetPlayerByName(string firstName, string lastName, string middleName = "");

        IEnumerable<Player> FilterPlayers(string q, string fl, bool activeOnly);

        IEnumerable<Player> GetAll(bool active);
    }
}