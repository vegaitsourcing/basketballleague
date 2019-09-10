namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SeasonId = c.Guid(nullable: false),
                        RoundId = c.Guid(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        TeamAId = c.Guid(nullable: false),
                        TeamBId = c.Guid(nullable: false),
                        Team_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Round", t => t.RoundId)
                .ForeignKey("dbo.Team", t => t.Team_Id)
                .ForeignKey("dbo.Season", t => t.SeasonId)
                .ForeignKey("dbo.Team", t => t.TeamAId)
                .ForeignKey("dbo.Team", t => t.TeamBId)
                .Index(t => t.SeasonId)
                .Index(t => t.RoundId)
                .Index(t => t.TeamAId)
                .Index(t => t.TeamBId)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Referee",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Game", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Round",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoundName = c.String(),
                        LeagueSeasonId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId)
                .Index(t => t.LeagueSeasonId);
            
            CreateTable(
                "dbo.LeagueSeason",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LeagueId = c.Guid(nullable: false),
                        SeasonId = c.Guid(nullable: false),
                        Summary = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.League", t => t.LeagueId)
                .ForeignKey("dbo.Season", t => t.SeasonId)
                .Index(t => t.LeagueId)
                .Index(t => t.SeasonId);
            
            CreateTable(
                "dbo.League",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Season",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 120),
                        SeasonStartYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TeamName = c.String(nullable: false, maxLength: 100),
                        Image = c.String(),
                        PreviousTeamGuid = c.Guid(),
                        Coach = c.String(),
                        LeagueSeasonId = c.Guid(nullable: false),
                        TeamScoreId = c.Guid(nullable: false),
                        PreviousTeamRef_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId)
                .ForeignKey("dbo.Team", t => t.PreviousTeamRef_Id)
                .Index(t => t.LeagueSeasonId)
                .Index(t => t.PreviousTeamRef_Id);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        MiddleName = c.String(),
                        LastName = c.String(nullable: false),
                        Image = c.String(),
                        Height = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        YearOfBirth = c.Int(nullable: false),
                        Team_Id = c.Guid(),
                        UId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Team", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.PlayerPerTeam",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PlayerId = c.Guid(nullable: false),
                        TeamId = c.Guid(nullable: false),
                        LeagueSeason_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeason_Id)
                .ForeignKey("dbo.Player", t => t.PlayerId)
                .ForeignKey("dbo.Team", t => t.TeamId)
                .Index(t => t.PlayerId)
                .Index(t => t.TeamId)
                .Index(t => t.LeagueSeason_Id);
            
            CreateTable(
                "dbo.Stats",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GameId = c.Guid(nullable: false),
                        PlayerId = c.Guid(nullable: false),
                        JerseyNumber = c.String(nullable: false, maxLength: 2),
                        MinutesPlayed = c.Int(nullable: false),
                        TwoPtMissed = c.Int(nullable: false),
                        TwoPtMade = c.Int(nullable: false),
                        ThreePtMissed = c.Int(nullable: false),
                        ThreePtMade = c.Int(nullable: false),
                        OnLoan = c.Boolean(nullable: false),
                        FtMissed = c.Int(nullable: false),
                        FtMade = c.Int(nullable: false),
                        OReb = c.Int(nullable: false),
                        DReb = c.Int(nullable: false),
                        Ast = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                        Stl = c.Int(nullable: false),
                        Blk = c.Int(nullable: false),
                        Fd = c.Int(nullable: false),
                        Fc = c.Int(nullable: false),
                        Td = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Game", t => t.GameId)
                .ForeignKey("dbo.Player", t => t.PlayerId)
                .Index(t => t.GameId)
                .Index(t => t.PlayerId);
            
            CreateTable(
                "dbo.SingleGameModel",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Season = c.String(),
                        Liga = c.String(),
                        Date = c.String(),
                        Time = c.String(),
                        TeamA = c.String(),
                        TeamB = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Game", "TeamBId", "dbo.Team");
            DropForeignKey("dbo.Game", "TeamAId", "dbo.Team");
            DropForeignKey("dbo.Game", "SeasonId", "dbo.Season");
            DropForeignKey("dbo.Team", "PreviousTeamRef_Id", "dbo.Team");
            DropForeignKey("dbo.Player", "Team_Id", "dbo.Team");
            DropForeignKey("dbo.Stats", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.Stats", "GameId", "dbo.Game");
            DropForeignKey("dbo.PlayerPerTeam", "TeamId", "dbo.Team");
            DropForeignKey("dbo.PlayerPerTeam", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.PlayerPerTeam", "LeagueSeason_Id", "dbo.LeagueSeason");
            DropForeignKey("dbo.Team", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.Game", "Team_Id", "dbo.Team");
            DropForeignKey("dbo.LeagueSeason", "SeasonId", "dbo.Season");
            DropForeignKey("dbo.Round", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.LeagueSeason", "LeagueId", "dbo.League");
            DropForeignKey("dbo.Game", "RoundId", "dbo.Round");
            DropForeignKey("dbo.Referee", "Game_Id", "dbo.Game");
            DropIndex("dbo.Stats", new[] { "PlayerId" });
            DropIndex("dbo.Stats", new[] { "GameId" });
            DropIndex("dbo.PlayerPerTeam", new[] { "LeagueSeason_Id" });
            DropIndex("dbo.PlayerPerTeam", new[] { "TeamId" });
            DropIndex("dbo.PlayerPerTeam", new[] { "PlayerId" });
            DropIndex("dbo.Player", new[] { "Team_Id" });
            DropIndex("dbo.Team", new[] { "PreviousTeamRef_Id" });
            DropIndex("dbo.Team", new[] { "LeagueSeasonId" });
            DropIndex("dbo.LeagueSeason", new[] { "SeasonId" });
            DropIndex("dbo.LeagueSeason", new[] { "LeagueId" });
            DropIndex("dbo.Round", new[] { "LeagueSeasonId" });
            DropIndex("dbo.Referee", new[] { "Game_Id" });
            DropIndex("dbo.Game", new[] { "Team_Id" });
            DropIndex("dbo.Game", new[] { "TeamBId" });
            DropIndex("dbo.Game", new[] { "TeamAId" });
            DropIndex("dbo.Game", new[] { "RoundId" });
            DropIndex("dbo.Game", new[] { "SeasonId" });
            DropTable("dbo.SingleGameModel");
            DropTable("dbo.Stats");
            DropTable("dbo.PlayerPerTeam");
            DropTable("dbo.Player");
            DropTable("dbo.Team");
            DropTable("dbo.Season");
            DropTable("dbo.League");
            DropTable("dbo.LeagueSeason");
            DropTable("dbo.Round");
            DropTable("dbo.Referee");
            DropTable("dbo.Game");
        }
    }
}
