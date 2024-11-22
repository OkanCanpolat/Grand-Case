using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MinXRandomBoardCreator : IBoardCreator
{
    [Inject] private Board board;
    [Inject] private SignalBus signalBus;
    [Inject] private IObjectPool objectPool;
    private BoardConfig boardConfig;
    private GameObject mask;
    private Dictionary<PoolObjectType, int> boardStructure;
    private List<TileContainer> emptyContainers = new List<TileContainer>();
    private List<TileContainer> emptyExpandedContainers = new List<TileContainer>();
    private System.Random random = new System.Random();

    public MinXRandomBoardCreator(BoardConfig boardConfig)
    {
        this.boardConfig = boardConfig;
        boardStructure = new Dictionary<PoolObjectType, int>();
        mask = Object.Instantiate(boardConfig.MaskPrefab);
    }
    public void CreateBoard()
    {
        InitBoard();
        SetupMask();
        CreateContainers();
        CreateMin3();
        FillEmptyContainers();
        signalBus.TryFire<BoardCreateSignal>();
    }
    private void InitBoard()
    {
        int index = Random.Range(0, boardConfig.RandomBoardSizes.Length);
        Vector2Int size = boardConfig.RandomBoardSizes[index];
        board.Width = size.x;
        board.Height = size.y;
        board.ExpandedHeight = size.y + BoardConfig.ExtraLineCount;
        board.Containers = new TileContainer[size.x, size.y + BoardConfig.ExtraLineCount];
    }
    private void SetupMask()
    {
        mask.transform.position = new Vector3((board.Width - 1) / 2, (board.Height - 1) / 2);
        Vector3 scale = new Vector3(board.Width, board.Height, 1);
        mask.transform.localScale = scale;
    }
    private void CreateContainers()
    {
        for (int i = 0; i < board.Containers.GetLength(0); i++)
        {
            for (int j = 0; j < board.Containers.GetLength(1); j++)
            {
                Vector3 position = new Vector3(i, j);
                TileContainer container = objectPool.Get(boardConfig.ContainerType).GetComponent<TileContainer>();
                container.transform.position = position;
                board.Containers[i, j] = container;
                AddEmptyContainer(container, j);
                container.Column = i;
                container.Row = j;
            }
        }
    }
    private void AddEmptyContainer(TileContainer container, int height)
    {
        if (height >= board.Height)
        {
            emptyExpandedContainers.Add(container);
        }
        else
        {
            emptyContainers.Add(container);
        }
    }
    private void CreateMin3()
    {
        for (int i = 0; i < boardConfig.TilesToCreate.Count; i++)
        {
            PoolObjectType poolObjectType = boardConfig.TilesToCreate[i].GetPoolType();
            CreateTypeAtRandom(poolObjectType, boardConfig.MinForEachType);
        }
    }
    private void FillEmptyContainers(bool extraLine = false)
    {
        List<TileContainer> containers = extraLine ? emptyExpandedContainers : emptyContainers;

        for (int i = containers.Count - 1; i >= 0; i--)
        {
            TileContainer container = containers[i];

            boardConfig.TilesToCreate.Shuffle(random);

            for (int k = 0; k < boardConfig.TilesToCreate.Count; k++)
            {
                TileBase createdTile = objectPool.Get(boardConfig.TilesToCreate[k].GetPoolType()).GetComponent<TileBase>();
                createdTile.transform.position = new Vector3(container.Column, container.Row, -1);
                createdTile.Column = container.Column;
                createdTile.Row = container.Row;

                if (!createdTile.FindMatch())
                {
                    container.SetTile(createdTile);
                    containers.Remove(container);
                    break;
                }
                else
                {
                    objectPool.ReturnToPool(createdTile.gameObject);
                }
            }
        }
    }
    public void CreateExtraLine()
    {
        GetSctructure();

        foreach (var item in boardStructure)
        {
            if (item.Value < boardConfig.MinForEachType)
            {
                int count = boardConfig.MinForEachType - item.Value;
                CreateTypeAtRandom(item.Key, count, true);
            }
        }
        FillEmptyContainers(true);
    }
    private void CreateTypeAtRandom(PoolObjectType type, int count, bool extraLine = false)
    {
        List<TileContainer> containers = extraLine ? emptyExpandedContainers : emptyContainers;

        for (int j = 0; j < count; j++)
        {
            containers.Shuffle(random);

            for (int i = 0; i < containers.Count; i++)
            {
                TileContainer container = containers[i];
                Vector3 pos = new Vector3(container.Column, container.Row, -1);
                TileBase createdTile = objectPool.Get(type).GetComponent<TileBase>();
                createdTile.transform.position = pos;
                createdTile.Column = container.Column;
                createdTile.Row = container.Row;

                if (!createdTile.FindMatch())
                {
                    container.SetTile(createdTile);
                    containers.Remove(container);
                    break;
                }
                else
                {
                    objectPool.ReturnToPool(createdTile.gameObject);
                }
            }
        }
       
    }
    private void GetSctructure()
    {
        boardStructure.Clear();

        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 1; j < board.Height; j++)
            {
                PoolObjectType tile = board.GetTile(i, j).GetPoolType();

                if (boardStructure.ContainsKey(tile))
                {
                    boardStructure[tile]++;
                }
                else
                {
                    boardStructure.Add(tile, 1);
                }
            }
        }
    }
    public void DecreaseRows()
    {
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.ExpandedHeight - 1; j++)
            {
                board.Containers[i, j].SetTile(board.Containers[i, j + 1].Tile);
            }

            board.Containers[i, (int)board.ExpandedHeight - 1].SetTile(null);
            emptyExpandedContainers.Add(board.Containers[i, (int)board.ExpandedHeight - 1]);
        }
    }
    public void DestroyBottom()
    {
        for (int i = 0; i < board.Width; i++)
        {
            TileBase tileBase = board.Containers[i, 0].Tile;
            objectPool.ReturnToPool(tileBase.gameObject);
        }
    }
    public void ClearBoard()
    {
        foreach (var container in board.Containers)
        {
            if (container.Tile != null)
            {
                objectPool.ReturnToPool(container.Tile.gameObject);
            }
            objectPool.ReturnToPool(container.gameObject);
        }
        board.Containers = null;
        emptyContainers.Clear();
        emptyExpandedContainers.Clear();
    }
}



