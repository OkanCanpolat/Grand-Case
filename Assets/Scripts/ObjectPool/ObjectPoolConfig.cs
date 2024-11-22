using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ObjectPool Config", menuName = "Configurations/ObjectPool Config")]
public class ObjectPoolConfig : ScriptableObject
{
    public List<PoolObjectTypeMap> PoolObjectTypeMaps;
}
