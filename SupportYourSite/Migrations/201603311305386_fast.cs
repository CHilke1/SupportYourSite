namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fast : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Websites", "Name", c => c.String());
            AlterColumn("dbo.Websites", "URL", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Websites", "URL", c => c.String(nullable: false));
            AlterColumn("dbo.Websites", "Name", c => c.String(nullable: false));
        }
    }
}
