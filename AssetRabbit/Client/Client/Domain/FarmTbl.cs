using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    public class FarmTbl
    {
        public virtual int ID { get; set; }
        public virtual int SubprojectID { get; set; }
        public virtual String FarmName { get; set; }
        public virtual String Email { get; set; }
        public virtual String Latitude { get; set; }
        public virtual String Longitude { get; set; }
        public virtual String Country { get; set; }
        public virtual String City { get; set; }
        public virtual String Province { get; set; }
        public virtual String No { get; set; }
        public virtual String Street1 { get; set; }
        public virtual String Street2 { get; set; }
        public virtual String PostalCode { get; set; }
        public virtual String Phone1 { get; set; }
        public virtual String Phone2 { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual String FarmTypeUId { get; set; }
        public virtual int FarmTypeId { get; set; }
    }

    class FarmTblMapping : ClassMap<FarmTbl>
    {
        public FarmTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.SubprojectID).Nullable();
            Map(x => x.FarmName).Nullable();
            Map(x => x.Email).Nullable();
            Map(x => x.Latitude).Nullable();
            Map(x => x.Longitude).Nullable();
            Map(x => x.Country).Nullable();
            Map(x => x.City).Nullable();
            Map(x => x.Province).Nullable();
            Map(x => x.No).Nullable();
            Map(x => x.Street1).Nullable();
            Map(x => x.Street2).Nullable();
            Map(x => x.PostalCode).Nullable();
            Map(x => x.Phone1).Nullable();
            Map(x => x.Phone2).Nullable();
            Map(x => x.CreateDate).Nullable();
            Map(x => x.FarmTypeUId).Nullable();
            Map(x => x.FarmTypeId).Nullable();

            Table("SmartCattle.FarmTbl");
        }
    }
}
