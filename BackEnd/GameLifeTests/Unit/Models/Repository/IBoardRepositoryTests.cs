using Xunit;
using GameLifeModels.Interface.Repository;
using Moq;

namespace GameLifeTests.Unit.Repository
{
    public class IBoardRepositoryTests
    {
        [Fact]
        public void CreateBoard_ShouldReturnId()
        {
            var fakeId = Guid.NewGuid();
            var fakeCells = new string('0', 399) + "1";

            var mockRepo = new Mock<IBoardRepository>();
            mockRepo.Setup(r => r.CreateBoard(fakeCells)).Returns(fakeId);

            var result = mockRepo.Object.CreateBoard(fakeCells);
            Assert.Equal(fakeId, result);
        }

        [Fact]
        public void GetBoardCellsById_ShouldReturnCells()
        {
            var fakeId = Guid.NewGuid();
            var fakeCells = new string('1', 399) + "0";

            var mockRepo = new Mock<IBoardRepository>();
            mockRepo.Setup(r => r.GetBoardCellsById(fakeId)).Returns(fakeCells);

            var result = mockRepo.Object.GetBoardCellsById(fakeId);
            Assert.Equal(fakeCells, result);
        }

        [Fact]
        public void AddBoard_ShouldReturnId()
        {
            var fakeId = Guid.NewGuid();
            var fakeCells = new string('0', 399) + "1";

            var mockRepo = new Mock<IBoardRepository>();
            mockRepo.Setup(r => r.AddBoard(fakeId, fakeCells)).Returns(fakeId);

            var result = mockRepo.Object.AddBoard(fakeId, fakeCells);
            Assert.Equal(fakeId, result);
        }
    }
}
