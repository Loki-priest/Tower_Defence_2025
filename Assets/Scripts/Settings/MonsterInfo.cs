using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MonsterInfo : ScriptableObject
{
	public float speed = 0.1f;
	public float maxHP = 30;
	public float reachDistance = 0.3f;
}
