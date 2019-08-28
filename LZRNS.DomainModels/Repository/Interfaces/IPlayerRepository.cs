using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface IPlayerRepository : IRepositoryBase<Player>
    {
        //this is bad approach - multiple players can have the same firtsName, lastName nad middleName
        Player GetPlayerByName(string firstName, string lastName, string middleName = "");
        //Player GetlayerByTeamIdAndName(Guid teamId, string firstName, string lastName, string middleName = "")
        IEnumerable<Player> FilterPlayers(string q, string fl, bool activeOnly);
        IEnumerable<Player> GetAll(bool active);
    }
}
