namespace HTML_Parser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ParsedDatas", "LinkId_Id", "dbo.Links");
            DropIndex("dbo.ParsedDatas", new[] { "LinkId_Id" });
            AddColumn("dbo.ParsedDatas", "LinkId", c => c.Long(nullable: false));
            DropColumn("dbo.ParsedDatas", "LinkId_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParsedDatas", "LinkId_Id", c => c.Long());
            DropColumn("dbo.ParsedDatas", "LinkId");
            CreateIndex("dbo.ParsedDatas", "LinkId_Id");
            AddForeignKey("dbo.ParsedDatas", "LinkId_Id", "dbo.Links", "Id");
        }
    }
}
