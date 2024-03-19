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
                    SELECT region, country
                    FROM soil_data_denmark
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var regionName = reader.GetString(0);
                        var countryName = reader.GetString(1);
                        //what is int ordinal used in these methods?

                        regionList.Add(regionName);
                        countryList.Add(countryName);
                    }
                }

                command.CommandText =
                    @"
                        SELECT id
                        FROM address_data_denmark
                    ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);

                        idList.Add(id);
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

            try
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE address_data_denmark
                        SET region = $region, country = $country
                        WHERE id = $id
                    ";

                for (int listIndex = 0; listIndex < regionList.Count; listIndex++)
                {
                    using (var cmd = new SqliteCommand(command.CommandText, connection))
                    {
                        cmd.Parameters.AddWithValue("$region", regionList[listIndex]);
                        cmd.Parameters.AddWithValue("$country", countryList[listIndex]);
                        cmd.Parameters.AddWithValue("$id", idList[listIndex]);
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
