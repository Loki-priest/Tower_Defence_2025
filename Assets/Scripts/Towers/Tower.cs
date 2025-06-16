using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected TowerInfo myInfo;
    [SerializeField] protected Transform shootPoint;

    //можно составить композицию в виде БашняАгрегатор+ПоискЦели+Стрельба+Поворот (SRP + OCP)

    protected float lastShotTime = -0.5f;
    protected Monster currentTarget; //сюда можно Idamageable если нужны не только монстры
    protected Projectile currentProjectile;
    protected bool isNewProjectile;
    protected bool canShot = false;
    protected BaseProjectile projectile; //лучше интерфейс

    public delegate void ProjectileRequester(GameObject prefab, out Projectile projectile);
    public event ProjectileRequester OnProjectileRequested;


    protected void RequestProjectile()
    {
        OnProjectileRequested?.Invoke(myInfo.projectilePrefab, out currentProjectile);
    }

    protected void FindTarget()
    {
        Collider[] result = Physics.OverlapSphere(transform.position, myInfo.range); //как вариант для оптимизации - на башню повесить триггер коллайдер

        for (int i = 0; i < result.Length; i++)
        {
            if (currentTarget = result[i].GetComponent<Monster>())
            {
                return;
            }
        }
    }

    protected virtual void Shot() //можно кстати не стрелять если есть препятствие между башней и монстром. Но нужно рейкастить
    {
        RequestProjectile();
    }

    protected void CheckTarget()
    {
        if (currentTarget == null)
        {
            FindTarget();
        }
        else
        {
            if (!currentTarget.gameObject.activeSelf || Vector3.SqrMagnitude(transform.position - currentTarget.transform.position) > myInfo.range * myInfo.range)//если цель скрыта или далеко
                FindTarget();
        }
    }



    void Update()
    {
        if (myInfo.projectilePrefab == null || shootPoint == null)
            return;

        CheckTarget();

        if (currentTarget == null)
            return;

        FollowTarget();

        if (lastShotTime + myInfo.shootInterval < Time.time && canShot)
        {
            Shot(); //туду: не стрелять если упреждение за видимостью - не обязательно
            lastShotTime = Time.time;
        }
    }

    protected virtual void FollowTarget()
    {
        canShot = true;
    }





    private void OnDrawGizmos() //можно перенести в отдельный компонент-МоноБех
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, myInfo.range);
    }



}
