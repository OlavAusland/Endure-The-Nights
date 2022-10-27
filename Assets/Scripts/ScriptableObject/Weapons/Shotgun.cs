using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shotgun", order = 0)]
public class Shotgun : Weapon
{
    public GameObject projectile;
    public float arc;
    public int pellets;
    public float offset;

    public AudioClip sound;

    public override void PrimaryAttack(PlayerCombat caller)
    {
        var dir = Direction(caller.weaponTransform);

        for (float i = 0, angle = pellets > 1 ? -arc : 0; i < pellets; i++)
        {
            
            var obj = Instantiate(projectile, position: caller.weaponTransform.position + (Vector3)dir * offset, Quaternion.identity);
            Rigidbody2D rb2D = obj.GetComponent<Rigidbody2D>();

            float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            obj.transform.rotation = Quaternion.AngleAxis(rotation - angle, Vector3.forward);
            rb2D.velocity = obj.transform.right * 10;
            angle += (arc / (pellets / 2));
        }
        
        caller.AudioSource.PlayOneShot(sound);
    }
}
