using GameLifeModels.Interface.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace GameLifeRepository
{
    public class BoardRepository : IBoardRepository
    {
        private readonly SqliteConnectionFactory _factory;
        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public BoardRepository(SqliteConnectionFactory factory)
        {
            _factory = factory;
        }

        public Guid CreateBoard(string cells)
        {
            var id = Guid.NewGuid();
            AddBoard(id, cells);
            return id;
        }

        public Guid AddBoard(Guid id, string cells)
        {
            using var conn = _factory.CreateConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Boards (Id, Cells) VALUES ($id, $cells)";
            cmd.Parameters.AddWithValue("$id", id.ToString());
            cmd.Parameters.AddWithValue("$cells", cells);
            cmd.ExecuteNonQuery();
            _cache.Set(id.ToString(), cells);
            return id;
        }

        public string GetBoardCellsById(Guid boardId)
        {
            if (_cache.TryGetValue(boardId.ToString(), out string? cachedCells) && !string.IsNullOrEmpty(cachedCells))
                return cachedCells;

            using var conn = _factory.CreateConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Cells FROM Boards WHERE Id = $id ORDER BY Seq DESC LIMIT 1";
            cmd.Parameters.AddWithValue("$id", boardId.ToString());
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var cells = reader.GetString(0);
                _cache.Set(boardId.ToString(), cells);
                return cells;
            }
            return string.Empty;
        }
    }

    public static class DbInitializer
    {
        public static void EnsureCreated(SqliteConnectionFactory factory)
        {
            using var conn = factory.CreateConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Boards (
                    Seq INTEGER PRIMARY KEY AUTOINCREMENT,
                    Id TEXT NOT NULL,
                    Cells TEXT NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }
    }
}
