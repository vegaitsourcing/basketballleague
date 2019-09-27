namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RemoveTeamIdFromPlayerTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Player", "Team_Id", "dbo.Team");
            DropIndex("dbo.Player", new[] { "Team_Id" });
            DropColumn("dbo.Player", "Team_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.Player", "Team_Id", c => c.Guid());
            CreateIndex("dbo.Player", "Team_Id");
            AddForeignKey("dbo.Player", "Team_Id", "dbo.Team", "Id");
        }
    }
}