using Xunit;
using Moq;
using GameLifeApi.Controllers;
using GameLifeModels.Interface.Services;
using GameLifeModels.Interface.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GameLifeTests.Unit.Api.Controllers
{
    public class BoardsControllerTests
    {
        private readonly Mock<IBoardService> _serviceMock;
        private readonly BoardsController _controller;

        public BoardsControllerTests()
        {
            _serviceMock = new Mock<IBoardService>();
            _controller = new BoardsController(_serviceMock.Object);
        }

        [Fact]
        public void CreateBoard_ReturnsOkWithBoardId()
        {
            var request = new CreateBoardRequest { Cells = new string('0', 399) + "1" };
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CreateBoard(request.Cells)).Returns(guid);
            var result = _controller.CreateBoard(request) as OkObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.True(value.Success);
            Assert.Equal(guid, value.BoardId);
        }

        [Fact]
        public void GetNextState_ReturnsOkWithCells()
        {
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CalculateNextGeneration(guid)).Returns("cells");
            var result = _controller.GetNextState(guid) as OkObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.Equal(guid, value.BoardId);
            Assert.Equal("cells", value.Cells);
        }

        [Fact]
        public void GetNextGenerations_ReturnsOkWithCells()
        {
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CalculateNextGenerationTimes(guid, 2)).Returns("cells2");
            var result = _controller.GetNextGenerations(guid, 2) as OkObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.Equal(guid, value.BoardId);
            Assert.Equal("cells2", value.Cells);
        }

        [Fact]
        public void GetFinalState_ReturnsOkWithCells()
        {
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CalculateFinal(guid)).Returns("finalCells");
            var result = _controller.GetFinalState(guid) as OkObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.Equal(guid, value.BoardId);
            Assert.Equal("finalCells", value.Cells);
        }

        [Fact]
        public void GetCurrentState_ReturnsOkWithCells()
        {
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CurrentBoard(guid)).Returns("currentCells");
            var result = _controller.GetCurrentState(guid) as OkObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.Equal(guid, value.BoardId);
            Assert.Equal("currentCells", value.Cells);
        }

        [Fact]
        public void CreateBoard_ReturnsBadRequest_OnException()
        {
            var request = new CreateBoardRequest { Cells = new string('0', 399) + "1" };
            _serviceMock.Setup(s => s.CreateBoard(request.Cells)).Throws(new Exception("fail"));
            var result = _controller.CreateBoard(request) as BadRequestObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.False(value.Success);
            Assert.Equal("fail", value.Error);
        }

        public static IEnumerable<object[]> InvalidCellsCases => new List<object[]>
        {
            new object[] { "" },
            new object[] { "0" },
            new object[] { "1" },
            new object[] { "abcde" },
            new object[] { "0" + new string('1', 399) },
            new object[] { new string('0', 400) },
            new object[] { new string('1', 400) }
        };

        [Theory]
        [MemberData(nameof(InvalidCellsCases))]
        public void CreateBoard_ReturnsBadRequest_OnInvalidCells(string cells)
        {
            var request = new CreateBoardRequest { Cells = cells };
            _serviceMock.Setup(s => s.CreateBoard(request.Cells)).Throws(new ArgumentException("Elements from Board invalid."));
            var result = _controller.CreateBoard(request) as BadRequestObjectResult;
            Assert.NotNull(result);
            var value = result.Value;
            Assert.NotNull(value);
            var successProp = value.GetType().GetProperty("Success");
            var errorProp = value.GetType().GetProperty("Error");
            Assert.NotNull(successProp);
            Assert.NotNull(errorProp);
            var successValue = successProp.GetValue(value);
            var errorValue = errorProp.GetValue(value);
            Assert.False(successValue != null && (bool)successValue);
            Assert.Equal("Elements from Board invalid.", errorValue);
        }

        [Fact]
        public void GetNextState_ReturnsBadRequest_OnException()
        {
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CalculateNextGeneration(guid)).Throws(new Exception("fail-next"));
            var result = _controller.GetNextState(guid) as BadRequestObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.False(value.Success);
            Assert.Equal("fail-next", value.Error);
        }

        [Fact]
        public void GetNextGenerations_ReturnsBadRequest_OnException()
        {
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CalculateNextGenerationTimes(guid, 2)).Throws(new Exception("fail-next-times"));
            var result = _controller.GetNextGenerations(guid, 2) as BadRequestObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.False(value.Success);
            Assert.Equal("fail-next-times", value.Error);
        }

        [Fact]
        public void GetFinalState_ReturnsBadRequest_OnException()
        {
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CalculateFinal(guid)).Throws(new Exception("fail-final"));
            var result = _controller.GetFinalState(guid) as BadRequestObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.False(value.Success);
            Assert.Equal("fail-final", value.Error);
        }

        [Fact]
        public void GetCurrentState_ReturnsBadRequest_OnException()
        {
            var guid = Guid.NewGuid();
            _serviceMock.Setup(s => s.CurrentBoard(guid)).Throws(new Exception("fail-current"));
            var result = _controller.GetCurrentState(guid) as BadRequestObjectResult;
            Assert.NotNull(result);
            dynamic value = result.Value;
            Assert.False(value.Success);
            Assert.Equal("fail-current", value.Error);
        }
    }
}
