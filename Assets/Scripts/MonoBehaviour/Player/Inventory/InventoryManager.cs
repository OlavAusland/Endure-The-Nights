using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.VFX;
using Color = UnityEngine.Color;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public static class RectTransformExtensions
{

    public static bool Overlaps(this RectTransform a, RectTransform b)
    {
        return a.WorldRect().Overlaps(b.WorldRect());
    }
    public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
    {
        return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
    }

    public static Rect WorldRect(this RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;

        if (rectTransform.IsVertical())
            return new Rect(position.x - rectTransformHeight / 2f, position.y - rectTransformWidth / 2f,
                rectTransformHeight, rectTransformWidth);
        return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth,
            rectTransformHeight);
    }

    public static bool IsVertical(this RectTransform a)
    {
        Vector3 rotation = a.rotation.eulerAngles;
        if (Mathf.RoundToInt(rotation.z) == 90 || Mathf.RoundToInt(rotation.z) == 270)
            return true;
        return false;
    }

    public static float OverlapArea(this RectTransform a, RectTransform b)
    {
        Rect r1 = a.WorldRect();
        Rect r2 = b.WorldRect();
        
        
        float x1 = Mathf.Min(r1.xMax, r2.xMax);
        float x2 = Mathf.Max(r1.xMin, r2.xMin);
        float y1 = Mathf.Min(r1.yMax, r2.yMax);
        float y2 = Mathf.Max(r1.yMin, r2.yMin);
        float width = Mathf.Max(0.0f, x1 - x2);
        float height = Mathf.Max(0.0f, y1 - y2);

        return (width * height);
    }
}


public class InventoryManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerCombat pc;
    public PlayerManager pm;
    public PlayerEquipment pe;
    private GameObject itemPrefab {get{return Resources.Load<GameObject>("Inventory/Item");}}
    
    [Header("Equipped Items")] 
    public Item primary;
    public Item secondary;
    public List<Item> equipment;
    
    
    [Space(20)]
    [Header("Other")]
    public Transform itemTransform;
    public Vector2 dimentions;
    public float GRID_SIZE = 25;
    [SerializeField]
    InventoryItem selectedItem; 
    
    private Vector2 mouse;

    public List<InventorySlot> slots;
    public List<InventoryItem> items;

    private void Update()
    {
        mouse = Input.mousePosition;
        if (selectedItem) { MoveItem(); }
        if(Input.GetKeyDown(KeyCode.C))
            foreach (InventorySlot slot in slots)
                slot.canPlace = true;
    }

    private void MoveItem()
    {
        selectedItem.transform.position = mouse;
        List<InventorySlot> hovering = new List<InventorySlot>();

        foreach (InventorySlot slot in slots)
        {
            slot.isHovering = false;
            slot.UpdateColor();
        }

        var index = slots.FindIndex(x => selectedItem.rt.Overlaps(x.rt));
        if(index == -1){return;}
        for (int i = 0; i < selectedItem.item.size.height; i++)
        {
            for (int j = 0; j < selectedItem.item.size.width; j++)
            {
                if((index % 10) > dimentions.x - selectedItem.item.size.width){ break; }
                var position = (index + (int)(i * dimentions.x)) + j;
                if (position > slots.Count - 1)
                {
                    slots.Where(x => x.isHovering)
                        .ToList().ForEach(x =>
                        {
                            x.isHovering = false;
                            x.UpdateColor();
                        });
                    break;
                }
                InventorySlot slot = slots[position];
                if (slot.canPlace)
                {
                    hovering.Add(slot);
                    slot.isHovering = true;
                    slot.UpdateColor();
                }
            }
        }

        if (hovering.Count < selectedItem.item.size.width * selectedItem.item.size.height)
        {
            hovering.Where(x => x.canPlace).
                ToList().ForEach(x => { x.isHovering = false;x.UpdateColor();});
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            selectedItem.Rotate90();
        }else if (Input.GetKey(KeyCode.X))
            Debug.Log(CheckSpace(1, 1));
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(selectedItem != null){return;}
        if(eventData.pointerEnter)
        {
            if(eventData.pointerEnter.transform.CompareTag("InventoryItem"))
            {
                selectedItem = eventData.pointerEnter.transform.gameObject.GetComponent<InventoryItem>();
                if(selectedItem.item == primary){primary = null; pc.primary = null; pc.Weapon = null;  }

                if (equipment.Contains(selectedItem.item))
                {
                    equipment.Remove(selectedItem.item);
                    pe.Combine(equipment.ConvertAll(x => x.sprite));
                }
                
                else if(selectedItem.item == secondary){secondary = null; pc.secondary = null; pc.Weapon = null;  }
                Image image = selectedItem.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
                
                foreach (InventorySlot slot in selectedItem.slots)
                {
                    slot.isHovering = false;
                    slot.canPlace = true;
                }
                selectedItem.GetComponent<InventoryItem>().slots.Clear();
                image.raycastTarget = false;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(selectedItem == null)
            return;
        if (eventData.pointerEnter == null)
        {
            SpawnItem(selectedItem);
            Destroy(selectedItem.transform.gameObject);
            selectedItem = null;
        }
            

        Transform target = eventData.pointerEnter.transform;
        switch (eventData.pointerEnter.transform.tag)
        {
            case "PrimarySlot":
                if(selectedItem.item.itemType != ItemType.Weapon && selectedItem.item.itemType != ItemType.Tool)
                    return;
                selectedItem.transform.position = target.position;
                selectedItem.container.raycastTarget = true;
                primary = selectedItem.item;
                pc.primary = primary as Weapon;
                selectedItem = null;
                return;
            case "SecondarySlot":
                if(selectedItem.item.itemType != ItemType.Weapon && selectedItem.item.itemType != ItemType.Tool)
                    return;
                selectedItem.transform.position = target.position;
                selectedItem.container.raycastTarget = true;
                secondary = selectedItem.item;
                pc.secondary = secondary as Weapon;
                selectedItem = null;
                return;
            case "InventoryEquipment":
                if(selectedItem.item.itemType != ItemType.Equipment){return;}
                selectedItem.transform.position = target.position;
                selectedItem.container.raycastTarget = true;
                equipment.Add(selectedItem.item);
                selectedItem = null;
                pe.Combine(equipment.ConvertAll(x => x.sprite));
                return;
        }
        
        List<InventorySlot> candidateSlots = new List<InventorySlot>(slots.FindAll(x => x.isHovering && x.canPlace));
        if (candidateSlots.Count != (selectedItem.item.size.width * selectedItem.item.size.height)){return;}
        selectedItem.transform.position = candidateSlots[0].transform.position;

        selectedItem.transform.position = new Vector3(
            candidateSlots[0].transform.position.x + (selectedItem.item.size.width / 2.0f * GRID_SIZE) - (GRID_SIZE / 2.0f),
            candidateSlots[0].transform.position.y - (selectedItem.item.size.height / 2.0f * GRID_SIZE) + (GRID_SIZE / 2.0f));

        candidateSlots.ForEach(x =>
        {
            selectedItem.slots.Add(x);
            x.canPlace = false;
            x.isHovering = false;
            x.UpdateColor();    
        }
        );

        // -------------- //
        selectedItem.container.raycastTarget = true;
        selectedItem.container.color = new Color(selectedItem.container.color.r, 
            selectedItem.container.color.g, 
            selectedItem.container.color.b, 1f);
        selectedItem = null;
    }

    //FUNCTION to check if there is space for an item given (width and height) as parameters on a grid represented by a 1d array with dimentions represented by (dimentions.x, dimentions.y), return first index of the grid where the item can be placed
    public (int, List<InventorySlot>, bool) CheckSpace(int width, int height)
    {
        bool isVertical = false;
        // CHECK FOR BOTH VERTICAL AND HORIZONTAL LAYOUT
        foreach ((var horizontal, var vertical) in new List<(int, int)>() { (width, height), (height, width) })
        {
            for (int index = 0; index < slots.Count; index++)
            {
                List<InventorySlot> hovering = new List<InventorySlot>();
                for (int j = 0; j < vertical; j++)
                {
                    for (int k = 0; k < horizontal; k++)
                    {
                        if((index % 10) > dimentions.x - horizontal){break;}

                        var position = (index + (int)(j * dimentions.x)) + k;
                        if(position > slots.Count - 1){break;}

                        InventorySlot slot = slots[position];
                    
                        hovering.Add(slot);
                    }
                    if(hovering.Any(x => !x.canPlace)){break;}
                }
            
                if(hovering.Count != (horizontal * vertical) || hovering.Any(x => !x.canPlace)){continue;}
            
                hovering.ForEach(x => { Debug.Log(x.canPlace);x.UpdateColor(Color.cyan);});
                return (slots.IndexOf(hovering[0]), hovering, isVertical);
            }

            isVertical = true;
        }
        return (-1, new List<InventorySlot>(), false);
    }

    public void AddItem(Item item)
    {
        var (position, coveredSlots, vertical) = CheckSpace(item.size.width, item.size.height);

        if (position == -1)
        {
            Debug.Log("Not Space!");
            Instantiate(Resources.Load<GameObject>("UI/Item"), pc.transform.position,
                Quaternion.identity).GetComponent<ItemInformation>().item = item;
            return;
        }
        
        InventoryItem obj = Instantiate(Resources.Load<GameObject>("UI/InventoryItem"), itemTransform.position,
            Quaternion.identity, parent: itemTransform).GetComponent<InventoryItem>();
        
        obj.item = Instantiate(item);
        if(vertical){obj.Rotate90();}

        coveredSlots.ForEach(x => x.canPlace = false);
        obj.slots = coveredSlots;
        
        items.Add(obj);
        
        
        obj.transform.position = slots[position].transform.position;

        obj.transform.position = new Vector3(
            slots[position].transform.position.x + (obj.item.size.width / 2.0f * GRID_SIZE) - (GRID_SIZE / 2.0f),
            slots[position].transform.position.y - (obj.item.size.height / 2.0f * GRID_SIZE) + (GRID_SIZE / 2.0f));
    }

    private void SpawnItem(InventoryItem item)
    {
        ItemInformation obj = Instantiate(Resources.Load<GameObject>("UI/Item"), 
            pc.transform.position, Quaternion.identity).GetComponent<ItemInformation>();
        if(item.IsVertical())
            item.Rotate90();
        
        obj.item = Instantiate(item.item);
    }
    

    public void OnDrawGizmos()
    {
        
        foreach (InventorySlot slot in slots)
        {
            if(slot.canPlace)
                Gizmos.color = Color.white;
            else if (!slot.canPlace)
                Gizmos.color = Color.red;
            else if (slot.isHovering && slot.canPlace)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.white;
            Gizmos.DrawSphere(slot.transform.position, 5f);
        }
    }
}
