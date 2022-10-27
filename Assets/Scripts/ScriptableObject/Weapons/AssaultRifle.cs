using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/AssaultRifle", order = 0)]
public class AssaultRifle : Weapon
{
    public DefaultProjectile projectile;
    public float offset;
    public AudioClip sound;

    public override void PrimaryAttack(PlayerCombat caller)
    {
        Vector2 dir = Direction(caller.weaponTransform);
        
        GameObject obj = Instantiate(projectile.prefab, 
            caller.weaponTransform.position + (Vector3)(dir * offset), Quaternion.identity);
        Rigidbody2D rb2D = obj.GetComponent<Rigidbody2D>();
        float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
        obj.GetComponent<SpriteRenderer>().sprite = projectile.sprite;
        if(sound){caller.AudioSource.PlayOneShot(sound);}
        
        rb2D.velocity = dir * projectile.magnitude;
    }

    public override void SecondaryAttack(PlayerCombat caller)
    {
        Debug.Log("Attack from AR");
    }
}
