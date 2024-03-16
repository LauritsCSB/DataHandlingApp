using Microsoft.Data.Sqlite;
using DataHandlingApp;
namespace SQLite_project
{ 
    class Program
    {
        static void Main()
        { 
            List<int> idList = new List<int>();
            List<string> wineyardNameList = new List<string>();
            List<string> addressList = new List<string>();
            List<string> regionList = new List<string>();
            List<string> countryList = new List<string>();

            string database = "/Users/Cecilie/Projects/soil_data_denmark.sqlite";

            if (!File.Exists(database))
            {
                Console.WriteLine("Error locating database.");
                System.Environment.Exit(1);
            }

            var connection = new SqliteConnection("Data Source=" + database);

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database Connection Unsuccessfull.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.HelpLink);
                System.Environment.Exit(1);
            }

            try
            { 
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT name
                    FROM soil_data_denmark
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var wineyardName = reader.GetString(0);

                        wineyardNameList.Add(wineyardName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading data.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.HelpLink);
                System.Environment.Exit(1);
            }

            int idNumber = 1;
            for (int listIndex = 0; listIndex < wineyardNameList.Count; listIndex++)
            {
                idList.Add(idNumber);
                idNumber++;
            }

            try
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        INSERT INTO address_data_denmark (id, wineyard_name)
                        VALUES ($id, $wineyard_name)
                    ";

                for (int listIndex = 0; listIndex < wineyardNameList.Count; listIndex++)
                {
                    using (var cmd = new SqliteCommand(command.CommandText, connection))
                    {
                        cmd.Parameters.AddWithValue("$id", idList[listIndex]);
                        cmd.Parameters.AddWithValue("$wineyard_name", wineyardNameList[listIndex]);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Data inserted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting data.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.HelpLink);
                System.Environment.Exit(1);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
