using UnityEngine;
using Zenject;

public class BoardLockState : IBoardState
{
    private Board board;
    private SignalBus signalBus;
    public BoardLockState(Board board, SignalBus signalBus)
    {
        this.board = board;
        this.signalBus = signalBus;

        signalBus.Subscribe<SpinStopSignal>(OnSpinStop);
        signalBus.Subscribe<RestartGameSignal>(OnRestartGame);
    }
    public void OnClickTile(TileBase tile, Vector3 clickPosition)
    {
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnReleaseTile(TileBase tile, Vector3 releasePosition)
    {
    }
    private void OnSpinStop()
    {
        board.StateMachine.ChangeState(board.ReadySwipeState);
    }
    private void OnRestartGame()
    {
        board.ClearBoard();
        board.CreateBoard();
        board.StateMachine.ChangeState(board.ReadySwipeState);
    }
}
