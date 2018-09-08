using LZRNS.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
