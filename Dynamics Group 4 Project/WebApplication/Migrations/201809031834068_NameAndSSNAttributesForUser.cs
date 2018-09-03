namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameAndSSNAttributesForUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMapModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserWebAppId = c.Guid(nullable: false),
                        ClientDynamicsId = c.Guid(nullable: false),
                        UserDynamicsId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserMapModels");
        }
    }
}
