namespace HTML_Parser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Links", "ParsedData_Id", "dbo.ParsedDatas");
            DropIndex("dbo.Links", new[] { "ParsedData_Id" });
            AddColumn("dbo.ParsedDatas", "LinkId_Id", c => c.Long());
            CreateIndex("dbo.ParsedDatas", "LinkId_Id");
            AddForeignKey("dbo.ParsedDatas", "LinkId_Id", "dbo.Links", "Id");
            DropColumn("dbo.Links", "ParsedData_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Links", "ParsedData_Id", c => c.Long());
            DropForeignKey("dbo.ParsedDatas", "LinkId_Id", "dbo.Links");
            DropIndex("dbo.ParsedDatas", new[] { "LinkId_Id" });
            DropColumn("dbo.ParsedDatas", "LinkId_Id");
            CreateIndex("dbo.Links", "ParsedData_Id");
            AddForeignKey("dbo.Links", "ParsedData_Id", "dbo.ParsedDatas", "Id");
        }
    }
}
