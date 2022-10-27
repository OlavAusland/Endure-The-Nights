using System;
using UnityEngine;

interface IEntity
{
    public virtual void OnDelete(Transform caller){}
    public virtual void OnHit(Transform caller){}
}

//[CreateAssetMenu(fileName = "Entities", menuName = "MENUNAME", order = 0)]
public class Entity : ScriptableObject, IEntity
{
    public int health;

    public virtual void OnDelete(Transform caller) { }
    public virtual void OnHit(Transform caller){}
}
