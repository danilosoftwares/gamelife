
import Controls from '../controls';
import Board from '../board';
import AlertMessage from '../alert/AlertMessage';

function Home() {

  return (
    <>
      <AlertMessage />
      <img src="/logo.jpg" alt="Logo Game of Life" style={{ display: 'block', margin: '20px auto' }} />
      <Controls />
      <Board />
    </>
  );
}

export default Home;
