using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Image image { get { return GetComponent<Image>(); } }
    public bool isHovering;
    public bool canPlace;
    public Item item;
    public RectTransform rt { get { return GetComponent<RectTransform>(); } }
    
    public void UpdateColor()
    {
        if (isHovering)
            if (canPlace)
                image.color = new Color(100f/255.0f, 142/255.0f, 106/255.0f, 1f);
            else
                image.color = Color.red;
        else
            image.color = new Color(0.34f, 0.34f, 0.34f, 1);
    }

    public void UpdateColor(Color color){ image.color = color; }
}
