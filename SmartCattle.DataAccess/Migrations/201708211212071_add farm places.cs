namespace SmartCattle.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfarmplaces : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("SmartCattle.CattlePositionTbl", "FarmZone_ID", "SmartCattle.FarmZoneTbl");
            DropForeignKey("SmartCattle.FarmZoneTbl", "UserId", "SmartCattle.AspNetUsers");
            DropIndex("SmartCattle.CattlePositionTbl", new[] { "FarmZone_ID" });
            DropIndex("SmartCattle.FarmZoneTbl", new[] { "UserId" });
            CreateTable(
                "SmartCattle.FarmPlacesTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("SmartCattle.FarmZoneTbl", "Title", c => c.String());
            AddColumn("SmartCattle.FarmZoneTbl", "PlaceId", c => c.Int());
            AlterColumn("SmartCattle.FarmZoneTbl", "UserId", c => c.String());
            CreateIndex("SmartCattle.FarmZoneTbl", "PlaceId");
            AddForeignKey("SmartCattle.FarmZoneTbl", "PlaceId", "SmartCattle.FarmPlacesTbl", "ID");
            DropColumn("SmartCattle.CattlePositionTbl", "FarmZone_ID");
            DropColumn("SmartCattle.FarmZoneTbl", "zoneId");
            DropColumn("SmartCattle.FarmZoneTbl", "MAC");
            DropColumn("SmartCattle.FarmZoneTbl", "detectorTime");
            DropColumn("SmartCattle.FarmZoneTbl", "LastReceivedId");
            DropColumn("SmartCattle.FarmZoneTbl", "name");
        }
        
        public override void Down()
        {
            AddColumn("SmartCattle.FarmZoneTbl", "name", c => c.String());
            AddColumn("SmartCattle.FarmZoneTbl", "LastReceivedId", c => c.String());
            AddColumn("SmartCattle.FarmZoneTbl", "detectorTime", c => c.Int(nullable: false));
            AddColumn("SmartCattle.FarmZoneTbl", "MAC", c => c.String());
            AddColumn("SmartCattle.FarmZoneTbl", "zoneId", c => c.Int(nullable: false));
            AddColumn("SmartCattle.CattlePositionTbl", "FarmZone_ID", c => c.Int());
            DropForeignKey("SmartCattle.FarmZoneTbl", "PlaceId", "SmartCattle.FarmPlacesTbl");
            DropIndex("SmartCattle.FarmZoneTbl", new[] { "PlaceId" });
            AlterColumn("SmartCattle.FarmZoneTbl", "UserId", c => c.String(maxLength: 128));
            DropColumn("SmartCattle.FarmZoneTbl", "PlaceId");
            DropColumn("SmartCattle.FarmZoneTbl", "Title");
            DropTable("SmartCattle.FarmPlacesTbl");
            CreateIndex("SmartCattle.FarmZoneTbl", "UserId");
            CreateIndex("SmartCattle.CattlePositionTbl", "FarmZone_ID");
            AddForeignKey("SmartCattle.FarmZoneTbl", "UserId", "SmartCattle.AspNetUsers", "Id");
            AddForeignKey("SmartCattle.CattlePositionTbl", "FarmZone_ID", "SmartCattle.FarmZoneTbl", "ID");
        }
    }
}
