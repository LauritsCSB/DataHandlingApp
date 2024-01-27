using Microsoft.Data.Sqlite;

namespace DataHandlingApp
{
    public static class DataBaseConnection
    {
        public static void OpenConnection(string filePath)
        {
            var connection = new SqliteConnection($"Data Source={filePath}");
            connection.Open();
        }
    }
}

