using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

interface IEntity
{
    public virtual void OnDelete(Transform caller){}
    public virtual void OnHit(Transform caller){}
}


[System.Serializable]
public class ItemDrop
{
    [Range(0, 100)]
    public int DropChance;
    public int Quantity = 1;
    public Item Item;
}

[System.Serializable]
public class EntityDrop
{
    public GameObject prefab {get{return Resources.Load<GameObject>("UI/Item");}}
    public List<ItemDrop> ItemDrops;
    
    public void Spawn(Transform caller)
    {
        foreach (var itemDrop in ItemDrops)
        {
            if (UnityEngine.Random.Range(0, 100) <= itemDrop.DropChance)
            {
                for(int i = 0; i < itemDrop.Quantity; i++)
                {
                    // add some randomness to the spawn position
                    var item = MonoBehaviour.Instantiate(prefab, caller.position, Quaternion.identity);
                    item.GetComponent<ItemInformation>().item = itemDrop.Item;
                }
            }
        }
    }
}


//[CreateAssetMenu(fileName = "Entities", menuName = "MENUNAME", order = 0)]
public class Entity : ScriptableObject, IEntity
{
    public int health;
    public EntityDrop dropPool;

    public virtual void OnDelete(Transform caller) { dropPool.Spawn(caller);Destroy(caller.gameObject);}
    public virtual void OnHit(Transform caller){}
}
