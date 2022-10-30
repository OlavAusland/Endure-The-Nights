using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Weapon,
    Consumable,
    Equipment,
    Resource,
    Tool,
    Any
}

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    public Rarity rarity;
    public ItemType itemType;
    public float uiRotation;
    public ItemSize size;
    public virtual bool IsStackable { get { return false; } }

    public string name;
    public Sprite sprite;
}

/*
 * Change a stackable item to be another class that inherits from Item
 */