using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
	[SerializeField] SpawnerInfo myInfo;
	[SerializeField] Transform moveTarget;
	[SerializeField] Transform spawnPosition;
	[SerializeField] Transform monsterContainer;

	private ObjectPool<Monster> monsterPool;
	private float lastSpawnTime = -1;
	private int monsterCount;

	private void Awake()
	{
		monsterPool = new ObjectPool<Monster>(
			createFunc: () =>
			{
				var monster = Instantiate(myInfo.monsterPrefab, monsterContainer).GetComponent<Monster>();
				monster.OnBotKilled += m => monsterPool.Release(m);
				return monster;
			},
			actionOnGet: m =>
			{
				m.transform.position = spawnPosition.position;
				m.SetMoveTarget(moveTarget);
				m.ResetMe();
				m.gameObject.SetActive(true);
				m.name = $"Monster{++monsterCount}";
			},
			actionOnRelease: m => m.gameObject.SetActive(false),
			actionOnDestroy: m => Destroy(m.gameObject),
			collectionCheck: false,
			defaultCapacity: 10,
			maxSize: 50
		);
	}

	private void Update()
	{
		if (Time.time > lastSpawnTime + myInfo.spawnInterval )
		{
			monsterPool.Get();
			lastSpawnTime = Time.time;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.gray;
		Gizmos.DrawLine(spawnPosition.position, moveTarget.position);
	}



}
