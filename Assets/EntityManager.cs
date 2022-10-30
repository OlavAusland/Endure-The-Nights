using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public enum EntityType
{
    Enemy,
    Tree,
    Stone
}

public class EntityManager : MonoBehaviour
{
    public Entity entity;
    public EntityType type;
    private int _health = 100;
    public int Health
    {
        get { return _health; }
        set
        {
            Utilities.Health.TakeDamage(transform, Health - value);
            entity.OnHit(transform); 
            _health = value; 
            Debug.Log($"{Health}, {_health}, {value}");
            if(_health <= 0){entity.OnDelete(transform);}
        }
    }

    public void Start() { entity = Instantiate(entity); _health = entity.health; }
}
