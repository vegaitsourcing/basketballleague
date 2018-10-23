using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System.Linq;
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
