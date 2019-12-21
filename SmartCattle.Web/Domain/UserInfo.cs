using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class UserInfo
    {
        public virtual int ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Family { get; set; }
        public virtual String Email { get; set; }
        public virtual String Password { get; set; }
        public virtual String RoleId { get; set; }
        public virtual String RoleName { get; set; }
        //public virtual String Permissions { get; set; }
        public virtual int FarmId { get; set; }
        public virtual String FarmIdList { get; set; }
        public virtual DateTime CreateDate { get; set; }
    }

    class UserInfoMapping : ClassMap<UserInfo>
    {
        public UserInfoMapping()
        {
            Id(x => x.ID);
            Map(x => x.Name).Nullable();
            Map(x => x.Family).Nullable();
            Map(x => x.Email).Nullable();
            Map(x => x.Password).Nullable();
            Map(x => x.RoleId).Nullable();
            //Map(x => x.Permissions).Nullable();
            Map(x => x.FarmId).Nullable();
            Map(x => x.CreateDate).Nullable();
            Map(x => x.FarmIdList).Nullable();
            Map(x => x.RoleName).Nullable();
            
            Table("SmartCattle.UserInfo");
        }
    }

}