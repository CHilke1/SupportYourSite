namespace SupportYourSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Proprietors", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Proprietors", "Email", c => c.String());
        }
    }
}
