namespace SmartCattle.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mergefarmandfarmmap : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("SmartCattle.FarmTbl", "ID", "SmartCattle.FarmMapTbl");
            DropIndex("SmartCattle.FarmTbl", new[] { "ID" });
            AddColumn("SmartCattle.FarmTbl", "MapGeoJson", c => c.String());
            AddColumn("SmartCattle.FarmTbl", "MapFilePath", c => c.String());
            DropTable("SmartCattle.FarmMapTbl");
        }
        
        public override void Down()
        {
            CreateTable(
                "SmartCattle.FarmMapTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MapType = c.String(),
                        MapGeoJson = c.String(),
                        MapFilePath = c.String(),
                        FarmId = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("SmartCattle.FarmTbl", "MapFilePath");
            DropColumn("SmartCattle.FarmTbl", "MapGeoJson");
            CreateIndex("SmartCattle.FarmTbl", "ID");
            AddForeignKey("SmartCattle.FarmTbl", "ID", "SmartCattle.FarmMapTbl", "ID");
        }
    }
}
