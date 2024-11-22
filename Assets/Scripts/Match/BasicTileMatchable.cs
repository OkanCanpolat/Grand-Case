using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BasicTileMatchable : IMatchable
{
    [Inject] private Board board;
    [Inject] private TileBase tile;
    [Inject] private SwipeConfig swipeConfig;
    private bool isMatched;
    public bool IsMatched { get => isMatched; set => isMatched = value; }
  
    public async void OnMatch()
    {
        await Task.Delay((int)(swipeConfig.MatchAnimationDelay * 1000));
        Vector3 initialScale = tile.gameObject.transform.localScale;
        iTween.ScaleTo(tile.gameObject, iTween.Hash("scale", initialScale * swipeConfig.ScalePingPongScaler, "time", swipeConfig.MatchAnimationScaleTime,  "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.pingPong));
        await Task.Delay((int)(swipeConfig.MatchAnimationTime * 1000));
        iTween.Stop(tile.gameObject);
        iTween.ScaleTo(tile.gameObject, iTween.Hash("scale", initialScale, "time", swipeConfig.MatchAnimationScaleTime, "easetype", iTween.EaseType.linear));
    }
}
