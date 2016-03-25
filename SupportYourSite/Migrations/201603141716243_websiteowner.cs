namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class websiteowner : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CategoryWebsites", newName: "WebsiteCategories");
            DropForeignKey("dbo.Websites", "ProprietorID", "dbo.Proprietors");
            DropIndex("dbo.Websites", new[] { "ProprietorID" });
            DropPrimaryKey("dbo.WebsiteCategories");
            AddColumn("dbo.Websites", "OwnerName", c => c.String(maxLength: 100));
            AddColumn("dbo.Websites", "OwnerEmail", c => c.String(nullable: false));
            AddColumn("dbo.Websites", "PayPalInfo", c => c.String());
            AddPrimaryKey("dbo.WebsiteCategories", new[] { "Website_WebsiteID", "Category_CategoryID" });
            DropColumn("dbo.Websites", "ProprietorID");
            DropTable("dbo.Proprietors");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Proprietors",
                c => new
                    {
                        ProprietorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(nullable: false),
                        PayPalInfo = c.String(),
                    })
                .PrimaryKey(t => t.ProprietorId);
            
            AddColumn("dbo.Websites", "ProprietorID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.WebsiteCategories");
            DropColumn("dbo.Websites", "PayPalInfo");
            DropColumn("dbo.Websites", "OwnerEmail");
            DropColumn("dbo.Websites", "OwnerName");
            AddPrimaryKey("dbo.WebsiteCategories", new[] { "Category_CategoryID", "Website_WebsiteID" });
            CreateIndex("dbo.Websites", "ProprietorID");
            AddForeignKey("dbo.Websites", "ProprietorID", "dbo.Proprietors", "ProprietorId", cascadeDelete: true);
            RenameTable(name: "dbo.WebsiteCategories", newName: "CategoryWebsites");
        }
    }
}
