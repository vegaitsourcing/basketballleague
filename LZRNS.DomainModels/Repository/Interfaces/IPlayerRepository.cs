using LZRNS.DomainModel.Models;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface IPlayerRepository : IRepositoryBase<Player>
    {

        Player GetPlayerByName(string firstName, string lastName, string middleName = "");
    }
}
