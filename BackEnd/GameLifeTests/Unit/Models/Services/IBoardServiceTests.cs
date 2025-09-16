using Xunit;
using GameLifeModels.Interface.Services;
using System;

namespace GameLifeTests.Unit.Services
{
    public class IBoardServiceTests
    {
        private class FakeBoardService : IBoardService
        {
            public Guid CreateBoard(string cells) => Guid.Parse("11111111-1111-1111-1111-111111111111");
            public string CurrentBoard(Guid boardId) => "current-cells";
            public string CalculateNextGeneration(Guid boardId) => "next-cells";
            public string CalculateNextGenerationTimes(Guid boardId, int times) => $"next-cells-{times}";
            public string CalculateFinal(Guid boardId) => "final-cells";
        }

        [Fact]
        public void CreateBoard_ShouldReturnBoardId()
        {
            var service = new FakeBoardService();
            var result = service.CreateBoard("00001111");
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), result);
        }

        [Fact]
        public void CurrentBoard_ShouldReturnCurrentCells()
        {
            var service = new FakeBoardService();
            var result = service.CurrentBoard(Guid.Parse("11111111-1111-1111-1111-111111111111"));
            Assert.Equal("current-cells", result);
        }

        [Fact]
        public void CalculateNextGeneration_ShouldReturnNextCells()
        {
            var service = new FakeBoardService();
            var result = service.CalculateNextGeneration(Guid.Parse("11111111-1111-1111-1111-111111111111"));
            Assert.Equal("next-cells", result);
        }

        [Fact]
        public void CalculateNextGenerationTimes_ShouldReturnNextCellsTimes()
        {
            var service = new FakeBoardService();
            var result = service.CalculateNextGenerationTimes(Guid.Parse("11111111-1111-1111-1111-111111111111"), 3);
            Assert.Equal("next-cells-3", result);
        }

        [Fact]
        public void CalculateFinal_ShouldReturnFinalCells()
        {
            var service = new FakeBoardService();
            var result = service.CalculateFinal(Guid.Parse("11111111-1111-1111-1111-111111111111"));
            Assert.Equal("final-cells", result);
        }
    }
}