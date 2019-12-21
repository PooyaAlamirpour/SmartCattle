namespace SmartCattle.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SmartCattle.ActivityStateTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        jsonedActivities = c.String(),
                        Sitting = c.Double(nullable: false),
                        Standing = c.Double(nullable: false),
                        Walking = c.Double(nullable: false),
                        Eating = c.Double(nullable: false),
                        Rumination = c.Double(nullable: false),
                        Drinking = c.Double(nullable: false),
                        cattleId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FarmID = c.Int(nullable: false),
                        LastRecievedId = c.Long(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.CattleTbl", t => t.cattleId, cascadeDelete: true)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .Index(t => t.cattleId)
                .Index(t => t.FarmID);
            
            CreateTable(
                "SmartCattle.CattleTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        age = c.Int(nullable: false),
                        preg = c.Int(nullable: false),
                        milkAvg = c.Double(nullable: false),
                        healthStatus = c.Int(nullable: false),
                        animalNumber = c.Int(nullable: false),
                        heatStatus = c.Int(nullable: false),
                        birthDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Dim = c.Int(nullable: false),
                        fertilityStatus = c.Int(nullable: false),
                        lactationNumber = c.Int(nullable: false),
                        InseminationCount = c.Int(nullable: false),
                        lastInseminationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        lastCalvingDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        calvingCount = c.Int(nullable: false),
                        CattleGroupId = c.Int(),
                        FreeStallId = c.Int(nullable: false),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CattleHerd_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.CattleGroupTbl", t => t.CattleGroupId)
                .ForeignKey("SmartCattle.CattleHerds", t => t.CattleHerd_ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID, cascadeDelete: true)
                .ForeignKey("SmartCattle.FreeStallTbl", t => t.FreeStallId, cascadeDelete: false)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.CattleGroupId)
                .Index(t => t.FreeStallId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId)
                .Index(t => t.CattleHerd_ID);
            
            CreateTable(
                "SmartCattle.CattleGroupTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        Description = c.String(),
                        order = c.String(),
                        code = c.String(),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.FarmTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        LogoUrl = c.String(),
                        email = c.String(),
                        website = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmMapTbl", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "SmartCattle.CattleTHITbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TdbValue = c.Double(nullable: false),
                        RHValue = c.Double(nullable: false),
                        THIValue = c.Double(nullable: false),
                        Location = c.Geography(),
                        FreeStallId = c.Int(),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Cattle_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID, cascadeDelete: true)
                .ForeignKey("SmartCattle.FreeStallTbl", t => t.FreeStallId)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.CattleTbl", t => t.Cattle_ID)
                .Index(t => t.FreeStallId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId)
                .Index(t => t.Cattle_ID);
            
            CreateTable(
                "SmartCattle.FreeStallTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        Description = c.String(),
                        code = c.Int(nullable: false),
                        location = c.Geography(),
                        FarmID = c.Int(nullable: false),
                        GroupID = c.Int(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID, cascadeDelete: true)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.CattleGroupTbl", t => t.GroupID)
                .Index(t => t.FarmID)
                .Index(t => t.GroupID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        firstName = c.String(),
                        lastName = c.String(),
                        lastLogin = c.DateTime(precision: 7, storeType: "datetime2"),
                        avatarUrl = c.String(),
                        phone = c.String(),
                        FarmID = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID, cascadeDelete: true)
                .Index(t => t.FarmID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "SmartCattle.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.UserNotificationTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Seen = c.Boolean(nullable: false),
                        Received = c.Boolean(nullable: false),
                        SeenDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        NotificationId = c.Int(nullable: false),
                        priority = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.NotificationTbl", t => t.NotificationId, cascadeDelete: true)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.NotificationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.NotificationTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Rule = c.Int(nullable: false),
                        FarmId = c.Int(nullable: false),
                        UserId = c.String(),
                        UserRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmId, cascadeDelete: true)
                .ForeignKey("SmartCattle.AspNetRoles", t => t.UserRole_Id)
                .Index(t => t.FarmId)
                .Index(t => t.UserRole_Id);
            
            CreateTable(
                "SmartCattle.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("SmartCattle.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "SmartCattle.CattleEventTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        value = c.Int(nullable: false),
                        description = c.String(),
                        cattleId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.CattleTbl", t => t.cattleId, cascadeDelete: true)
                .Index(t => t.cattleId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
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
            
            CreateTable(
                "SmartCattle.FarmSettingTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        key = c.String(),
                        value = c.String(),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.CattleFrtilityStateTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        value = c.Int(nullable: false),
                        description = c.String(),
                        cattleId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.CattleTbl", t => t.cattleId, cascadeDelete: true)
                .Index(t => t.cattleId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.GroupTimeBudgetTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        season = c.Int(nullable: false),
                        Title = c.String(),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FarmID = c.Int(nullable: false),
                        CattleGroupId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.CattleGroupTbl", t => t.CattleGroupId, cascadeDelete: true)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.FarmID)
                .Index(t => t.CattleGroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.GroupTimeBudgetItemTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Item = c.Int(nullable: false),
                        valuePercent = c.Double(nullable: false),
                        colorCode = c.String(),
                        description = c.String(),
                        TimeBudgetId = c.Int(nullable: false),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID, cascadeDelete: true)
                .ForeignKey("SmartCattle.GroupTimeBudgetTbl", t => t.TimeBudgetId, cascadeDelete: false)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.TimeBudgetId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.CattleHealthStateTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        value = c.Int(nullable: false),
                        description = c.String(),
                        cattleId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.CattleTbl", t => t.cattleId, cascadeDelete: true)
                .Index(t => t.cattleId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.CattleHeatStateTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        value = c.Int(nullable: false),
                        description = c.String(),
                        cattleId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.CattleTbl", t => t.cattleId, cascadeDelete: true)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.cattleId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.HerdEventTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        description = c.String(),
                        eventDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        herdId = c.Int(nullable: false),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.CattleHerds", t => t.herdId, cascadeDelete: true)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.herdId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.CattleHerds",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        Description = c.String(),
                        code = c.String(),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID, cascadeDelete: true)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.MilkingTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        protein = c.Double(nullable: false),
                        fat = c.Double(nullable: false),
                        turn = c.Int(nullable: false),
                        value = c.Double(nullable: false),
                        FatProteinIndex = c.Double(nullable: false),
                        cattleId = c.Int(nullable: false),
                        SCC = c.Double(nullable: false),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.CattleTbl", t => t.cattleId, cascadeDelete: true)
                .Index(t => t.cattleId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.CattlePositionTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        value = c.String(),
                        x = c.Double(nullable: false),
                        y = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        LatLong = c.Geography(),
                        cattleId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastRecievedId = c.Long(nullable: false),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        FarmZone_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.FarmZoneTbl", t => t.FarmZone_ID)
                .ForeignKey("SmartCattle.CattleTbl", t => t.cattleId, cascadeDelete: true)
                .Index(t => t.cattleId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId)
                .Index(t => t.FarmZone_ID);
            
            CreateTable(
                "SmartCattle.SensorTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MacAddress = c.String(),
                        cattleId = c.Int(),
                        lastTransmitDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        activationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        linkingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        lastSyncDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        antennaId = c.Int(nullable: false),
                        antennaName = c.String(),
                        status = c.Int(nullable: false),
                        softwareVersion = c.String(),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .Index(t => t.FarmID);
            
            CreateTable(
                "SmartCattle.TempretureTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        value = c.Double(nullable: false),
                        point = c.String(),
                        cattleId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastRecievedId = c.Long(nullable: false),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.CattleTbl", t => t.cattleId, cascadeDelete: true)
                .Index(t => t.cattleId)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.FarmZoneTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        FarmID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID)
                .Index(t => t.FarmID)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.CattleScoreTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        item = c.Int(nullable: false),
                        value = c.Double(nullable: false),
                        CattleId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.CattleTbl", t => t.CattleId, cascadeDelete: true)
                .Index(t => t.CattleId);
            
            CreateTable(
                "SmartCattle.TimeBudgetTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CattleId = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.CattleTbl", t => t.CattleId, cascadeDelete: true)
                .Index(t => t.CattleId);
            
            CreateTable(
                "SmartCattle.BudgetItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Item = c.Int(nullable: false),
                        valuePercent = c.Double(nullable: false),
                        description = c.String(),
                        TimeBudget_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.TimeBudgetTbl", t => t.TimeBudget_ID)
                .Index(t => t.TimeBudget_ID);
            
            CreateTable(
                "SmartCattle.ApplicationSettingTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        key = c.String(),
                        value = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "SmartCattle.RoleNotificationTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NotificationId = c.Int(nullable: false),
                        RoleId = c.String(maxLength: 128),
                        FarmId = c.Int(nullable: false),
                        priority = c.Int(nullable: false),
                        Maskable = c.Boolean(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmId, cascadeDelete: true)
                .ForeignKey("SmartCattle.NotificationTbl", t => t.NotificationId, cascadeDelete: false)
                .ForeignKey("SmartCattle.AspNetRoles", t => t.RoleId)
                .Index(t => t.NotificationId)
                .Index(t => t.RoleId)
                .Index(t => t.FarmId);
            
            CreateTable(
                "SmartCattle.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        FarmID = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCattle.FarmTbl", t => t.FarmID, cascadeDelete: true)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.User_Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex")
                .Index(t => new { t.Name, t.FarmID }, unique: true, name: "ComplexRoleNameIndex")
                .Index(t => t.User_Id);
            
            CreateTable(
                "SmartCattle.RolePermissionTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        Read = c.Boolean(nullable: false),
                        Write = c.Boolean(nullable: false),
                        Description = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "SmartCattle.UserSettingTbl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        key = c.String(),
                        value = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("SmartCattle.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "SmartCattle.RolePermissionUserRoles",
                c => new
                    {
                        RolePermission_ID = c.Int(nullable: false),
                        UserRole_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RolePermission_ID, t.UserRole_Id })
                .ForeignKey("SmartCattle.RolePermissionTbl", t => t.RolePermission_ID, cascadeDelete: true)
                .ForeignKey("SmartCattle.AspNetRoles", t => t.UserRole_Id, cascadeDelete: false)
                .Index(t => t.RolePermission_ID)
                .Index(t => t.UserRole_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SmartCattle.UserSettingTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.AspNetUserRoles", "RoleId", "SmartCattle.AspNetRoles");
            DropForeignKey("SmartCattle.RoleNotificationTbl", "RoleId", "SmartCattle.AspNetRoles");
            DropForeignKey("SmartCattle.AspNetRoles", "User_Id", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.RolePermissionUserRoles", "UserRole_Id", "SmartCattle.AspNetRoles");
            DropForeignKey("SmartCattle.RolePermissionUserRoles", "RolePermission_ID", "SmartCattle.RolePermissionTbl");
            DropForeignKey("SmartCattle.NotificationTbl", "UserRole_Id", "SmartCattle.AspNetRoles");
            DropForeignKey("SmartCattle.AspNetRoles", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.RoleNotificationTbl", "NotificationId", "SmartCattle.NotificationTbl");
            DropForeignKey("SmartCattle.RoleNotificationTbl", "FarmId", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.ActivityStateTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.CattleTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.TempretureTbl", "cattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.CattlePositionTbl", "cattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.MilkingTbl", "cattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.CattleHealthStateTbl", "cattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.CattleTbl", "FreeStallId", "SmartCattle.FreeStallTbl");
            DropForeignKey("SmartCattle.CattleFrtilityStateTbl", "cattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.CattleTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.CattleEventTbl", "cattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.TimeBudgetTbl", "CattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.BudgetItems", "TimeBudget_ID", "SmartCattle.TimeBudgetTbl");
            DropForeignKey("SmartCattle.CattleTHITbl", "Cattle_ID", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.CattleScoreTbl", "CattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.CattleTbl", "CattleHerd_ID", "SmartCattle.CattleHerds");
            DropForeignKey("SmartCattle.CattleTbl", "CattleGroupId", "SmartCattle.CattleGroupTbl");
            DropForeignKey("SmartCattle.CattleGroupTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.FreeStallTbl", "GroupID", "SmartCattle.CattleGroupTbl");
            DropForeignKey("SmartCattle.CattleGroupTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.FarmZoneTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.FarmZoneTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.CattlePositionTbl", "FarmZone_ID", "SmartCattle.FarmZoneTbl");
            DropForeignKey("SmartCattle.TempretureTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.TempretureTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.SensorTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.CattlePositionTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.CattlePositionTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.MilkingTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.MilkingTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.HerdEventTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.HerdEventTbl", "herdId", "SmartCattle.CattleHerds");
            DropForeignKey("SmartCattle.CattleHerds", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.CattleHerds", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.HerdEventTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.CattleHeatStateTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.CattleHeatStateTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.CattleHeatStateTbl", "cattleId", "SmartCattle.CattleTbl");
            DropForeignKey("SmartCattle.CattleHealthStateTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.CattleHealthStateTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.GroupTimeBudgetTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.GroupTimeBudgetItemTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.GroupTimeBudgetItemTbl", "TimeBudgetId", "SmartCattle.GroupTimeBudgetTbl");
            DropForeignKey("SmartCattle.GroupTimeBudgetItemTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.GroupTimeBudgetTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.GroupTimeBudgetTbl", "CattleGroupId", "SmartCattle.CattleGroupTbl");
            DropForeignKey("SmartCattle.CattleFrtilityStateTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.CattleFrtilityStateTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.FarmSettingTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.FarmSettingTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.FarmTbl", "ID", "SmartCattle.FarmMapTbl");
            DropForeignKey("SmartCattle.CattleEventTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.CattleEventTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.CattleTHITbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.FreeStallTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.AspNetUserRoles", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.UserNotificationTbl", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.UserNotificationTbl", "NotificationId", "SmartCattle.NotificationTbl");
            DropForeignKey("SmartCattle.NotificationTbl", "FarmId", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.AspNetUserLogins", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.AspNetUsers", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.AspNetUserClaims", "UserId", "SmartCattle.AspNetUsers");
            DropForeignKey("SmartCattle.CattleTHITbl", "FreeStallId", "SmartCattle.FreeStallTbl");
            DropForeignKey("SmartCattle.FreeStallTbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.CattleTHITbl", "FarmID", "SmartCattle.FarmTbl");
            DropForeignKey("SmartCattle.ActivityStateTbl", "cattleId", "SmartCattle.CattleTbl");
            DropIndex("SmartCattle.RolePermissionUserRoles", new[] { "UserRole_Id" });
            DropIndex("SmartCattle.RolePermissionUserRoles", new[] { "RolePermission_ID" });
            DropIndex("SmartCattle.UserSettingTbl", new[] { "UserId" });
            DropIndex("SmartCattle.AspNetRoles", new[] { "User_Id" });
            DropIndex("SmartCattle.AspNetRoles", "ComplexRoleNameIndex");
            DropIndex("SmartCattle.AspNetRoles", "RoleNameIndex");
            DropIndex("SmartCattle.RoleNotificationTbl", new[] { "FarmId" });
            DropIndex("SmartCattle.RoleNotificationTbl", new[] { "RoleId" });
            DropIndex("SmartCattle.RoleNotificationTbl", new[] { "NotificationId" });
            DropIndex("SmartCattle.BudgetItems", new[] { "TimeBudget_ID" });
            DropIndex("SmartCattle.TimeBudgetTbl", new[] { "CattleId" });
            DropIndex("SmartCattle.CattleScoreTbl", new[] { "CattleId" });
            DropIndex("SmartCattle.FarmZoneTbl", new[] { "UserId" });
            DropIndex("SmartCattle.FarmZoneTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.TempretureTbl", new[] { "UserId" });
            DropIndex("SmartCattle.TempretureTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.TempretureTbl", new[] { "cattleId" });
            DropIndex("SmartCattle.SensorTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattlePositionTbl", new[] { "FarmZone_ID" });
            DropIndex("SmartCattle.CattlePositionTbl", new[] { "UserId" });
            DropIndex("SmartCattle.CattlePositionTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattlePositionTbl", new[] { "cattleId" });
            DropIndex("SmartCattle.MilkingTbl", new[] { "UserId" });
            DropIndex("SmartCattle.MilkingTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.MilkingTbl", new[] { "cattleId" });
            DropIndex("SmartCattle.CattleHerds", new[] { "UserId" });
            DropIndex("SmartCattle.CattleHerds", new[] { "FarmID" });
            DropIndex("SmartCattle.HerdEventTbl", new[] { "UserId" });
            DropIndex("SmartCattle.HerdEventTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.HerdEventTbl", new[] { "herdId" });
            DropIndex("SmartCattle.CattleHeatStateTbl", new[] { "UserId" });
            DropIndex("SmartCattle.CattleHeatStateTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleHeatStateTbl", new[] { "cattleId" });
            DropIndex("SmartCattle.CattleHealthStateTbl", new[] { "UserId" });
            DropIndex("SmartCattle.CattleHealthStateTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleHealthStateTbl", new[] { "cattleId" });
            DropIndex("SmartCattle.GroupTimeBudgetItemTbl", new[] { "UserId" });
            DropIndex("SmartCattle.GroupTimeBudgetItemTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.GroupTimeBudgetItemTbl", new[] { "TimeBudgetId" });
            DropIndex("SmartCattle.GroupTimeBudgetTbl", new[] { "UserId" });
            DropIndex("SmartCattle.GroupTimeBudgetTbl", new[] { "CattleGroupId" });
            DropIndex("SmartCattle.GroupTimeBudgetTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleFrtilityStateTbl", new[] { "UserId" });
            DropIndex("SmartCattle.CattleFrtilityStateTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleFrtilityStateTbl", new[] { "cattleId" });
            DropIndex("SmartCattle.FarmSettingTbl", new[] { "UserId" });
            DropIndex("SmartCattle.FarmSettingTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleEventTbl", new[] { "UserId" });
            DropIndex("SmartCattle.CattleEventTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleEventTbl", new[] { "cattleId" });
            DropIndex("SmartCattle.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("SmartCattle.AspNetUserRoles", new[] { "UserId" });
            DropIndex("SmartCattle.NotificationTbl", new[] { "UserRole_Id" });
            DropIndex("SmartCattle.NotificationTbl", new[] { "FarmId" });
            DropIndex("SmartCattle.UserNotificationTbl", new[] { "UserId" });
            DropIndex("SmartCattle.UserNotificationTbl", new[] { "NotificationId" });
            DropIndex("SmartCattle.AspNetUserLogins", new[] { "UserId" });
            DropIndex("SmartCattle.AspNetUserClaims", new[] { "UserId" });
            DropIndex("SmartCattle.AspNetUsers", "UserNameIndex");
            DropIndex("SmartCattle.AspNetUsers", new[] { "FarmID" });
            DropIndex("SmartCattle.FreeStallTbl", new[] { "UserId" });
            DropIndex("SmartCattle.FreeStallTbl", new[] { "GroupID" });
            DropIndex("SmartCattle.FreeStallTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleTHITbl", new[] { "Cattle_ID" });
            DropIndex("SmartCattle.CattleTHITbl", new[] { "UserId" });
            DropIndex("SmartCattle.CattleTHITbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleTHITbl", new[] { "FreeStallId" });
            DropIndex("SmartCattle.FarmTbl", new[] { "ID" });
            DropIndex("SmartCattle.CattleGroupTbl", new[] { "UserId" });
            DropIndex("SmartCattle.CattleGroupTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleTbl", new[] { "CattleHerd_ID" });
            DropIndex("SmartCattle.CattleTbl", new[] { "UserId" });
            DropIndex("SmartCattle.CattleTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.CattleTbl", new[] { "FreeStallId" });
            DropIndex("SmartCattle.CattleTbl", new[] { "CattleGroupId" });
            DropIndex("SmartCattle.ActivityStateTbl", new[] { "FarmID" });
            DropIndex("SmartCattle.ActivityStateTbl", new[] { "cattleId" });
            DropTable("SmartCattle.RolePermissionUserRoles");
            DropTable("SmartCattle.UserSettingTbl");
            DropTable("SmartCattle.RolePermissionTbl");
            DropTable("SmartCattle.AspNetRoles");
            DropTable("SmartCattle.RoleNotificationTbl");
            DropTable("SmartCattle.ApplicationSettingTbl");
            DropTable("SmartCattle.BudgetItems");
            DropTable("SmartCattle.TimeBudgetTbl");
            DropTable("SmartCattle.CattleScoreTbl");
            DropTable("SmartCattle.FarmZoneTbl");
            DropTable("SmartCattle.TempretureTbl");
            DropTable("SmartCattle.SensorTbl");
            DropTable("SmartCattle.CattlePositionTbl");
            DropTable("SmartCattle.MilkingTbl");
            DropTable("SmartCattle.CattleHerds");
            DropTable("SmartCattle.HerdEventTbl");
            DropTable("SmartCattle.CattleHeatStateTbl");
            DropTable("SmartCattle.CattleHealthStateTbl");
            DropTable("SmartCattle.GroupTimeBudgetItemTbl");
            DropTable("SmartCattle.GroupTimeBudgetTbl");
            DropTable("SmartCattle.CattleFrtilityStateTbl");
            DropTable("SmartCattle.FarmSettingTbl");
            DropTable("SmartCattle.FarmMapTbl");
            DropTable("SmartCattle.CattleEventTbl");
            DropTable("SmartCattle.AspNetUserRoles");
            DropTable("SmartCattle.NotificationTbl");
            DropTable("SmartCattle.UserNotificationTbl");
            DropTable("SmartCattle.AspNetUserLogins");
            DropTable("SmartCattle.AspNetUserClaims");
            DropTable("SmartCattle.AspNetUsers");
            DropTable("SmartCattle.FreeStallTbl");
            DropTable("SmartCattle.CattleTHITbl");
            DropTable("SmartCattle.FarmTbl");
            DropTable("SmartCattle.CattleGroupTbl");
            DropTable("SmartCattle.CattleTbl");
            DropTable("SmartCattle.ActivityStateTbl");
        }
    }
}
