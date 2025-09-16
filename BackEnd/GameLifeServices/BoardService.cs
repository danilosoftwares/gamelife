using GameLifeModels.Interface.Repository;
using GameLifeModels.Interface.Services;

namespace GameLifeServices
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _repository;

        public BoardService(IBoardRepository repository)
        {
            _repository = repository;
        }

        public Guid CreateBoard(string cells)
        {
            if (string.IsNullOrEmpty(cells) || cells.Length != 400)
                throw new ArgumentException("Elements from Board invalid.");

            if (cells.IndexOf('0') == -1 || cells.IndexOf('1') == -1)
                throw new ArgumentException("Elements from Board invalid.");
            if (cells.IndexOfAny(new char[] { '0', '1' }) == -1 || cells.Any(c => c != '0' && c != '1'))
                throw new ArgumentException("Elements from Board invalid.");

            var id = _repository.CreateBoard(cells);
            return id;
        }

        public string CurrentBoard(Guid boardId)
        {
            string boardString = _repository.GetBoardCellsById(boardId);
            if (string.IsNullOrEmpty(boardString) || boardString.Length != 400)
                throw new ArgumentException("Board invalid or not found.");
            return boardString;
        }        

        public string CalculateFinal(Guid boardId)
        {
            string boardString = _repository.GetBoardCellsById(boardId);
            if (string.IsNullOrEmpty(boardString) || boardString.Length != 400)
                throw new ArgumentException("Board invalid or not found.");

            var lastBoards = new Queue<string>(5);
            string previous;
            do
            {
                previous = boardString;
                boardString = Gaming(boardString);
                _repository.AddBoard(boardId, boardString);

                if (lastBoards.Count == 5)
                    lastBoards.Dequeue();
                lastBoards.Enqueue(boardString);

                if (lastBoards.Count > 1 && lastBoards.Count(s => s == boardString) > 1)
                    break;

            } while (previous != boardString);
            return boardString;
        }

        public string CalculateNextGenerationTimes(Guid boardId, int times)
        {
            string boardString = _repository.GetBoardCellsById(boardId);
            if (string.IsNullOrEmpty(boardString) || boardString.Length != 400)
                throw new ArgumentException("Board invalid or not found.");

            for (int i = 0; i < times; i++)
            {
                boardString = Gaming(boardString);
                _repository.AddBoard(boardId, boardString);
            }
            return boardString;
        }

        public string CalculateNextGeneration(Guid boardId)
        {
            string boardString = _repository.GetBoardCellsById(boardId);
            if (string.IsNullOrEmpty(boardString) || boardString.Length != 400)
                throw new ArgumentException("Board invalid or not found.");

            var result = Gaming(boardString);
            _repository.AddBoard(boardId, result);
            return result;
        }

        private string Gaming(string boardString)
        {
            int size = 20;
            Span<char> grid = stackalloc char[400];
            boardString.AsSpan().CopyTo(grid);
            Span<char> nextGrid = stackalloc char[400];
            
            for (int i = 0; i < 400; i++)
            {
                nextGrid[i] = GetNeighbors(i, size, grid);
            }
            return new string(nextGrid);
        }

        private static char GetNeighbors(int pos, int width, ReadOnlySpan<char> board)
        {
            int current = board[pos] - '0'; 
            int aliveCount = 0;
            int up = pos - width;
            int down = pos + width;
            bool isFirstColumn = pos % width == 0;
            bool isLastColumn = (pos + 1) % width == 0;

            if (up >= 0 && board[up] == '1') aliveCount++;
            if (down < board.Length && board[down] == '1') aliveCount++;
            if (!isFirstColumn && board[pos - 1] == '1') aliveCount++;
            if (!isLastColumn && board[pos + 1] == '1') aliveCount++;
            if (up >= 0 && !isFirstColumn && board[up - 1] == '1') aliveCount++;
            if (up >= 0 && !isLastColumn && board[up + 1] == '1') aliveCount++;
            if (down < board.Length && !isFirstColumn && board[down - 1] == '1') aliveCount++;
            if (down < board.Length && !isLastColumn && board[down + 1] == '1') aliveCount++;

            return (current == 1 && (aliveCount == 2 || aliveCount == 3)) || (current == 0 && aliveCount == 3) ? '1' : '0';
        }
    }
}
