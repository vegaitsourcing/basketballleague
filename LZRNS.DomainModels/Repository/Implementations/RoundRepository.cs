using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class RoundRepository : RepositoryBase<Round>, IRoundRepository
    {
        public RoundRepository(BasketballDbContext context) : base(context)
        {
        }
    }
}
