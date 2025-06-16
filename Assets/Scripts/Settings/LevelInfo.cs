using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelInfo : ScriptableObject
{
    public GameObject locationPrefab;

    [System.Serializable]
    public struct Object
    {
        public GameObject prefab;
        public Vector3 position;
    }

    public Object[] towers;
    public Object[] spawners;
}
