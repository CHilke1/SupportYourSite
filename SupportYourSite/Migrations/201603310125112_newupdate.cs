namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Websites", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Websites", "URL", c => c.String(nullable: false));
            AlterColumn("dbo.SiteOwners", "OwnerEmail", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SiteOwners", "OwnerEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Websites", "URL", c => c.String());
            AlterColumn("dbo.Websites", "Name", c => c.String());
        }
    }
}
