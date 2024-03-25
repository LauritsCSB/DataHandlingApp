using Microsoft.Data.Sqlite;
using DataHandlingApp;
namespace SQLite_project
{ 
    class Program
    {
        static void Main()
        { 
            List<int> idList = new List<int>();
            List<string> addressList = new List<string>();
            List<string> streetAddressList = new List<string>();
            List<string> houseNumberList = new List<string>();
            List<string> cityList = new List<string>();
            List<int> postalCodeList = new List<int>();

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
                    SELECT address
                    FROM soil_data_denmark
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var address = reader.GetString(0);
                        //what is int ordinal used in these methods?

                        addressList.Add(address);
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

            for (int listIndex = 0; listIndex < addressList.Count; listIndex++)
            {
                Console.WriteLine(addressList[listIndex]);
                Console.WriteLine("Street address:");
                streetAddressList.Add(Console.ReadLine());

                Console.WriteLine("House number:");
                houseNumberList.Add(Console.ReadLine());

                Console.WriteLine("Ciy name:");
                cityList.Add(Console.ReadLine());

                Console.WriteLine("Postal code:");
                postalCodeList.Add(Convert.ToInt32(Console.ReadLine()));

                Console.Clear();
            }

            try
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE address_data_denmark
                        SET street_name = $street_name,
                            house_number = $house_number,
                            city = $city,
                            postal_code = $postal_code
                        WHERE id = $id
                    ";

                for (int listIndex = 0; listIndex < idList.Count; listIndex++)
                {
                    using (var cmd = new SqliteCommand(command.CommandText, connection))
                    {
                        cmd.Parameters.AddWithValue("$street_name", streetAddressList[listIndex]);
                        cmd.Parameters.AddWithValue("$house_number", houseNumberList[listIndex]);
                        cmd.Parameters.AddWithValue("$city", cityList[listIndex]);
                        cmd.Parameters.AddWithValue("$postal_code", postalCodeList[listIndex]);
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
