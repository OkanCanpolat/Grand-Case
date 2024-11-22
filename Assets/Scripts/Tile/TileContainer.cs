using UnityEngine;
using Zenject;
using System.Collections;
public class TileContainer : MonoBehaviour, IPoolObject
{
    public int Column;
    public int Row;
    [SerializeField] private PoolObjectType poolType;
    [SerializeField] private GameObject matchAnimationParent;
    [Inject] private SignalBus signalBus;
    [Inject] private SwipeConfig swipeConfig;
    private TileBase tile;
    public TileBase Tile => tile;

    private void Awake()
    {
        signalBus.Subscribe<MatchFoundSignal>(OnMatchFound);
    }

    public void SetTile(TileBase tile)
    {
        this.tile = tile;
        if (tile == null) return;
        tile.Column = Column;
        tile.Row = Row;
    }
    public PoolObjectType GetPoolType()
    {
        return poolType;
    }
    public void OnReturnPool()
    {
        tile = null;
    }
    public void OnMatchFound()
    {
        if (tile == null || !tile.IsMatched)
        {
            return;
        }

        StartCoroutine(CoOnMatchFound());
    }
    private IEnumerator CoOnMatchFound()
    {
        yield return new WaitForSeconds(swipeConfig.MatchAnimationDelay);
        matchAnimationParent.SetActive(true);
        yield return new WaitForSeconds(swipeConfig.MatchAnimationTime);
        matchAnimationParent.SetActive(false);
    }
   
}
