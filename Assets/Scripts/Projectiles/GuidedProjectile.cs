using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Projectile))]
public class GuidedProjectile : BaseProjectile
{

	[Header("Guided")]
	[SerializeField] private float destroyDelay = 2f;
	GameObject target;
	bool isToDestroy = false;

	public override void SetTarget(GameObject _target)
	{
		target = _target;
	}

	public override void Move()
    {
		if (target)
			if (target.activeInHierarchy)
				translation = target.transform.position - transform.position;

		if (translation.magnitude > projectile.MyInfo.speed * Time.deltaTime)
		{
			translation = translation.normalized * projectile.MyInfo.speed * Time.deltaTime;
		}
		transform.position += translation;//transform.Translate(translation);
	}

    private void OnEnable()
    {
		isToDestroy = false;
		StartCoroutine(DestroyAfterDelay());
	}

    void Update () 
	{
		if(target && !target.activeSelf)
        {
			target = null;
        }

		Move();
	}

	private IEnumerator DestroyAfterDelay()
	{
		yield return new WaitForSeconds(destroyDelay);
		if (target == null && !isToDestroy)
		{
			projectile.DestroyMe();
		}
	}


}
