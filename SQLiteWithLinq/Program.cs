// Using SQLite database in .NET with LINQ to SQL and Entity Framework 6, https://vijayt.com/post/using-sqlite-database-in-net-with-linq-to-sql-and-entity-framework-6/
// SQLite LINQ Provider Data Context, https://stackoverflow.com/questions/22561248/sqlite-linq-provider-data-context

using System;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SQLite;
using System.Linq;

namespace SQLiteWithLinq
{
    class Program
    {
        static void Main()
        {
            using (var dbConnection = new SQLiteConnection(ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString))
            {
                dbConnection.Open();

                var sql = "CREATE TABLE IF NOT EXISTS company (id INT, seats INT)";
                var command = new SQLiteCommand(sql, dbConnection);

                command.ExecuteNonQuery();

                sql = "insert into company (id, seats) values (1, 20)";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                sql = "insert into company (id, seats) values (2, 30)";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                sql = "insert into company (id, seats) values (3, 50)";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                using (var context = new MyDataContext(dbConnection))
                {
                    var companies = context.Companies;
                    foreach (var company in companies)
                    {
                        Console.WriteLine("Company: {0} {1}", company.Id, company.Seats);
                    }

                    // this will generate sql statement that not compatible with sqlite, need to resolve later
                    var query = companies.Where(c => c.Id > 1).OrderBy(c => c.Id).Skip(1).Take(1);
                    Console.WriteLine(query.ToString());
                    foreach (var company in query)
                    {
                        Console.WriteLine("Company: {0} {1}", company.Id, company.Seats);
                    }

                    Console.ReadKey();
                }
            }
        }
    }

    public class MyDataContext : DataContext
    {
        public MyDataContext(IDbConnection connection)
            : base(connection)
        {
        }

        public Table<Company> Companies => GetTable<Company>();
    }

    [Table(Name = "company")]
    public class Company
    {
        [Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "seats")]
        public int Seats { get; set; }
    }
}
