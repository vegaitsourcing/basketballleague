namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateteam : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Team", "PreviousTeamGuid", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Team", "PreviousTeamGuid", c => c.Guid(nullable: false));
        }
    }
}
