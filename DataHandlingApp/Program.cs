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
                    SELECT address, id
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

                /*foreach (var id in idList)
                {
                    Console.WriteLine(id);
                }*/

                for (int oldAddressIndex = 0; oldAddressIndex < oldAddressList.Count; oldAddressIndex++)
                {
                    Console.WriteLine(oldAddressList[oldAddressIndex]);

                    Console.WriteLine("Enter new address without city name: ");
                    newAddressList.Add(Console.ReadLine());

                    Console.WriteLine("Enter city name from current address: ");
                    cityList.Add(Console.ReadLine());

                    Console.Clear();
                }

                /*for (int i = 0; i < cityList.Count; i++)
                {
                    Console.WriteLine($"{idList[i]}. {newAddressList[i]} {cityList[i]}.");
                }*/

                for (int cityIndex = 0; cityIndex < cityList.Count; cityIndex++)
                {

                    command.CommandText =
                    $@"
                        INSERT INTO soil_data (address, city)
                        VALUES ('{newAddressList[cityIndex]}', '{cityList[cityIndex]}')
                        WHERE id = {cityIndex + 1}
                    ";
                }
            }
        }
    }
}
