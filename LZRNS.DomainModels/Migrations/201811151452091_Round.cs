namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Round : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Round", "RoundName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Round", "RoundName", c => c.String(nullable: false));
        }
    }
}
