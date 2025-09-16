
namespace GameLifeModels.Interface.Responses
{
    public class BoardWithCellsResponse
    {
        public Guid BoardId { get; set; }
        public string? Error { get; set; }
        public string Cells { get; set; } = "";
        public bool Success { get; set; } = true;
    }
}