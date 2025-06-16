using UnityEngine;
using System.Collections;


public class SimpleTower : Tower
{


	protected override void Shot()
    {
        base.Shot();

		currentProjectile.transform.position = shootPoint.position;
		currentProjectile.transform.rotation = Quaternion.identity;

		GameObject projectileGO = currentProjectile.gameObject;

		projectile = projectileGO.GetComponent<BaseProjectile>();
		projectile.SetTarget(currentTarget.gameObject);
	}


	
}
