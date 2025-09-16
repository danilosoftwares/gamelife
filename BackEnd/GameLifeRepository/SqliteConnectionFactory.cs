using Microsoft.Data.Sqlite;

namespace GameLifeRepository
{
    public class SqliteConnectionFactory
    {
        private readonly string _connectionString;

        public SqliteConnectionFactory(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
        }

        public SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);            
            connection.Open();
            return connection;
        }
    }
}
