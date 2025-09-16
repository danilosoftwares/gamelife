using Xunit;
using GameLifeRepository;
using GameLifeServices;
using System;
using System.Collections.Generic;

namespace GameLifeTests.Unit.Services
{
    public class BoardServiceTests
    {
        private static readonly string DbPath = "test_service.db";
        private static readonly SqliteConnectionFactory factory = new SqliteConnectionFactory(DbPath);
        private static readonly BoardRepository repository;
        private static readonly BoardService service;
        static BoardServiceTests()
        {
            DbInitializer.EnsureCreated(factory);
            repository = new BoardRepository(factory);
            service = new BoardService(repository);
        }

        [Fact]
        public void CreateBoard_ShouldReturnValidId()
        {
            var cells = new string('0', 199) + new string('1', 201);
            var id = service.CreateBoard(cells);
            Assert.NotEqual(Guid.Empty, id);
        }

        public static IEnumerable<object[]> InvalidCellsCases => new List<object[]>
        {
            new object[] { "" },
            new object[] { "0" },
            new object[] { "1" },
            new object[] { "abcde" },
            new object[] { new string('0', 400) },
            new object[] { new string('1', 400) }
        };

        [Theory]
        [MemberData(nameof(InvalidCellsCases))]
        public void CreateBoard_ShouldThrowArgumentException_OnInvalidCells(string cells)
        {
            Assert.Throws<ArgumentException>(() => service.CreateBoard(cells));
        }

        [Fact]
        public void CurrentBoard_ShouldReturnCells()
        {
            var cells = new string('0', 199) + new string('1', 201);
            var id = service.CreateBoard(cells);
            var result = service.CurrentBoard(id);
            Assert.Equal(cells, result);
        }

        [Fact]
        public void CurrentBoard_ShouldThrowArgumentException_OnInvalidId()
        {
            var ex = Assert.Throws<ArgumentException>(() => service.CurrentBoard(Guid.NewGuid()));
            Assert.Contains("Board invalid or not found.", ex.Message);
        }

        [Fact]
        public void CalculateNextGeneration_ShouldReturnDifferentCells()
        {
            var cells = new string('0', 199) + new string('1', 201);
            var id = service.CreateBoard(cells);
            var next = service.CalculateNextGeneration(id);
            Assert.NotNull(next);
            Assert.NotEqual(cells, next);
        }

        [Fact]
        public void CalculateNextGeneration_ShouldThrowArgumentException_OnInvalidId()
        {
            var ex = Assert.Throws<ArgumentException>(() => service.CalculateNextGeneration(Guid.NewGuid()));
            Assert.Contains("Board invalid or not found.", ex.Message);
        }

        [Fact]
        public void CalculateNextGenerationTimes_ShouldReturnDifferentCells()
        {
            var cells = new string('0', 199) + new string('1', 201);
            var id = service.CreateBoard(cells);
            var next = service.CalculateNextGenerationTimes(id, 3);
            Assert.NotNull(next);
        }

        [Fact]
        public void CalculateNextGenerationTimes_ShouldThrowArgumentException_OnInvalidId()
        {
            var ex = Assert.Throws<ArgumentException>(() => service.CalculateNextGenerationTimes(Guid.NewGuid(), 2));
            Assert.Contains("Board invalid or not found.", ex.Message);
        }

        [Fact]
        public void CalculateFinal_ShouldReturnCells()
        {
            var cells = new string('0', 199) + new string('1', 201);
            var id = service.CreateBoard(cells);
            var final = service.CalculateFinal(id);
            Assert.NotNull(final);
        }

        [Fact]
        public void CalculateFinal_ShouldThrowArgumentException_OnInvalidId()
        {
            var ex = Assert.Throws<ArgumentException>(() => service.CalculateFinal(Guid.NewGuid()));
            Assert.Contains("Board invalid or not found.", ex.Message);
        }
    }
}