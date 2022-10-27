using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Projectile projectile;
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Entity")){return;}
        
        projectile.Activate(col.transform);
        projectile.Destroy(this);
    }
}
