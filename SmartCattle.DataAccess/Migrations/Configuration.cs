namespace SmartCattle.DataAccess.Migrations
{
    using DomainClass;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Core;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SmartCattle.DataAccess.SmartCattleContext>
    {
        private readonly bool _pendingMigrations;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(SmartCattle.DataAccess.SmartCattleContext context)
        {
            SeedUser(context);
        }

        private void SeedUser(SmartCattleContext context)
        {
            //if (!context.Roles.Any(r => r.Name == "SuperAdmin"))
            //{
            //    var store = new RoleStore<UserRole>(context);
            //    var manager = new RoleManager<UserRole>(store);
            //    var role = new UserRole { Name = "SuperAdmin" };
            //    manager.Create(role);
            //}

            //if (!context.Farms.Any(f => f.name == "BaseFarm"))
            //{
            //    var farm = new Farm() { name = "BaseFarm" }; 
            //    context.Farms.Add(farm); 
            //    context.saveChanges();
            //}

            //if (!context.Users.Any(u => u.UserName == Setting.AdminEmail ))
            //{
            //    var store = new UserStore<SmartCattleUser>(context);
            //    var manager = new UserManager<SmartCattleUser>(store);
            //    var farm = context.Farms.FirstOrDefault(f => f.name == "BaseFarm");
            //    var user = new SmartCattleUser { UserName = Setting.AdminEmail, FarmID=farm.ID , Email= Setting.AdminEmail };
            //    manager.Create(user, "ChangeItAsap!");
            //    manager.SendEmail(user.Id, "temporary password !", "Your temporary password is : ChangeItAsap! reset password after first login");
            //    manager.AddToRole(user.Id, "SuperAdmin");
            //}
        }
    }
}
