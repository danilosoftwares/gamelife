
namespace GameLifeModels.Interface.Responses
{
    public class CreateBoardResponse
    {
        public Guid BoardId { get; set; }
        public string? Error { get; set; }
        public bool Success { get; set; } = true;
    }
}