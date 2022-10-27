using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

interface IProjectile
{
    public virtual void Activate() {}
    public virtual void Destroy() {}
}

public class Projectile : ScriptableObject, IProjectile
{
    public int damage;
    public Sprite sprite;

    public virtual void Activate(Transform hit)
    {
        Debug.Log("Activated");
        hit.GetComponent<EntityManager>().Health -= damage;
    }

    public virtual void Destroy(ProjectileManager pm)
    {
        Debug.Log("Destroyed");
        Destroy(pm.transform.gameObject);
    }
}
