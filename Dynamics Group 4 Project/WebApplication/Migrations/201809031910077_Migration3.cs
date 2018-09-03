namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserMapModels", "FirstName", c => c.String());
            AddColumn("dbo.UserMapModels", "LastName", c => c.String());
            AddColumn("dbo.UserMapModels", "SSN", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserMapModels", "SSN");
            DropColumn("dbo.UserMapModels", "LastName");
            DropColumn("dbo.UserMapModels", "FirstName");
        }
    }
}
