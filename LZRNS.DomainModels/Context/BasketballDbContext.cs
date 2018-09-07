using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace LZRNS.DomainModel.Context
{
    public class BasketballDbContext : DbContext
    {
        public BasketballDbContext()
        {
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}
