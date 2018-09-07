namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SeasonId = c.Guid(nullable: false),
                        Round = c.Guid(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        TeamAId = c.Guid(nullable: false),
                        TeamBId = c.Guid(nullable: false),
                        PointsA = c.Int(nullable: false),
                        PointsB = c.Int(nullable: false),
                        Team_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Seasons", t => t.SeasonId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .ForeignKey("dbo.Teams", t => t.TeamAId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.TeamBId, cascadeDelete: true)
                .Index(t => t.SeasonId)
                .Index(t => t.TeamAId)
                .Index(t => t.TeamBId)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Referees",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        LastName = c.String(),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Seasons",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        League_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leagues", t => t.League_Id)
                .Index(t => t.League_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TeamName = c.String(),
                        PreviousTeamGuid = c.Guid(nullable: false),
                        Coach = c.String(),
                        PreviousTeamRef_Id = c.Guid(),
                        Season_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.PreviousTeamRef_Id)
                .ForeignKey("dbo.Seasons", t => t.Season_Id)
                .Index(t => t.PreviousTeamRef_Id)
                .Index(t => t.Season_Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Height = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        YearOfBirth = c.Int(nullable: false),
                        Team_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Leagues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Seasons", "League_Id", "dbo.Leagues");
            DropForeignKey("dbo.Games", "TeamBId", "dbo.Teams");
            DropForeignKey("dbo.Games", "TeamAId", "dbo.Teams");
            DropForeignKey("dbo.Teams", "Season_Id", "dbo.Seasons");
            DropForeignKey("dbo.Teams", "PreviousTeamRef_Id", "dbo.Teams");
            DropForeignKey("dbo.Players", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Games", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Games", "SeasonId", "dbo.Seasons");
            DropForeignKey("dbo.Referees", "Game_Id", "dbo.Games");
            DropIndex("dbo.Players", new[] { "Team_Id" });
            DropIndex("dbo.Teams", new[] { "Season_Id" });
            DropIndex("dbo.Teams", new[] { "PreviousTeamRef_Id" });
            DropIndex("dbo.Seasons", new[] { "League_Id" });
            DropIndex("dbo.Referees", new[] { "Game_Id" });
            DropIndex("dbo.Games", new[] { "Team_Id" });
            DropIndex("dbo.Games", new[] { "TeamBId" });
            DropIndex("dbo.Games", new[] { "TeamAId" });
            DropIndex("dbo.Games", new[] { "SeasonId" });
            DropTable("dbo.Leagues");
            DropTable("dbo.Players");
            DropTable("dbo.Teams");
            DropTable("dbo.Seasons");
            DropTable("dbo.Referees");
            DropTable("dbo.Games");
        }
    }
}
