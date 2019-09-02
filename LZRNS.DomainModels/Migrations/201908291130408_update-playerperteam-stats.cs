namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateplayerperteamstats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerPerTeam", "LeagueSeason_Id", c => c.Guid());
            AddColumn("dbo.Stats", "OnLoan", c => c.Boolean(nullable: false));
            CreateIndex("dbo.PlayerPerTeam", "LeagueSeason_Id");
            AddForeignKey("dbo.PlayerPerTeam", "LeagueSeason_Id", "dbo.LeagueSeason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerPerTeam", "LeagueSeason_Id", "dbo.LeagueSeason");
            DropIndex("dbo.PlayerPerTeam", new[] { "LeagueSeason_Id" });
            DropColumn("dbo.Stats", "OnLoan");
            DropColumn("dbo.PlayerPerTeam", "LeagueSeason_Id");
        }
    }
}
