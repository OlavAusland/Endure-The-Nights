using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "Tree", menuName = "Entities/Environment/Tree", order = 0)]
public class Tree : Entity
{
    public GameObject hitEffect;
    
    public override void OnHit(Transform caller)
    {
        GameObject obj = Instantiate<GameObject>(hitEffect, caller.position, Quaternion.identity);
        Destroy(obj, 0.5f);
    }
}
