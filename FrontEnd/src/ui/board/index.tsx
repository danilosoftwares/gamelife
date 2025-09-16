import { useBoardStore } from "../hooks/useBoard";


const Board = () => {
  const { board, toggleCell } = useBoardStore();

  return (
    <div className="grid gap-1 p-6 bg-card rounded-lg border">
      <div
        style={{
          gridTemplateColumns: `repeat(${board[0]?.length || 0}, minmax(0, 1fr))`,
          gap: "4px",
          display: "grid"
        }}
      >
        {board.map((row, rowIndex) =>
          row.map((cell, colIndex) => (
            <button
              key={`${rowIndex}-${colIndex}`}
              style={cell ? { background:'rgba(98, 234, 148, 1)', boxShadow: 'rgba(98, 234, 148, 0.796) 0px 0px 19.7326px 0px' } : undefined}
              onClick={() => toggleCell(rowIndex, colIndex)}
              aria-label={`Célula ${rowIndex + 1}, ${colIndex + 1} - ${cell ? 'ativa' : 'inativa'}`}
            >{cell}</button>
          ))
        )}
      </div>
    </div>
  );
};

export default Board;