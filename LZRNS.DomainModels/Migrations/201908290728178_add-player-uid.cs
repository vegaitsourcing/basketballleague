namespace LZRNS.DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addplayeruid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "UId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Player", "UId");
        }
    }
}
