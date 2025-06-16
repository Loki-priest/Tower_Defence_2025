using UnityEngine;
using System.Collections;


public class Monster : MonoBehaviour, IDamageable
{
	//здесь move и damage можно в отдельные компоненты вынести для следования SRP


	[SerializeField] MonsterInfo myInfo;

	Transform moveTarget;
	

	float hp;

    public delegate void BotKiller(Monster m);
	public event BotKiller OnBotKilled;



	public void SetMoveTarget(Transform _target)
	{
		moveTarget = _target;
	}


	void Start()
    {
		ResetMe();
	}

	public void ResetMe()
    {
		hp = myInfo.maxHP;
		//OnBotKilled = null;
	}

	public Vector3 GetTranslationVector()
    {
		return (moveTarget.position - transform.position).normalized;
	}

	public float GetSpeed()
    {
		return myInfo.speed;
    }

	Vector3 translation;
	public void Move()
    {
		translation = moveTarget.position - transform.position;
		if (translation.magnitude > myInfo.speed)
		{
			translation = translation.normalized * myInfo.speed * Time.deltaTime;
		}
		transform.position += translation;//transform.Translate(translation);
	}

	public void DamageMe(float damage)
    {
		hp -= damage;
		if (hp <= 0.0f)
			KillMe();
    }

	public void KillMe()
    {
		OnBotKilled?.Invoke(this);
	}


	bool HasReachedTarget()
	{
		return Vector3.SqrMagnitude(transform.position - moveTarget.position) <= myInfo.reachDistance * myInfo.reachDistance;
	}

	void Update ()
	{
		if (moveTarget == null)
			return;

		if (HasReachedTarget())
		{
			KillMe();
			return;
		}

		Move();
	}

}
