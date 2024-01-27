using Microsoft.Data.Sqlite;
using DataHandlingApp;
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
                    SELECT address
                    FROM soil_data
                    WHERE region = 'Nordjylland'
                ";

                List<string> oldAddressList = new List<string>();
                List<string> newAddressList = new List<string>();
                List<string> cityList = new List<string>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var address = reader.GetString(0);

                        oldAddressList.Add(address);
                    }

                }

                for (int index = 0; index < oldAddressList.Count; index++)
                {
                    Console.WriteLine(oldAddressList[index]);

                    Console.WriteLine("Enter new address without city name: ");
                    newAddressList.Add(Console.ReadLine());

                    Console.WriteLine("Enter city name from current address: ");
                    cityList.Add(Console.ReadLine());

                    Console.Clear();
                }

                /*foreach (var item in newAddressList)
                {
                    Console.WriteLine(item);
                }

                
                foreach (var item in cityList)
                {
                    Console.WriteLine(item);
                }*/

                for (int cityIndex = 0; cityIndex <= cityList.Count; cityIndex++)
                {

                    command.CommandText =
                    $@"
                        UPDATE soil_data
                        SET city = {cityList[cityIndex]}
                        WHERE id = {cityIndex + 1}
                    
                    ";
                }
            }
        }
    }
}
