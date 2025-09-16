namespace GameLifeModels.Interface.Services
{
    public interface IBoardService
    {
        Guid CreateBoard(string cells);
        string CurrentBoard(Guid boardId);
        string CalculateNextGeneration(Guid boardId);
        string CalculateNextGenerationTimes(Guid boardId, int times);
        string CalculateFinal(Guid boardId);
    }
}