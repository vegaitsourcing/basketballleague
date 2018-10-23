namespace LZRNS.DomainModels.Migrations
{
	using System.Data.Entity.Migrations;
    
    public partial class modelchange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SingleGameModel",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Season = c.String(),
                        Liga = c.String(),
                        Date = c.String(),
                        Time = c.String(),
                        TeamA = c.String(),
                        TeamB = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SingleGameModel");
        }
    }
}
