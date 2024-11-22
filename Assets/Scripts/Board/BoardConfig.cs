using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Config", menuName = "Configurations/Board Config")]
public class BoardConfig : ScriptableObject
{
    public List<TileBase> TilesToCreate;
    public PoolObjectType ContainerType;
    public GameObject MaskPrefab;
    public Vector2Int[] RandomBoardSizes;
    public int MinForEachType = 3;
    public const int ExtraLineCount = 1;
}
