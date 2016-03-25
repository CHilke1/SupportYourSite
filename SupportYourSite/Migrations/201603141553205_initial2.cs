namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {

        public override void Up()
        {
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

            DropForeignKey("dbo.Proprietors", "ProprietorId", "dbo.Websites");
            DropIndex("dbo.Proprietors", new[] { "ProprietorId" });
            DropPrimaryKey("dbo.Proprietors");
            AlterColumn("dbo.Proprietors", "ProprietorId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Proprietors", "ProprietorId");
            CreateIndex("dbo.Websites", "ProprietorID");
            AddForeignKey("dbo.Websites", "ProprietorID", "dbo.Proprietors", "ProprietorId", cascadeDelete: true);
            DropColumn("dbo.Proprietors", "WebsiteID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proprietors", "WebsiteID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Websites", "ProprietorID", "dbo.Proprietors");
            DropIndex("dbo.Websites", new[] { "ProprietorID" });
            DropPrimaryKey("dbo.Proprietors");
            AlterColumn("dbo.Proprietors", "ProprietorId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Proprietors", "ProprietorId");
            CreateIndex("dbo.Proprietors", "ProprietorId");
            AddForeignKey("dbo.Proprietors", "ProprietorId", "dbo.Websites", "WebsiteID");
        }
    }
}
