
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] ProjectileInfo myInfo;

    public delegate void ProjectileDestroyer(Projectile m);
    public event ProjectileDestroyer OnProjectileDestroyed;


	public ProjectileInfo MyInfo
	{
		get { return myInfo; }
	}


	void OnTriggerEnter(Collider other)
	{
		IDamageable target = other.gameObject.GetComponent<IDamageable>();
		if (target != null)
			target.DamageMe(myInfo.damage);

		DestroyMe();
	}

	public void DestroyMe()
	{
		OnProjectileDestroyed?.Invoke(this);
	}

}

/*
public enum ProjectileType
{
	Cannon,
	Guided
}
*/