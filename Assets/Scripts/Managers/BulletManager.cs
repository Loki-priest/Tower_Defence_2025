using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : MonoBehaviour
{
    //TODO: фабрика для пуль

    [SerializeField] Transform container;
  

    [System.Serializable] 
    public class BulletPool
    {
        List<Projectile> bulletsPool;

        public List<Projectile> BulletsPool
        {
            get {
                if (bulletsPool == null)
                    bulletsPool = new();
                return bulletsPool;
            }
        }
    }

    List<Tower> towers = new();
    private readonly Dictionary<GameObject, ObjectPool<Projectile>> pools = new();


    public void ResetMe()
    {
        for(int i=0;i<towers.Count;i++)
        {
            towers[i].OnProjectileRequested -= RequestTheBullet;
        }

        towers.Clear();
        /*
        foreach (var pool in pools.Values)
        {
            pool.Clear();
        }
        pools.Clear();
        */
    }


    public void AddTower(Tower tower)
    {
        towers.Add(tower);
        tower.OnProjectileRequested += RequestTheBullet;
    }



    private void RequestTheBullet(GameObject prefab, out Projectile projectile)
    {
        if (!pools.TryGetValue(prefab, out var pool))
        {
            pool = new ObjectPool<Projectile>(
                createFunc: () =>
                {
                    var proj = Instantiate(prefab, container).GetComponent<Projectile>();
                    proj.OnProjectileDestroyed += p => pool.Release(p);
                    return proj;
                },
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Destroy(p.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 100
            );
            pools[prefab] = pool;
        }

        projectile = pool.Get();
    }



    /*
    private void DestroyProjectile(Projectile p)
    {
        p.gameObject.SetActive(false);
    }
    */

}
