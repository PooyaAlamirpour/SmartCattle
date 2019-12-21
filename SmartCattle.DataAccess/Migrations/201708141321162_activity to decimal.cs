namespace SmartCattle.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activitytodecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SmartCattle.ActivityStateTbl", "Sitting", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SmartCattle.ActivityStateTbl", "Standing", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SmartCattle.ActivityStateTbl", "Walking", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SmartCattle.ActivityStateTbl", "Eating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SmartCattle.ActivityStateTbl", "Rumination", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SmartCattle.ActivityStateTbl", "Drinking", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("SmartCattle.ActivityStateTbl", "Drinking", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Rumination", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Eating", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Walking", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Standing", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Sitting", c => c.Single(nullable: false));
        }
    }
}
