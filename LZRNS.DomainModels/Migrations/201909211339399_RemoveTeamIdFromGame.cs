namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTeamIdFromGame : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Game", "Team_Id", "dbo.Team");
            DropIndex("dbo.Game", new[] { "Team_Id" });
            DropColumn("dbo.Game", "Team_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game", "Team_Id", c => c.Guid());
            CreateIndex("dbo.Game", "Team_Id");
            AddForeignKey("dbo.Game", "Team_Id", "dbo.Team", "Id");
        }
    }
}
