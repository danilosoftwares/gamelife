import { useState, useRef } from 'react';
import { useBoardStore } from '../hooks/useBoard';

const INTERVAL_MS = 500;

const Controls = () => {
    const {
        resetBoard,
        createBoard,
        getNextBoard,
        getNextBoardTimes,
        boardId,
    } = useBoardStore();

    const [steps, setSteps] = useState<Number | undefined>(1);
    const [isPlaying, setIsPlaying] = useState(false);
    const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);

    const handleAdvance = async (steps: number) => {
        await getNextBoardTimes(steps);
    };

    const handleNext = async () => {
        await getNextBoard();
    };

    const handleCreate = async () => {
        await createBoard();
    };

    const handlePlay = () => {
        if (isPlaying) return;
        setIsPlaying(true);
        timerRef.current = setInterval(() => {
            getNextBoard();
        }, INTERVAL_MS);
    };

    const handlePause = () => {
        setIsPlaying(false);
        if (timerRef.current) {
            clearInterval(timerRef.current);
            timerRef.current = null;
        }
    };

    const handleStepsInput = (e: React.FormEvent<HTMLInputElement>) => {
        const input = e.currentTarget;
        input.value = input.value.replace(/[^0-9]/g, '');
        if (input.value !== '') {
            let num = parseInt(input.value, 10);
            if (num < 1) num = 1;
            if (num > 99) num = 99;
            input.value = num.toString();
            setSteps(num);
        } else {
            setSteps(undefined);
        }
    };

    return (
        <div style={{ marginBottom: "20px", display: "flex", justifyContent: "space-between" }}>
            <button onClick={handleCreate} disabled={isPlaying}>Create</button>
            <button onClick={() => resetBoard()} disabled={isPlaying || !boardId}>Reset</button>
            <div className='div-gray-bar'/>
            <button onClick={() => handleNext()} disabled={isPlaying || !boardId}>Next</button>
            <button onClick={() => handleAdvance(10)} disabled={isPlaying || !boardId || steps == undefined}>Advance 10</button>
            <input
                name="steps"
                placeholder="Steps"
                defaultValue="1"
                style={{ width: '60px', textAlign: 'center' }}
                type="number"
                min={1}
                max={99}
                step={1}
                disabled={isPlaying || !boardId}
                inputMode="numeric"
                pattern="[0-9]*"
                onInput={handleStepsInput}
            />
            <div className='div-gray-bar'/>          
            <button onClick={handlePlay} disabled={isPlaying || !boardId}>Play</button>
            <button onClick={handlePause} disabled={!isPlaying}>Pause</button>
        </div>
    );
};

export default Controls;