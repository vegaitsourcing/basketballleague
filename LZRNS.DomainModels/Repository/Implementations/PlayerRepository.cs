using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LZRNS.DomainModel.Context;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(BasketballDbContext context) : base(context)
        {
        }

        public Player GetPlayerByName(string firstName, string lastName, string middleName = "")
        {
            Player player = GetAll().Where(p => p.Name.Equals(firstName) && p.LastName.Equals(lastName) && p.MiddleName.Equals(middleName)).FirstOrDefault();

            return player;
        }
    }
}
