using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface ILeagueRepository : IRepositoryBase<League>
    {
        void GenerateSchedule(LeagueSeason leagueSeason);
    }
}
