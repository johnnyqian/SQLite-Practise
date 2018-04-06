using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;

namespace SQLiteWithEFMigrations
{
    class Program
    {
        static void Main()
        {
            using (var db = new MyDbContext())
            {
                db.Notes.Add(new Note { Text = "Hello, world" });
                db.SaveChanges();
            }

            using (var db = new MyDbContext())
            {
                foreach (var note in db.Notes)
                {
                    Console.WriteLine("Note {0} = {1}", note.NoteId, note.Text);
                }
            }
        }
    }

    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("name=SQLiteContext") { }

        static MyDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyDbContext, ContextMigrationConfiguration>(true));
        }

        public DbSet<Note> Notes { get; set; }

        private sealed class ContextMigrationConfiguration : DbMigrationsConfiguration<MyDbContext>
        {
            public ContextMigrationConfiguration()
            {
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;

                SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
            }

            protected override void Seed(MyDbContext context)
            {
                context.Notes.Add(new Note { Text = "Hello, world" });
                context.Notes.Add(new Note { Text = "A second note" });

                base.Seed(context);
            }
        }
    }

    public class Note
    {
        public long NoteId { get; set; }

        public string Text { get; set; }
    }
}
