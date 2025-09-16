namespace GameLifeModels.Interface.Repository
{
    public interface IBoardRepository
    {
        Guid CreateBoard(string cells);
        string GetBoardCellsById(Guid boardId);
        Guid AddBoard(Guid id, string cells);
    }
}