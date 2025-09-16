import { render, screen } from '@testing-library/react';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import Controls from '../../src/ui/controls';
import * as useBoard from '../../src/ui/hooks/useBoard';

const mockBoardStore = {
  board: [[false]],
  boardId: '123',
  setBoard: vi.fn(),
  setCell: vi.fn(),
  toggleCell: vi.fn(),
  resetBoard: vi.fn(),
  createBoard: vi.fn(),
  getNextBoard: vi.fn(),
  getNextBoardTimes: vi.fn(),
  getFinalBoard: vi.fn(),
  getCurrentBoard: vi.fn(),
  alert: {
    message: null,
    type: null,
    showAlert: vi.fn(),
  },
};

describe('Controls', () => {
  beforeEach(() => {
    vi.spyOn(useBoard, 'useBoardStore').mockReturnValue(mockBoardStore);
  });

  it('renderiza os botões principais', () => {
    render(<Controls />);
    const buttons = screen.getAllByRole('button');
    expect(buttons).toHaveLength(6);
  });
});
