namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Websites", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Websites", "Image");
        }
    }
}
