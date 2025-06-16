using UnityEngine;


[RequireComponent(typeof(Projectile))]
public class CannonProjectile: BaseProjectile
{

	[Header("Cannon")]
	[SerializeField] float lifeTime = 5.0f;
	float time = 1.0f;




    public override void Move()
    {
		translation = transform.forward * projectile.MyInfo.speed * Time.deltaTime;
		
		transform.position += translation;  //transform.Translate(translation);
	}

    private void OnEnable()
    {
		time = lifeTime;
    }

    void CheckLifeTime()
    {
		time -= Time.deltaTime;
		if (time <= 0.0f)
			projectile.DestroyMe();
	}

    void Update ()
	{
		Move();

		CheckLifeTime();
	}

}
