using SmartCattle.Web.Areas.APIs.Models;
using System;
using System.Web.Mvc;

namespace SmartCattle.Web.Controllers
{
    public class IdentityController : Controller
    {
        String _con = "data source=79.175.133.194;User ID=hossein;Password=myzYKEGuP70V_oWb30Yr; initial catalog=SmartCattle;";

        // GET: Identity
        public ActionResult Index()
        {
            //var mapper = new ModelToTableMapper<Customer>();
            //mapper.AddMapping(c => c.Surname, "SecondName");
            //mapper.AddMapping(c => c.Name, "FirstName");

            //using (var dep = new SqlTableDependency<Customer>(_con, "IdentityTbl", mapper))
            //{
            //    dep.OnChanged += Changed;
            //    dep.Start();

            //    Console.WriteLine("Press a key to exit");

            //    dep.Stop();
            //}

            return View();
        }

        //static void Changed(object sender, RecordChangedEventArgs<Customer> e)
        //{
        //    var changedEntity = e.Entity;
        //    Console.WriteLine("DML operation: " + e.ChangeType);
        //    Console.WriteLine("ID: " + changedEntity.Id);
        //    Console.WriteLine("Name: " + changedEntity.Name);
        //    Console.WriteLine("Surame: " + changedEntity.Surname);
        //}
    }
}