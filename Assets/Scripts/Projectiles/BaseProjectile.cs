using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected Projectile projectile;
    protected Vector3 translation;
    public Projectile Projectile
    {
        get { return projectile; }
    }

    protected virtual void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    public abstract void Move();

    public virtual void SetTarget(GameObject go)
    {

    }
}
