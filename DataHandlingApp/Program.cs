using Microsoft.Data.Sqlite;

namespace SQLite_project
{ 
    class Program
    {
        static void Main()
        { 
            using (var connection = new SqliteConnection("Data Source=/Users/Cecilie/Projects/WineAppDataFile.sqlite"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT city
                    FROM soil_data
                ";



                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var city = reader.FieldCount;

                        Console.WriteLine($"{city}");
                    }
                }
            }
        }
    }
}
