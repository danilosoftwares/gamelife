using Xunit;
using GameLifeModels.Interface.Requests;

namespace GameLifeTests.Unit.Models
{
    public class CreateBoardRequestTests
    {
        [Fact]
        public void Cells_ShouldBeEmptyString_ByDefault()
        {
            var request = new CreateBoardRequest();
            Assert.Equal("", request.Cells);
        }

        [Fact]
        public void Cells_ShouldSetValue()
        {
            var request = new CreateBoardRequest { Cells = "101010" };
            Assert.Equal("101010", request.Cells);
        }
    }
}
