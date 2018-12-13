namespace HTML_Parser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalColumnToProxy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proxies", "RequestCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proxies", "RequestCount");
        }
    }
}
