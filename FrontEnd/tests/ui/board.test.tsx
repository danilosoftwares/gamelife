// Board.test.tsx
import { render, screen, fireEvent } from "@testing-library/react";
import Board from "../../src/ui/board";
import * as boardHook from "../../src/ui/hooks/useBoard";
import { describe, it, beforeEach, vi, expect } from 'vitest';

vi.mock("../../src/ui/hooks/useBoard", async () => {
  const actual: object = await vi.importActual("../../src/ui/hooks/useBoard");
  return {
    ...actual,
    useBoardStore: vi.fn(),
  };
});

describe("Board component", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("renderiza o tabuleiro corretamente", () => {
    (boardHook.useBoardStore as any).mockReturnValue({
      board: [
        [0, 1],
        [1, 0],
      ],
      toggleCell: vi.fn(),
    });

    render(<Board />);

    const buttons = screen.getAllByRole("button");
    expect(buttons).toHaveLength(4);

    expect(buttons[1]).toHaveStyle("background: rgba(98, 234, 148, 1)");
    expect(buttons[0]).not.toHaveStyle("background: rgba(98, 234, 148, 1)");
  });
});