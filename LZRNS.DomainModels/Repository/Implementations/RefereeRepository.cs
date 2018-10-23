using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.DomainModel.Context;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class RefereeRepository : RepositoryBase<Referee>, IRefereeRepository
    {
        public RefereeRepository(BasketballDbContext context) : base(context)
        {
        }
    }
}
