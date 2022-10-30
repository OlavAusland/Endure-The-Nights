using UnityEngine;


[CreateAssetMenu(menuName = "Entities/Enemies/Default", order = 0)]
public class Enemy : Entity
{
    public float speed;
    public Vector2 velocity;
    public Rigidbody2D rb2D;
    public SpriteRenderer sr;
    public GameObject hitEffect;

    public virtual void Initialize(Transform caller)
    {
        rb2D = caller.GetComponent<Rigidbody2D>();
        sr = caller.GetComponent<SpriteRenderer>();

    }
    
    public virtual void Move(Transform caller, Transform target){}
    public virtual void Attack(){}  

    public override void OnHit(Transform caller)
    {
        if(!hitEffect){return;}
        GameObject obj = Instantiate(hitEffect, caller.position, Quaternion.identity);
        Destroy(obj, 0.5f);
    }
}
