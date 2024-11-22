using UnityEngine;
using Zenject;

public class Board : MonoBehaviour
{
    private float width;
    private float height;
    private float expandedHeight;
    private TileContainer[,] containers;
    [Inject] IObjectPool objectPool;
    [Inject] IBoardCreator boardCreator;
    [Inject] public BoardStateMachine StateMachine;
    [Inject(Id = "ReadySwipe")] public BoardReadySwipeState ReadySwipeState;
    [Inject(Id = "Swiping")] public BoardSwipingState SwipingState;
    [Inject(Id = "Lock")] public BoardLockState LockState;
   
    public float Width { get => width; set => width = value; }
    public float Height { get => height; set => height = value; }
    public float ExpandedHeight { get => expandedHeight; set => expandedHeight = value; }

    public TileContainer[,] Containers { get => containers; set => containers = value; }
   
    private void Start()
    {
        Application.targetFrameRate = 60;
        objectPool.SetupPool();
        boardCreator.CreateBoard();
    }
    public void OnClickTile(TileBase tile, Vector3 clickPosition)
    {
        StateMachine.CurrentState.OnClickTile(tile, clickPosition);
    }
    public void OnReleaseTile(TileBase tile, Vector3 clickPosition)
    {
        StateMachine.CurrentState.OnReleaseTile(tile, clickPosition);
    }
    public TileBase GetTile(int column, int row)
    {
        if (!IsValidTile(column, row)) return null;

        return containers[column, row].Tile;
    }
    public bool IsValidTile(int column, int row)
    {
        return !(column >= width || column < 0 || row >= expandedHeight || row < 0);
    }
    public void CreateBoard()
    {
        boardCreator.CreateBoard();
    }
    public void ClearBoard()
    {
        boardCreator.ClearBoard();
    }
    public void DestroyBottom()
    {
        boardCreator.DestroyBottom();
    }
    public void DecreaseRows()
    {
        boardCreator.DecreaseRows();
    }
    public void CreateExtraLine()
    {
        boardCreator.CreateExtraLine();
    }
}

public class BoardCreateSignal { }
