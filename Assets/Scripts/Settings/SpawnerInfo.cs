using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SpawnerInfo : ScriptableObject
{
    public GameObject monsterPrefab;

    public float spawnInterval = 3;
}
