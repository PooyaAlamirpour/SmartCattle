namespace SmartCattle.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeactivitesdoubletofloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SmartCattle.ActivityStateTbl", "Sitting", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Standing", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Walking", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Eating", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Rumination", c => c.Single(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Drinking", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("SmartCattle.ActivityStateTbl", "Drinking", c => c.Double(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Rumination", c => c.Double(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Eating", c => c.Double(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Walking", c => c.Double(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Standing", c => c.Double(nullable: false));
            AlterColumn("SmartCattle.ActivityStateTbl", "Sitting", c => c.Double(nullable: false));
        }
    }
}
