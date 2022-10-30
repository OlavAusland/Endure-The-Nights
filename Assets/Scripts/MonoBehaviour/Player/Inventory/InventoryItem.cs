using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ItemSize
{
    public int width;
    public int height;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legendary,
    Mythical
}

public class InventoryItem : MonoBehaviour
{
    public RectTransform rt { get { return GetComponent<RectTransform>(); } }
    public Image container {get {return GetComponent<Image>();}}
    public Image image;
    public Item item;

    public List<InventorySlot> slots;
    public TMP_Text count;

    private void Start()
    {
        if(item.IsStackable){count.enabled = true;}
        
        rt.sizeDelta = new Vector2(item.size.width, item.size.height) * 25; //change to dynamically fit inventory size
        count.rectTransform.sizeDelta = new Vector2(item.size.width, item.size.height) * 25;
        image.sprite = item.sprite;
        if(!IsVertical())
            image.rectTransform.eulerAngles = new Vector3(0, 0, item.uiRotation);

        switch (item.rarity)
        {
            case Rarity.Uncommon:
                container.color = new Color(100/255.0f, 1, 100/255.0f);
                break;
            case Rarity.Rare:
                container.color = new Color(100/255.0f, 1, 1);
                break;
            case Rarity.Legendary:
                container.color = new Color(1, 200/255.0f, 100/255.0f);
                break;
            case Rarity.Mythical:
                container.color = new Color(1, 0, 100/255.0f);
                break;
            case Rarity.Common:
            default:
                container.color = new Color(125/255.0f, 125/255.0f, 125/255.0f);
                break;
        }
    }

    /// <summary>
    /// Rotates an inventory item's size by 90 degrees.
    /// </summary>
    /// <returns>void</returns>
    public void Rotate90()
    {
        rt.Rotate(0, 0, 90);
        
        (item.size.width, item.size.height) = (item.size.height, item.size.width);
        count.rectTransform.Rotate(0, 0 , -90);
        count.rectTransform.sizeDelta = new Vector2(item.size.width, item.size.height) * 25;
    }
    

    /// <summary>
    /// Checks if the inventory item is rotated to be vertical
    /// </summary>
    /// <returns>Bool</returns>
    public bool IsVertical()
    {
        if (item.size.height > item.size.width)
            return true;
        return false;
        //        return Mathf.RoundToInt(rt.rotation.eulerAngles.z) == 90 || Mathf.RoundToInt(rt.rotation.eulerAngles.z) == -270;
    }

}