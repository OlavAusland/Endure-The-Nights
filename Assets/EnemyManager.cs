using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(EntityManager))]
public class EnemyManager : MonoBehaviour
{
    public EntityManager entityManager;
    public Transform target;
    public Enemy enemy;

    private void Start() {enemy = Instantiate(enemy); enemy.Initialize(transform);}

    public void Update()
    {
        enemy.Move(transform, target);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Player")){return;}
        target = col.transform;
    }

    public void OnTriggerExit2D(Collider2D other){ target = null; }
}
