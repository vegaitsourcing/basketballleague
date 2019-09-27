namespace LZRNS.DomainModels.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<LZRNS.DomainModel.Context.BasketballDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LZRNS.DomainModel.Context.BasketballDbContext";
        }

        protected override void Seed(LZRNS.DomainModel.Context.BasketballDbContext context)
        {
        }
    }
}