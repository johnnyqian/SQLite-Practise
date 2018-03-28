using System;
using System.Data.SQLite;

namespace SQLiteBasic
{
    class Program
    {
        static void Main()
        {
            // SQLiteConnection.CreateFile("Database.sqlite");

            // If the database file doesn't exist, the default behavior is to create a new one
            using (var dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;"))
            {
                dbConnection.Open();

                var sql = "CREATE TABLE scores (name VARCHAR(20), score INT)";
                var command = new SQLiteCommand(sql, dbConnection);

                command.ExecuteNonQuery();

                sql = "insert into scores (name, score) values ('Me', 3000)";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                sql = "insert into scores (name, score) values ('Myself', 6000)";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                sql = "insert into scores (name, score) values ('And I', 9000)";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                sql = "select * from scores order by score desc";
                command = new SQLiteCommand(sql, dbConnection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
                }
            }

            Console.ReadLine();
        }
    }
}
