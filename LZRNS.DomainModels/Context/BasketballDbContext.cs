using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Exceptions;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.TimetableModels;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;

namespace LZRNS.DomainModel.Context
{
    public class BasketballDbContext : DbContext
    {
        public BasketballDbContext() : base("domainDb")
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<PlayerPerTeam> PlayersPerTeam { get; set; }
        public DbSet<LeagueSeason> LeagueSeasons { get; set; }
        public DbSet<Stats> Stats { get; set; }
        public DbSet<SingleGameModel> Schedules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var newException = new FormattedDbEntityValidationException(e);
                throw newException;
            }
        }
    }
}