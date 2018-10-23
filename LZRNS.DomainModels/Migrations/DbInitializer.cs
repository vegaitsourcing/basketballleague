using LZRNS.DomainModel.Context;
using System.Data.Entity;

namespace LZRNS.DomainModels.Migrations
{
    public class DbInitializer: DropCreateDatabaseIfModelChanges<BasketballDbContext>
    {
        protected override void Seed(BasketballDbContext context)
        {
            SeedHelper.Databuild(context);
            base.Seed(context);
        }
    }
}
