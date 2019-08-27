namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.League", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Season", "Name", c => c.String(nullable: false, maxLength: 120));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Season", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.League", "Name", c => c.String(nullable: false));
        }
    }
}
