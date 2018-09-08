using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace LZRNS.DomainModel.Context
{
    public class BasketballDbContext : DbContext
    {
        public BasketballDbContext(string connString) : base(connString)
        {
            Database.SetInitializer<BasketballDbContext>(new DbInitializer());
            Database.Initialize(true);
        }

        public BasketballDbContext()
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasRequired(x => x.TeamA).WithMany()
                 .HasForeignKey(c => c.TeamAId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>().HasRequired(x => x.TeamB).WithMany()
                .HasForeignKey(c => c.TeamBId).WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
