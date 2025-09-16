import { useRef, useSyncExternalStore } from 'react';
import { createBoard as apiCreateBoard, getNextBoard as apiGetNextBoard, getNextBoardTimes as apiGetNextBoardTimes, getFinalBoard as apiGetFinalBoard, getCurrentBoard as apiGetCurrentBoard } from '../../api';
import { useAlertStore } from './useAlert';

export const GRID_SIZE = 20;

function createInitialBoard(initialBoard?: boolean[][]) {
    return initialBoard ?? Array(GRID_SIZE).fill(null).map(() => Array(GRID_SIZE).fill(false));
}

class BoardStore {
    private listeners: Set<() => void> = new Set();
    private _board: boolean[][];
    boardId: string | null = null;
    alert = null;

    constructor(initialBoard?: boolean[][]) {
        this._board = createInitialBoard(initialBoard);
        const storedId = localStorage.getItem('boardId');
        if (storedId) {
            this.boardId = storedId;
            this.getCurrentBoard();
        }
    }

    setAlertHook(hook: ReturnType<typeof useAlertStore>) {
        this.alert = hook;
    }

    get board() {
        return this._board;
    }

    setBoard = (newBoard: boolean[][]) => {
        this._board = newBoard;
        this.emit();
    };

    setCell = (row: number, col: number, value: boolean) => {
        this._board = this._board.map((r, i) =>
            i === row ? r.map((cell, j) => j === col ? value : cell) : r
        );
        this.emit();
    };

    toggleCell = (row: number, col: number) => {
        this._board = this._board.map((r, i) =>
            i === row ? r.map((cell, j) => j === col ? !cell : cell) : r
        );
        this.emit();
    };

    resetBoard = (initialBoard?: boolean[][]) => {
        this._board = createInitialBoard(initialBoard);
        this.boardId = null;
        localStorage.removeItem('boardId');
        this.emit();
    };

    async createBoard() {
        const cells = this._board.map(row => row.map(cell => cell ? '1' : '0').join('')).join('');
        try {
            const response = await apiCreateBoard({ cells });
            this.boardId = response.boardId;
            localStorage.setItem('boardId', response.boardId);
            this._board = response.boardId ? this._board : createInitialBoard();            
            this.emit();
        } catch (err) {
            if (this.alert) this.alert.showAlert(err.message, 'error');
        }
    }

    async getNextBoard() {
        if (!this.boardId) {
            if (this.alert) this.alert.showAlert('BoardId not defined. Please create the board first.', 'error');
            return;
        }
        try {
            const response = await apiGetNextBoard(this.boardId);
            this._board = response.cells.split('').map((v, i, arr) => {
                const rowLength = Math.sqrt(arr.length);
                if (i % rowLength === 0) {
                    return arr.slice(i, i + rowLength).map(cell => cell === '1');
                }
                return null;
            }).filter(Boolean) as boolean[][];
            this.emit();
        } catch (err) {
            if (this.alert) this.alert.showAlert(err.message, 'error');
        }
    }

    async getNextBoardTimes(times: number) {
        if (!this.boardId) {
            if (this.alert) this.alert.showAlert('BoardId not defined. Please create the board first.', 'error');
            return;
        }
        try {
            const response = await apiGetNextBoardTimes(this.boardId, times);
            this._board = response.cells.split('').map((v, i, arr) => {
                const rowLength = Math.sqrt(arr.length);
                if (i % rowLength === 0) {
                    return arr.slice(i, i + rowLength).map(cell => cell === '1');
                }
                return null;
            }).filter(Boolean) as boolean[][];
            this.emit();
        } catch (err) {
            if (this.alert) this.alert.showAlert(err.message, 'error');
        }
    }

    async getFinalBoard() {
        if (!this.boardId) {
            if (this.alert) this.alert.showAlert('BoardId not defined. Please create the board first.', 'error');
            return;
        }
        try {
            const response = await apiGetFinalBoard(this.boardId);
            this._board = response.cells.split('').map((v, i, arr) => {
                const rowLength = Math.sqrt(arr.length);
                if (i % rowLength === 0) {
                    return arr.slice(i, i + rowLength).map(cell => cell === '1');
                }
                return null;
            }).filter(Boolean) as boolean[][];
            this.emit();
        } catch (err) {
            if (this.alert) this.alert.showAlert(err.message, 'error');
        }
    }

    async getCurrentBoard() {
        if (!this.boardId) {
            if (this.alert) this.alert.showAlert('BoardId not defined. Please create the board first.', 'error');
            return;
        }
        try {
            const response = await apiGetCurrentBoard(this.boardId);
            this._board = response.cells.split('').map((v, i, arr) => {
                const rowLength = Math.sqrt(arr.length);
                if (i % rowLength === 0) {
                    return arr.slice(i, i + rowLength).map(cell => cell === '1');
                }
                return null;
            }).filter(Boolean) as boolean[][];
            this.emit();
        } catch (err) {
            if (this.alert) this.alert.showAlert(err.message, 'error');
        }
    }

    subscribe = (listener: () => void) => {
        this.listeners.add(listener);
        return () => this.listeners.delete(listener);
    };

    emit = () => {
        this.listeners.forEach(fn => fn());
    };
}

const boardStore = new BoardStore();

export function useBoardStore() {
    const storeRef = useRef(boardStore);
    const board = useSyncExternalStore(
        storeRef.current.subscribe,
        () => storeRef.current.board
    );

    const boardId = useSyncExternalStore(
        storeRef.current.subscribe,
        () => storeRef.current.boardId
    );

    // Adiciona o hook de alerta ao store
    const alert = useAlertStore();
    storeRef.current.setAlertHook(alert);

    return {
        board,
        boardId,
        setBoard: storeRef.current.setBoard,
        setCell: storeRef.current.setCell,
        toggleCell: storeRef.current.toggleCell,
        resetBoard: storeRef.current.resetBoard,
        createBoard: storeRef.current.createBoard.bind(storeRef.current),
        getNextBoard: storeRef.current.getNextBoard.bind(storeRef.current),
        getNextBoardTimes: storeRef.current.getNextBoardTimes.bind(storeRef.current),
        getFinalBoard: storeRef.current.getFinalBoard.bind(storeRef.current),
        getCurrentBoard: storeRef.current.getCurrentBoard.bind(storeRef.current),
        alert,
    };
}
