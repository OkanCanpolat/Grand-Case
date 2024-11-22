using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPool : IObjectPool
{
    private ObjectPoolConfig config;
    private Dictionary<PoolObjectType, Queue<GameObject>> objects;
    [Inject] private Factory factory;
    public ObjectPool(ObjectPoolConfig config)
    {
        this.config = config;
        objects = new Dictionary<PoolObjectType, Queue<GameObject>>();
    }
    
    public GameObject Get(PoolObjectType type)
    {
        if (!objects.ContainsKey(type)) return null;

        if (objects[type].Count <= 0)
        {
            InstantiatePoolObject(type);
        }

        GameObject poolObject = objects[type].Dequeue();
        poolObject.SetActive(true);
        return poolObject;
    }

    public void ReturnToPool(GameObject poolObject)
    {
        IPoolObject poolableObject = poolObject.GetComponent<IPoolObject>();
        PoolObjectType type = poolableObject.GetPoolType();

        if (!objects.ContainsKey(type)) return;

        poolableObject.OnReturnPool();
        poolObject.SetActive(false);

        objects[type].Enqueue(poolObject);
    }
    public void SetupPool()
    {
        foreach (PoolObjectTypeMap typeMap in config.PoolObjectTypeMaps)
        {
            PoolObjectType type = typeMap.Type;

            if (objects.ContainsKey(type)) continue;

            Queue<GameObject> objectQueue = new Queue<GameObject>();

            objects.Add(type, objectQueue);

            for (int i = 0; i < typeMap.InitialCount; i++)
            {
                GameObject poolObject = factory.Create(typeMap.ObjectPrefab);
                poolObject.gameObject.SetActive(false);
                objectQueue.Enqueue(poolObject);
            }
        }
    }
    private void InstantiatePoolObject(PoolObjectType type)
    {
        if (!objects.ContainsKey(type)) return;

        foreach (PoolObjectTypeMap typeMap in config.PoolObjectTypeMaps)
        {
            if (typeMap.Type == type)
            {
                GameObject poolObject = factory.Create(typeMap.ObjectPrefab);
                poolObject.SetActive(false);
                objects[type].Enqueue(poolObject);
                return;
            }
        }
    }
}

public class Factory : PlaceholderFactory<GameObject, GameObject> { }
public class CustomFactory : IFactory<GameObject, GameObject>
{
    readonly private DiContainer container;
    public CustomFactory(DiContainer container)
    {
        this.container = container;
    }
    public GameObject Create(GameObject prefab)
    {
        return container.InstantiatePrefab(prefab);
    }
}
