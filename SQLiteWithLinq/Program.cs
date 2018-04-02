// Using SQLite database in .NET with LINQ to SQL and Entity Framework 6, https://vijayt.com/post/using-sqlite-database-in-net-with-linq-to-sql-and-entity-framework-6/

using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SQLite;

namespace SQLiteWithLinq
{
    class Program
    {
        static void Main()
        {
            using (var dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;"))
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

                using (var context = new DataContext(dbConnection))
                {
                    var companies = context.GetTable<Company>();
                    foreach (var company in companies)
                    {
                        Console.WriteLine("Company: {0} {1}", company.Id, company.Seats);
                    }
                    Console.ReadKey();
                }
            }
        }
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
