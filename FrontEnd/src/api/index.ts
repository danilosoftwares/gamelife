const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:8080';

export interface CreateBoardRequest {
  cells: string;
}

export interface CreateBoardResponse {
    success: boolean;
    boardId: string;
    error: string | null;
}
export interface BoardNextResponse {
    boardId: string;
    cells: string;
    error: string | null;    
}

export async function createBoard(data: CreateBoardRequest): Promise<CreateBoardResponse> {
  const res = await fetch(`${API_URL}/Boards`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (res.status === 400) {
    const errorResponse = await res.json();
    throw new Error(errorResponse.error || 'Erro ao criar board');
  }
  if (!res.ok) throw new Error('Erro ao criar board');
  return await res.json() as CreateBoardResponse; 
}

export async function getNextBoard(id: string): Promise<BoardNextResponse> {
  const res = await fetch(`${API_URL}/Boards/${id}/next`);
  if (res.status === 400) {
    const errorResponse = await res.json();
    throw new Error(errorResponse.error || 'Erro ao buscar próximo board');
  }
  if (!res.ok) throw new Error('Erro ao buscar próximo board');
  return await res.json() as BoardNextResponse; 
}

export async function getNextBoardTimes(id: string, times: number): Promise<BoardNextResponse> {
  const res = await fetch(`${API_URL}/Boards/${id}/next/${times}`);
  if (res.status === 400) {
    const errorResponse = await res.json();
    throw new Error(errorResponse.error || 'Erro ao buscar board avançado');
  }
  if (!res.ok) throw new Error('Erro ao buscar board avançado');
  return await res.json() as BoardNextResponse; 
}

export async function getFinalBoard(id: string): Promise<BoardNextResponse> {
  const res = await fetch(`${API_URL}/Boards/${id}/final`);
  if (res.status === 400) {
    const errorResponse = await res.json();
    throw new Error(errorResponse.error || 'Erro ao buscar board final');
  }
  if (!res.ok) throw new Error('Erro ao buscar board final');
  return await res.json() as BoardNextResponse; 
}

export async function getCurrentBoard(id: string): Promise<BoardNextResponse> {
  const res = await fetch(`${API_URL}/Boards/${id}/current`);
  if (res.status === 400) {
    const errorResponse = await res.json();
    throw new Error(errorResponse.error || 'Erro ao buscar board current');
  }
  if (!res.ok) throw new Error('Erro ao buscar board current');
  return await res.json() as BoardNextResponse; 
}