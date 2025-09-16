using Xunit;
using GameLifeRepository;
using System;

namespace GameLifeTests.Unit.Repository
{
    public class BoardRepositoryTests
    {
        private static readonly string DbPath = "test.db";
        private static readonly GameLifeRepository.SqliteConnectionFactory factory = new GameLifeRepository.SqliteConnectionFactory(DbPath);
        static BoardRepositoryTests()
        {
            DbInitializer.EnsureCreated(factory);
        }

        [Fact]
        public void CreateBoard_ShouldReturnValidId()
        {
            var repo = new GameLifeRepository.BoardRepository(factory);
            var cells = new string('0', 199) + new string('1', 201);
            var id = repo.CreateBoard(cells);
            Assert.NotEqual(Guid.Empty, id);
        }

        [Fact]
        public void AddBoard_ShouldCacheCells()
        {
            var repo = new GameLifeRepository.BoardRepository(factory);
            var id = Guid.NewGuid();
            var cells = new string('1', 200) + new string('0', 200);
            repo.AddBoard(id, cells);
            var cached = repo.GetBoardCellsById(id);
            Assert.Equal(cells, cached);
        }

        [Fact]
        public void GetBoardCellsById_ShouldReturnEmptyIfNotFound()
        {
            var repo = new GameLifeRepository.BoardRepository(factory);
            var result = repo.GetBoardCellsById(Guid.NewGuid());
            Assert.Equal(string.Empty, result);
        }
    }
}