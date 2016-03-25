namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        CommentText = c.String(),
                        CommentName = c.String(),
                        CommentEmail = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                        WebsiteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Websites", t => t.WebsiteID, cascadeDelete: true)
                .Index(t => t.WebsiteID);
            
            CreateTable(
                "dbo.Websites",
                c => new
                    {
                        WebsiteID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        URL = c.String(),
                        iTunes = c.String(),
                        RSS = c.String(),
                        OwnerStatement = c.String(),
                        Description = c.String(),
                        ProprietorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WebsiteID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Proprietors",
                c => new
                    {
                        ProprietorId = c.Int(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        PayPalInfo = c.String(),
                        WebsiteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProprietorId)
                .ForeignKey("dbo.Websites", t => t.ProprietorId)
                .Index(t => t.ProprietorId);
            
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        Date = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        Salutation = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Suffix = c.String(),
                        Country = c.String(),
                        AddressType = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        Zipcode = c.String(),
                        State = c.String(),
                        Phone = c.String(),
                        Comment = c.String(),
                        Website_WebsiteID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Websites", t => t.Website_WebsiteID)
                .Index(t => t.Website_WebsiteID);
            
            CreateTable(
                "dbo.CategoryWebsites",
                c => new
                    {
                        Category_CategoryID = c.Int(nullable: false),
                        Website_WebsiteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_CategoryID, t.Website_WebsiteID })
                .ForeignKey("dbo.Categories", t => t.Category_CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Websites", t => t.Website_WebsiteID, cascadeDelete: true)
                .Index(t => t.Category_CategoryID)
                .Index(t => t.Website_WebsiteID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "Website_WebsiteID", "dbo.Websites");
            DropForeignKey("dbo.Proprietors", "ProprietorId", "dbo.Websites");
            DropForeignKey("dbo.Comments", "WebsiteID", "dbo.Websites");
            DropForeignKey("dbo.CategoryWebsites", "Website_WebsiteID", "dbo.Websites");
            DropForeignKey("dbo.CategoryWebsites", "Category_CategoryID", "dbo.Categories");
            DropIndex("dbo.CategoryWebsites", new[] { "Website_WebsiteID" });
            DropIndex("dbo.CategoryWebsites", new[] { "Category_CategoryID" });
            DropIndex("dbo.Donations", new[] { "Website_WebsiteID" });
            DropIndex("dbo.Proprietors", new[] { "ProprietorId" });
            DropIndex("dbo.Comments", new[] { "WebsiteID" });
            DropTable("dbo.CategoryWebsites");
            DropTable("dbo.Donations");
            DropTable("dbo.Proprietors");
            DropTable("dbo.Categories");
            DropTable("dbo.Websites");
            DropTable("dbo.Comments");
        }
    }
}
