using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface ILeagueRepository : IRepositoryBase<League>
    {
        void GenerateSchedule(LeagueSeason leagueSeason);
    }
}
