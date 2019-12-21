using Microsoft.AspNet.Identity.EntityFramework;
using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using SmartCattle.DataAccess.Migrations;

namespace SmartCattle.DataAccess
{
   public class SmartCattleContext : IdentityDbContext<SmartCattleUser>,IDbContext
    {
        public SmartCattleContext() : base("name=SmartCattle")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SmartCattleContext, DataAccess.Migrations.Configuration>());
        }

        //public DbSet<Cattle> Cattles { set; get; } 
        //public DbSet<CattleActivityState> ActivityStates { get; set; }  
        //public DbSet<CattleEvent> CattleEvents { get; set; }
        //public DbSet<CattleFrtilityState> FrtilityStates { get; set; }
        //public DbSet<CattleGroup> CattleGroups { get; set; }
        //public DbSet<CattleHealthState> HealthStates { get; set; }
        //public DbSet<CattleHeatState> HeatStates { get; set; }
        //public DbSet<CattleMilking> Milkings { get; set; }
        //public DbSet<CattlePosition> Positions { get; set; }        
        //public DbSet<Farm> Farms { get; set; }
        //public DbSet<HerdEvent> HerdEvents { get; set; }
        ////public DbSet<FreeStall> FreeStalls { get; set; }
        //public DbSet<Sensor> Sensors { get; set; }
        //public DbSet<CattleTempreture> Tempretures { get; set; }
        //public DbSet<UserSetting> UserSettings { get; set; }      
        //public DbSet<GroupTimeBudget> CattleGroupsTimeBudgets { get; set; }
        //public DbSet<GroupTimeBudgetItem> CattleGroupsTimeBudetItems { get; set; }
        //public DbSet<TimeBudget> CattleTimeBudgets { get; set; } 
        //public DbSet<ApplicationSetting> ApplicationSettings { get; set; } 
        //public DbSet<CattleTHI> CattleTHIs { get; set; }
        //public DbSet<CattleScore> CattleScrores { get; set; }
        //public DbSet<Notification> Notifications { get; set; }
        //public DbSet<UserNotification> UserNotifications { get; set; }
        //public DbSet<RoleNotification> RoleNotifications { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<RolePermission> RolePermissions { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>().Property(r => r.Name)
               .IsRequired()
               .HasMaxLength(256)
               .HasColumnAnnotation("Index", new IndexAnnotation(
                   new IndexAttribute("RoleNameIndex", 0)
                   { IsUnique = false }));


            modelBuilder.Entity<UserRole>().Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("ComplexRoleNameIndex",0)
                    { IsUnique = true }));

            modelBuilder.Entity<UserRole>().Property(r => r.FarmID)
               .IsRequired() 
               .HasColumnAnnotation("Index", new IndexAnnotation(
                   new IndexAttribute("ComplexRoleNameIndex", 1)
                   { IsUnique = true }));


            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.HasDefaultSchema("SmartCattle");
          
            #region Cattle Configuration 
            //Use TPC Mapping
            modelBuilder.Entity<Cattle>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattleTbl");
            });

            modelBuilder.Entity<Cattle>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

             
            modelBuilder.Entity<Cattle>()
               .HasMany(c => c.ActivityState)
               .WithRequired(c => c.Cattle)
               .HasForeignKey(c => c.cattleId).WillCascadeOnDelete(true); ;
             

            modelBuilder.Entity<Cattle>()
               .HasMany(c => c.Events)
               .WithRequired(c => c.Cattle)
               .HasForeignKey(c => c.cattleId);
             

            modelBuilder.Entity<Cattle>()
               .HasMany(c => c.FertilityStates)
               .WithRequired(c => c.Cattle)
               .HasForeignKey(c => c.cattleId);


            modelBuilder.Entity<Cattle>()
               .HasOptional(c => c.CattleGroup)
               .WithMany(c => c.Cattles);

            modelBuilder.Entity<Cattle>()
                .HasOptional(c => c.CattleHerd)
                .WithMany(c => c.Cattles);

            modelBuilder.Entity<Cattle>()
                .HasMany(c => c.HealthStates)
                .WithRequired(c => c.Cattle)
                .HasForeignKey(c => c.cattleId);


            modelBuilder.Entity<Cattle>()
               .HasMany(c => c.Milkings)
               .WithRequired(c => c.Cattle)
               .HasForeignKey(c => c.cattleId);


            modelBuilder.Entity<Cattle>()
                .HasMany(c => c.Positions)
                .WithRequired(c => c.Cattle)
                .HasForeignKey(c => c.cattleId).WillCascadeOnDelete(true);
             

            modelBuilder.Entity<Cattle>()
                .HasRequired(c => c.Farm)
                .WithMany(c => c.Cattles)
                .HasForeignKey(c => c.FarmID);

            modelBuilder.Entity<Cattle>()
                .HasMany(c => c.Tempretures)
                .WithRequired(c => c.Cattle)
                .HasForeignKey(c => c.cattleId).WillCascadeOnDelete(true); ;
              
            modelBuilder.Entity<Cattle>()
                .HasMany(t => t.CattleTimeBudgets)
                .WithRequired(c => c.Cattle)
                .HasForeignKey(t => t.CattleId);

            #endregion        
            #region Activity State Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleActivityState>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("ActivityStateTbl");
            });
            modelBuilder.Entity<CattleActivityState>().HasRequired(c => c.Farm)
              .WithMany(c => c.ActivityState).HasForeignKey(c => c.FarmID)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<CattleActivityState>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CattleActivityState>().HasKey(a => a.ID);
            #endregion                   
            #region CattleEvent Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleEvent>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattleEventTbl");
            });
            modelBuilder.Entity<CattleEvent>().HasRequired(c => c.Farm)
           .WithMany(c => c.Events).HasForeignKey(c => c.FarmID)
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<CattleEvent>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CattleEvent>().HasKey(c => c.ID);
            #endregion
            #region CattleGroup Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleGroup>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattleGroupTbl");
            });
            modelBuilder.Entity<CattleGroup>().HasRequired(c => c.Farm)
                .WithMany(c => c.Groups).HasForeignKey(c => c.FarmID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CattleGroup>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CattleGroup>().HasKey(c => c.ID);
            #endregion
            #region Cattle FrtilityState Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleFrtilityState>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattleFrtilityStateTbl");
            });
            modelBuilder.Entity<CattleFrtilityState>().HasRequired(c => c.Farm)
         .WithMany(c => c.FertilityStates).HasForeignKey(c => c.FarmID)
         .WillCascadeOnDelete(false);

            modelBuilder.Entity<CattleFrtilityState>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CattleFrtilityState>().HasKey(c => c.ID);
            #endregion
            #region Cattle HealthState Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleHealthState>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattleHealthStateTbl");
            });
            modelBuilder.Entity<CattleHealthState>().HasRequired(c => c.Farm)
       .WithMany(c => c.HealthStates).HasForeignKey(c => c.FarmID)
       .WillCascadeOnDelete(false);

            modelBuilder.Entity<CattleHealthState>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CattleHealthState>().HasKey(c => c.ID);
            #endregion
            #region CattleHeatState Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleHeatState>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattleHeatStateTbl");
            });
            modelBuilder.Entity<CattleHeatState>().HasRequired(c => c.Farm)
                  .WithMany(c => c.HeatStates).HasForeignKey(c => c.FarmID)
                  .WillCascadeOnDelete(false);
          

            modelBuilder.Entity<CattleHeatState>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CattleHeatState>().HasKey(c => c.ID);
            #endregion            
            #region HerdEvent Configuration
            //Use TPC Mapping
            modelBuilder.Entity<HerdEvent>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("HerdEventTbl");
            });
            modelBuilder.Entity<HerdEvent>().HasRequired(c => c.Farm)
     .WithMany(c => c.HerdEvents).HasForeignKey(c => c.FarmID)
     .WillCascadeOnDelete(false);

            modelBuilder.Entity<HerdEvent>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<HerdEvent>().HasKey(c => c.ID).
                HasRequired(g => g.Herd)
                .WithMany(g => g.Events)
                .HasForeignKey(g => g.herdId);
            #endregion
            #region Milking Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleMilking>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("MilkingTbl");
            });

            modelBuilder.Entity<CattleMilking>().HasRequired(c => c.Farm)
    .WithMany(c => c.Milkings).HasForeignKey(c => c.FarmID)
    .WillCascadeOnDelete(false);

            modelBuilder.Entity<CattleMilking>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CattleMilking>().HasKey(c => c.ID);
            #endregion
            #region Cattle Position Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattlePosition>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattlePositionTbl");
            });
            modelBuilder.Entity<CattlePosition>().HasRequired(c => c.Farm)
  .WithMany(c => c.Positions).HasForeignKey(c => c.FarmID)
  .WillCascadeOnDelete(false);

            modelBuilder.Entity<CattlePosition>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CattlePosition>().HasKey(c => c.ID).HasRequired(c=>c.Cattle).WithMany(p=>p.Positions);
            #endregion
            #region Cattle TimeBudget Configuration
            //Use TPC Mapping
            modelBuilder.Entity<TimeBudget>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("TimeBudgetTbl");
            });
            modelBuilder.Entity<TimeBudget>().HasKey(p => p.ID);
            modelBuilder.Entity<TimeBudget>().Property(p => p.ID)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity); 

            #endregion
            #region Cattle group TimeBudget Configuration
            //Use TPC Mapping
            modelBuilder.Entity<GroupTimeBudget>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("GroupTimeBudgetTbl");
            });
            modelBuilder.Entity<GroupTimeBudget>().HasKey(g => g.ID);
            modelBuilder.Entity<GroupTimeBudget>().Property(p => p.ID)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<GroupTimeBudget>().HasRequired(c => c.Farm)
                 .WithMany(c => c.GroupsTimeBudgets).HasForeignKey(c => c.FarmID)
                 .WillCascadeOnDelete(false);
            modelBuilder.Entity<GroupTimeBudget>().HasRequired(g => g.CattleGroup);
            modelBuilder.Entity<GroupTimeBudget>().HasMany(g => g.Items).WithRequired(g => g.TimeBudget).HasForeignKey(f => f.TimeBudgetId);
            #endregion
            #region GroupTimeBudgetItem Configuration
            //Use TPC Mapping
            modelBuilder.Entity<GroupTimeBudgetItem>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("GroupTimeBudgetItemTbl");
            });
            modelBuilder.Entity<GroupTimeBudgetItem>().HasRequired(t => t.TimeBudget).WithMany(t => t.Items);
            modelBuilder.Entity<GroupTimeBudgetItem>().HasKey(k => k.ID);
            modelBuilder.Entity<GroupTimeBudgetItem>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity); 
            #endregion
            #region Farm Configuration
            //Use TPC Mapping
            modelBuilder.Entity<Farm>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("FarmTbl");
            });
            modelBuilder.Entity<Farm>().HasMany(f => f.ActivityState).WithRequired(c => c.Farm).HasForeignKey(f => f.FarmID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Farm>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);


            #endregion
            #region Sensor Configuration
            //Use TPC Mapping
            modelBuilder.Entity<Sensor>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("SensorTbl");
            });

            modelBuilder.Entity<Sensor>().HasRequired(c => c.Farm)
                .WithMany(c => c.Sensors).HasForeignKey(c => c.FarmID)
                .WillCascadeOnDelete(false);

           
            modelBuilder.Entity<Sensor>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
             

            modelBuilder.Entity<Sensor>().HasKey(c => c.ID);
            #endregion
            #region User Setting Configuration
            //Use TPC Mapping
            modelBuilder.Entity<UserSetting>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("UserSettingTbl");
            }); 

            modelBuilder.Entity<UserSetting>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<UserSetting>().HasKey(c => c.ID);
            #endregion
            #region FarmSetting Setting Configuration
            //Use TPC Mapping
            modelBuilder.Entity<FarmSetting>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("FarmSettingTbl");
            });
            modelBuilder.Entity<FarmSetting>().HasRequired(c => c.Farm)
                .WithMany(c => c.FarmSetting).HasForeignKey(c => c.FarmID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FarmSetting>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<FarmSetting>().HasKey(c => c.ID);
            #endregion
            #region Application Setting Configuration
            //Use TPC Mapping
            modelBuilder.Entity<ApplicationSetting>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("ApplicationSettingTbl");
            }); 

            modelBuilder.Entity<ApplicationSetting>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<ApplicationSetting>().HasKey(c => c.ID);
            #endregion
            #region Tempreture Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleTempreture>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("TempretureTbl");
            });

            modelBuilder.Entity<CattleTempreture>().HasRequired(c => c.Farm)
.WithMany(c => c.Tempretures).HasForeignKey(c => c.FarmID)
.WillCascadeOnDelete(false);

            modelBuilder.Entity<CattleTempreture>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);



            modelBuilder.Entity<CattleTempreture>().HasKey(c => c.ID);
            #endregion 
            #region CattleScore Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleScore>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattleScoreTbl");
            });

            modelBuilder.Entity<CattleScore>().HasRequired(c => c.Cattle)
                    .WithMany(c => c.CattleScores).HasForeignKey(c => c.CattleId);

            modelBuilder.Entity<CattleScore>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
             
            modelBuilder.Entity<CattleScore>().HasKey(c => c.ID);
            #endregion

            #region CattleTHI Configuration
            //Use TPC Mapping
            modelBuilder.Entity<CattleTHI>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("CattleTHITbl");
            });
             
            modelBuilder.Entity<CattleTHI>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CattleTHI>().HasKey(c => c.ID);
            #endregion

            #region Notification Configuration
            modelBuilder.Entity<Notification>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("NotificationTbl");
            }); 
            modelBuilder.Entity<Notification>().Property(p => p.ID)
               .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Notification>().HasKey(n => n.ID);
            #endregion
            #region USerNotification Configuration
            modelBuilder.Entity<UserNotification>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("UserNotificationTbl");
            });
         
           modelBuilder.Entity<UserNotification>().Property(p => p.ID)
               .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<UserNotification>().HasKey(n => n.ID);
            #endregion            
            #region USerNotification Configuration
            modelBuilder.Entity<RoleNotification>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("RoleNotificationTbl");
            });
        
            modelBuilder.Entity<RoleNotification>().Property(p => p.ID)
            .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<RoleNotification>().HasKey(n => n.ID);
            #endregion
            #region Rolepermission Configuration
            modelBuilder.Entity<RolePermission>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("RolePermissionTbl");
            });
            modelBuilder.Entity<RolePermission>().Property(p => p.ID)
                  .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<RolePermission>().HasKey(n => n.ID);
            modelBuilder.Entity<RolePermission>().HasMany(r => r.UserRoles).WithMany(r => r.RolePermissions);
            #endregion
             
            #region Farm Place Configuration
            modelBuilder.Entity<FarmPlaces>().Map(m => {
                m.MapInheritedProperties();
                m.ToTable("FarmPlacesTbl");
            });

            modelBuilder.Entity<FarmPlaces>().Property(p => p.ID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<FarmPlaces>().HasKey(c => c.ID);
            #endregion

        }
        public static SmartCattleContext Create()
        {
            return new SmartCattleContext();
        }

        DbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public int saveChanges()
        {
            return base.SaveChanges();
        }

        public new DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            return  base.Entry<TEntity>(entity);
        }
    }
}
