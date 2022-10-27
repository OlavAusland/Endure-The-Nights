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

    public string name;
    public Sprite sprite;
}