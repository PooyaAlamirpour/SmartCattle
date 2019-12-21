using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency;
using TableDependency.EventArgs;
using TableDependency.SqlClient;

namespace SqlTableDependency
{
    public class Customer
    {
        public int Id { get; set; }
        public string ValueName { get; set; }
        public float Value { get; set; }
    }

    class Program
    {
        static void Main()
        {
            InitialSqlDependency();
        }

        public static void InitialSqlDependency()
        {
            String _con = Config.ConnectionString;
            var mapper = new ModelToTableMapper<Customer>();
            mapper.AddMapping(c => c.ValueName, "ValueName");
            mapper.AddMapping(c => c.Value, "Value");
            SqlTableDependency<Customer> dep = new SqlTableDependency<Customer>(_con, "SmartCattle.CurrentValue", mapper);
            dep.OnChanged += Dep_OnChanged;
            dep.Start();

            Console.WriteLine("Press a key to exit");
            Console.ReadKey();

            dep.Stop();
        }

        private static void Dep_OnChanged(object sender, RecordChangedEventArgs<Customer> e)
        {
            var changedEntity = e.Entity;
            Console.WriteLine("DML operation: " + e.ChangeType);
            Console.WriteLine("ID: " + changedEntity.Id);
            Console.WriteLine("ValueName: " + changedEntity.ValueName);
            Console.WriteLine("Value: " + changedEntity.Value);
        }
    }
}
