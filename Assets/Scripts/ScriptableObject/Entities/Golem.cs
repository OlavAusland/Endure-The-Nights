using UnityEngine;

[CreateAssetMenu(menuName = "Entities/Enemies/GolemBehaviour")]
public class Golem : Enemy
{
    public override void Move(Transform caller, Transform target)
    {
        if(target == null){velocity = Vector2.zero; return;}

        if (Vector2.Distance(caller.position, target.position) > 1f)
        {
            velocity = (target.position - caller.position).normalized * speed;
            if(velocity.x > 0 && sr.flipX){sr.flipX = false;}
            else if(velocity.x < 0 && !sr.flipX){sr.flipX = true;}
        }
        else velocity = Vector2.zero;
        rb2D.MovePosition(caller.position + (Vector3)velocity);
    }
}
