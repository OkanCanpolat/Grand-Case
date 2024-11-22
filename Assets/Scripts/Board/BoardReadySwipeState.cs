using UnityEngine;
using Zenject;

public class BoardReadySwipeState : IBoardState
{
    private Board board;
    private TileBase lastClickedTile;
    private Vector3 touchPosition;
    private SwipeConfig swipeSettings;
    private SignalBus signalBus;
    [Inject] private ISwipeDirectionCalculator swipeDirectionCalculator;
    [Inject(Id = "Swiping")] private BoardSwipingState swipingState;
 
    public BoardReadySwipeState(Board board, SwipeConfig swipeSettings, SignalBus signalBus)
    {
        this.board = board;
        this.swipeSettings = swipeSettings;
        this.signalBus = signalBus;
        this.signalBus.Subscribe<SpinStartSignal>(OnSpinStart);
        this.signalBus.Subscribe<LevelFailedSignal>(() => board.StateMachine.ChangeState(board.LockState));
    }
    public void OnClickTile(TileBase tile, Vector3 clickPosition)
    {
        touchPosition = clickPosition;
        lastClickedTile = tile;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnReleaseTile(TileBase tile, Vector3 releasePosition)
    {
        if (lastClickedTile != tile) return;

        if (Vector3.Distance(touchPosition, releasePosition) >= swipeSettings.MinDistanceToSwipe)
        {
            Vector2 direction = swipeDirectionCalculator.GetDirection(touchPosition, releasePosition);
            CheckSwipe(direction);
        }
    }

    private void CheckSwipe(Vector2 direction)
    {
        int column = lastClickedTile.Column + (int)direction.x;
        int row = lastClickedTile.Row + (int)direction.y;

        TileBase secondTile = board.GetTile(column, row);
        if (secondTile == null) return;

        swipingState.ClickedTile = lastClickedTile;
        swipingState.SecondTile = secondTile;
        board.StateMachine.ChangeState(swipingState);
    }
    private void OnSpinStart()
    {
        board.StateMachine.ChangeState(board.LockState);
    }
}
