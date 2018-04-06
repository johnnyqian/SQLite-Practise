// Getting started with SQLite and Entity Framework, http://blog.rniemand.com/getting-started-with-sqlite-and-entity-framework/
// SQLiteCodeFirst, https://github.com/msallin/SQLiteCodeFirst

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SQLite.CodeFirst;

namespace SQLiteWithEF
{
    class Program
    {
        static void Main()
        {
            using (var context = new MyDbContext())
            {
                var companies = context.Companies;
                foreach (var company in companies)
                {
                    Console.WriteLine("Company: {0} {1}", company.Id, company.Seats);
                }

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

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("name=SQLiteContext")
        {
        }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MyDbInitializer(modelBuilder));

            base.OnModelCreating(modelBuilder);
        }
    }

    public class MyDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<MyDbContext>
    {
        public MyDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        protected override void Seed(MyDbContext context)
        {
            IList<Company> companies = new List<Company>();

            companies.Add(new Company { Id = 1, Seats = 20 });
            companies.Add(new Company { Id = 2, Seats = 30 });
            companies.Add(new Company { Id = 3, Seats = 50 });

            context.Companies.AddRange(companies);

            base.Seed(context);
        }
    }

    [Table("Company")]
    public class Company
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int Seats { get; set; }
    }
}
