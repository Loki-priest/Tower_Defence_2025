using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TowerInfo : ScriptableObject
{
	public float shootInterval = 0.5f;
	public float range = 4f;
	public GameObject projectilePrefab;
}
