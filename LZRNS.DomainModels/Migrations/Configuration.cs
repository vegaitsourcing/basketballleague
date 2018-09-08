namespace LZRNS.DomainModels.Migrations
{
    using LZRNS.DomainModel.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LZRNS.DomainModel.Context.BasketballDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LZRNS.DomainModel.Context.BasketballDbContext context)
        {
            SeedHelper.Databuild(context);
        }
    }


    internal static class SeedHelper
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static Player GeneratePlayer()
        {
            return new Player
            {
                Name = RandomString(7),
                LastName = RandomString(7),
                Height = random.Next(180, 220),
                Weight = random.Next(80, 120),
                YearOfBirth = random.Next(1980, 2000)
            };
        }

        public static Team GenerateTeam()
        {
            return new Team
            {
                TeamName = RandomString(7),
                Coach = RandomString(7),
                Players = new List<Player>(),
                Games = new List<Game>(),
                Id = Guid.NewGuid()
            };
        }

        public static void Databuild(LZRNS.DomainModel.Context.BasketballDbContext context)
        {
            
            var season1718 = new Season { Name = "2017-2018", Games = new List<Game>(), Teams = new List<Team>() };

            var player1 = new Player { Name = "Player1Name", LastName = "Player1LastName", Height = 189, Weight = 95, YearOfBirth = 1990 };
            var player2 = new Player { Name = "Player2Name", LastName = "Player2LastName", Height = 200, Weight = 96, YearOfBirth = 1991 };
            var player3 = new Player { Name = "Player3Name", MiddleName = "Mn", LastName = "Player3LastName", Height = 186, Weight = 88, YearOfBirth = 1989 };
            var team1 = SeedHelper.GenerateTeam();
            team1.Players.ToList().AddRange(new List<Player> { player1, player2, player3 });
            team1.LeagueSeason.Season = season1718;
            var t2p1 = SeedHelper.GeneratePlayer();
            var t2p2 = SeedHelper.GeneratePlayer();
            var t2p3 = SeedHelper.GeneratePlayer();
            var t2p4 = SeedHelper.GeneratePlayer();
            var t2 = SeedHelper.GenerateTeam();
            t2.Players.ToList().AddRange(new List<Player> { t2p1, t2p2, t2p3, t2p4 });
            t2.LeagueSeason.Season = season1718;
            var t3p1 = SeedHelper.GeneratePlayer();
            var t3p2 = SeedHelper.GeneratePlayer();
            var t3p3 = SeedHelper.GeneratePlayer();
            var t3 = SeedHelper.GenerateTeam();
            t3.Players.ToList().AddRange(new List<Player> { t3p1, t3p2, t3p3 });
            t3.LeagueSeason.Season = season1718;

            season1718.Teams.ToList().AddRange(new List<Team> { team1, t2, t3 });

            context.Seasons.Add(season1718);
            context.Teams.AddRange(new List<Team> { team1, t2, t3 });
            context.Players.AddRange(new List<Player> { player1, player2, player3, t2p1, t2p2, t2p3, t2p4, t3p1, t3p2, t3p3 });



        }
    }
}
