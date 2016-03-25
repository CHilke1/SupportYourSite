namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class websiteid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Donations", "Website_WebsiteID", "dbo.Websites");
            DropIndex("dbo.Donations", new[] { "Website_WebsiteID" });
            RenameColumn(table: "dbo.Donations", name: "Website_WebsiteID", newName: "WebsiteID");
            AddColumn("dbo.SiteOwners", "OwnerBiogrpahy", c => c.String());
            AlterColumn("dbo.Donations", "WebsiteID", c => c.Int(nullable: false));
            CreateIndex("dbo.Donations", "WebsiteID");
            AddForeignKey("dbo.Donations", "WebsiteID", "dbo.Websites", "WebsiteID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "WebsiteID", "dbo.Websites");
            DropIndex("dbo.Donations", new[] { "WebsiteID" });
            AlterColumn("dbo.Donations", "WebsiteID", c => c.Int());
            DropColumn("dbo.SiteOwners", "OwnerBiogrpahy");
            RenameColumn(table: "dbo.Donations", name: "WebsiteID", newName: "Website_WebsiteID");
            CreateIndex("dbo.Donations", "Website_WebsiteID");
            AddForeignKey("dbo.Donations", "Website_WebsiteID", "dbo.Websites", "WebsiteID");
        }
    }
}
