using System;
using UnityEngine;

[Serializable]
public class PoolObjectTypeMap
{
    public PoolObjectType Type;
    public GameObject ObjectPrefab;
    public int InitialCount;
}
public enum PoolObjectType
{
    Orange, ColorBomb, Yellow, Blue, Cherry, Watermelon, Green, TileContainer
}
public interface IObjectPool
{
    public GameObject Get(PoolObjectType type);
    public void ReturnToPool(GameObject poolObject);
    public void SetupPool();
}
