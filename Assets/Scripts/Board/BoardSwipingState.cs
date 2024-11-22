using System.Threading.Tasks;
using UnityEngine;
using Zenject;
public class BoardSwipingState : IBoardState
{
    public TileBase ClickedTile;
    public TileBase SecondTile;
    private Board board;
    [Inject] private SwipeConfig swipeSettings;
    [Inject] private SignalBus signalBus;
    [Inject] private IAudioService audioService;
    [Inject] private GameViewSoundConfig soundConfig;
    public BoardSwipingState(Board board)
    {
        this.board = board;
    }
    public void OnClickTile(TileBase tile, Vector3 clickPosition)
    {
    }
    public void OnEnter()
    {
        audioService.PlaySoundOnce(soundConfig.SwipeSound, soundConfig.SwipeVolume);
        signalBus.TryFire<SwipeStartSignal>();
        SwapTiles();
    }
    public void OnExit()
    {
        ClickedTile = null;
        SecondTile = null;
    }
    public void OnReleaseTile(TileBase tile, Vector3 releasePosition)
    {
    }
    private async void SwapTiles()
    {
        await StartSwipeAnim();
        ControlMatch();
    }
    private async Task StartSwipeAnim()
    {
        int clickedColumn = ClickedTile.Column;
        int clickedRow = ClickedTile.Row;

        board.Containers[SecondTile.Column, SecondTile.Row].SetTile(ClickedTile);
        board.Containers[clickedColumn, clickedRow].SetTile(SecondTile);

        Vector3 clickedTargetPos = new Vector3(ClickedTile.Column, ClickedTile.Row, -1);
        Vector3 secondTargetPos = new Vector3(SecondTile.Column, SecondTile.Row, -1);

        iTween.MoveTo(ClickedTile.gameObject, iTween.Hash("position", clickedTargetPos, "time", swipeSettings.SwipeTime, "easetype", iTween.EaseType.easeOutBounce));
        iTween.MoveTo(SecondTile.gameObject, iTween.Hash("position", secondTargetPos, "time", swipeSettings.SwipeTime, "easetype", iTween.EaseType.easeOutBounce));
        iTween.PunchScale(ClickedTile.gameObject, iTween.Hash("amount", swipeSettings.PunchScale, "time", swipeSettings.SwipeTime));
        iTween.PunchScale(SecondTile.gameObject, iTween.Hash("amount", swipeSettings.PunchScale, "time", swipeSettings.SwipeTime));
        iTween.PunchRotation(SecondTile.gameObject, iTween.Hash("amount", swipeSettings.PunchRotation, "time", swipeSettings.SwipeTime));
        iTween.PunchRotation(ClickedTile.gameObject, iTween.Hash("amount", swipeSettings.PunchRotation, "time", swipeSettings.SwipeTime));

        await Task.Delay((int)swipeSettings.SwipeTime * 1000);
    }
    private void ControlMatch()
    {
        if (ClickedTile.FindMatch(true) | SecondTile.FindMatch(true))
        {
            PerformMatchActions();
            audioService.PlaySoundOnce(soundConfig.MatchSound, soundConfig.MatchVolume);
            board.StateMachine.ChangeState(board.LockState);
            signalBus.TryFire<MatchFoundSignal>();
        }
        else
        {
            board.StateMachine.ChangeState(board.ReadySwipeState);
            signalBus.TryFire<UnvalidSwipeSignal>();
        }
    }
    private void PerformMatchActions()
    {
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                TileBase tile = board.Containers[i, j].Tile;

                if(tile != null && tile.IsMatched)
                {
                    tile.OnMatch();
                }
            }
        }
    }
}
public class MatchFoundSignal { }
public class SwipeStartSignal { }
public class UnvalidSwipeSignal { }
