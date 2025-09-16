import { describe, it, afterEach, vi, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import AlertMessage from '../../src/ui/alert/AlertMessage';
import * as useAlert from '../../src/ui/hooks/useAlert';

// Garante que os mocks não vazem entre os testes
afterEach(() => {
  vi.clearAllMocks();
});

describe('AlertMessage', () => {
  it('não renderiza nada se não houver mensagem', () => {
    vi.spyOn(useAlert, 'useAlertStore').mockReturnValue({ message: null, type: null, showAlert: vi.fn() });
    render(<AlertMessage />);
    expect(screen.queryByText(/./)).toBeNull();
  });

  it('renderiza mensagem de erro', () => {
    vi.spyOn(useAlert, 'useAlertStore').mockReturnValue({ message: 'Erro!', type: 'error', showAlert: vi.fn() });
    render(<AlertMessage />);
    expect(screen.getByText('Erro!')).toBeDefined();
    expect(screen.getByText('Erro!')).toHaveStyle({ color: 'rgb(255, 0, 0)' });
  });

  it('renderiza mensagem de sucesso', () => {
    vi.spyOn(useAlert, 'useAlertStore').mockReturnValue({ message: 'Sucesso!', type: 'success', showAlert: vi.fn() });
    render(<AlertMessage />);
    expect(screen.getByText('Sucesso!')).toBeDefined();
    expect(screen.getByText('Sucesso!')).toHaveStyle({ color: 'rgb(0, 128, 0)' });
  });
});
