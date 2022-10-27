using UnityEngine;


[CreateAssetMenu(menuName = "Entities/Enemies/Default", order = 0)]
public class Enemy : Entity
{
    public GameObject hitEffect;
    
    public virtual void Move(){}
    public virtual void Attack(){}

    public override void OnHit(Transform caller)
    {
        if(!hitEffect){return;}
        GameObject obj = Instantiate(hitEffect, caller.position, Quaternion.identity);
        Destroy(obj, 0.5f);
    }
}
