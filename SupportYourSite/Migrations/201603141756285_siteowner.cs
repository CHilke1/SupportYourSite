namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class siteowner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteOwners",
                c => new
                    {
                        WebsiteID = c.Int(nullable: false),
                        OwnerName = c.String(maxLength: 100),
                        OwnerEmail = c.String(nullable: false),
                        PayPalInfo = c.String(),
                        OwnerStatement = c.String(),
                    })
                .PrimaryKey(t => t.WebsiteID)
                .ForeignKey("dbo.Websites", t => t.WebsiteID)
                .Index(t => t.WebsiteID);
            
            AddColumn("dbo.Websites", "SiteOwnerID", c => c.Int(nullable: false));
            DropColumn("dbo.Websites", "OwnerName");
            DropColumn("dbo.Websites", "OwnerEmail");
            DropColumn("dbo.Websites", "PayPalInfo");
            DropColumn("dbo.Websites", "OwnerStatement");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Websites", "OwnerStatement", c => c.String());
            AddColumn("dbo.Websites", "PayPalInfo", c => c.String());
            AddColumn("dbo.Websites", "OwnerEmail", c => c.String(nullable: false));
            AddColumn("dbo.Websites", "OwnerName", c => c.String(maxLength: 100));
            DropForeignKey("dbo.SiteOwners", "WebsiteID", "dbo.Websites");
            DropIndex("dbo.SiteOwners", new[] { "WebsiteID" });
            DropColumn("dbo.Websites", "SiteOwnerID");
            DropTable("dbo.SiteOwners");
        }
    }
}
