namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateteamname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Team", "TeamName", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Team", "TeamName", c => c.String(nullable: false));
        }
    }
}
