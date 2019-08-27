namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _dummymigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.League", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Season", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Season", "Name", c => c.String(nullable: false, maxLength: 120));
            AlterColumn("dbo.League", "Name", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
