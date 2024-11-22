using UnityEngine;
using Zenject;
public enum TileType
{
    Orange, ColorBomb, Yellow, Blue, Cherry, Watermelon, Green
}
public abstract class TileBase : MonoBehaviour , IPoolObject
{
    [SerializeField] protected TileType type;
    [SerializeField] protected PoolObjectType poolType;
    [Inject] protected IMatchFinder matchFinder;
    [Inject] protected IMatchable matchable;
    private int column;
    private int row;

    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }
    public bool IsMatched { get => matchable.IsMatched; set => matchable.IsMatched = value; }
    public TileType Type => type;
    public abstract bool FindMatch(bool mark = false);
    public PoolObjectType GetPoolType()
    {
        return poolType;
    }
    public virtual void OnReturnPool()
    {
        IsMatched = false;
    }
    public virtual void OnMatch()
    {
        matchable.OnMatch();
    }
}
