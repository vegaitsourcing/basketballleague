using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Migrations;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.TimetableModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;

namespace LZRNS.DomainModel.Context
{
    public class BasketballDbContext : DbContext
    {
        /*public BasketballDbContext() : base("domainDb")
        {
            //Database.SetInitializer<BasketballDbContext>(new DbInitializer());
            //Database.Initialize(true);
        }*/

        public BasketballDbContext():base("second")
        {
            Database.SetInitializer<BasketballDbContext>(new DbInitializer());
            Database.Initialize(true);
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

            //modelBuilder.Entity<Game>().HasRequired(x => x.TeamA).WithMany()
            //     .HasForeignKey(c => c.TeamAId).WillCascadeOnDelete(false);

            //modelBuilder.Entity<Game>().HasRequired(x => x.TeamB).WithMany()
            //    .HasForeignKey(c => c.TeamBId).WillCascadeOnDelete(false);

            //modelBuilder.Entity<LeagueSeason>().HasRequired(x => x.Season).WithMany()
            //    .HasForeignKey(c => c.).WillCascadeOnDelete(false);
            //base.OnModelCreating(modelBuilder);
        }
    }
}
