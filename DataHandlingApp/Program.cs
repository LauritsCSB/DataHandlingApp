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

            using (var connection = new SqliteConnection("Data Source=/Users/Cecilie/Projects/soil_data_denmark.sqlite"))
            {
                connection.Open();

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

                int idNumber = 1;

                for (int nameIndex = 0; nameIndex < wineyardNameList.Count; nameIndex++)
                {
                    idList.Add(idNumber);
                    idNumber++;
                }

                /*for (int nameIndex = 0; nameIndex < wineyardNameList.Count; nameIndex++)
                {
                    Console.WriteLine($"ID: {idList[nameIndex]}, Name: {wineyardNameList[nameIndex]}");
                }*/

                //Change list before updating new row(s)
                List<string> defaultList = wineyardNameList;
                for (int listIndex = 0; listIndex < defaultList.Count; listIndex++)
                {

                    command.CommandText =
                        @"
                            INSERT INTO address_data_denmark (id, wineyard_name)
                            VALUES ($id, $wineyard_name)
                        ";

                    command.Parameters.AddWithValue("$id", idList[listIndex]);
                    command.Parameters.AddWithValue("$wineyard_name", wineyardNameList[listIndex]);

                    command.ExecuteNonQuery();
                };


                connection.Close();
            }
        }
    }
}
