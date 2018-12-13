namespace HTML_Parser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Links", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Links", "AddingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ParsedDatas", "ParsedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Proxies", "MaxRequests", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proxies", "MaxRequests");
            DropColumn("dbo.ParsedDatas", "ParsedTime");
            DropColumn("dbo.Links", "AddingDate");
            DropColumn("dbo.Links", "ProductId");
        }
    }
}
