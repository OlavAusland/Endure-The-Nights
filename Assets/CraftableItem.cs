using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class CraftableItem : MonoBehaviour
{
    public CraftingManager craftingManager;

    public Image icon;
    public TMP_Text itemsNeeded;
    public Recipe recipe;
    
    public void Start()
    {
        icon.sprite = recipe.output.sprite;
        itemsNeeded.text = "";
        foreach(var item in recipe.items)
        {
            itemsNeeded.text +=
                $"{craftingManager.inventoryManager.GetItemQuantity(item.item)}/{item.quantity}x {item.item.name}\n";
        }
    }


    public void UpdateText()
    {
        itemsNeeded.text = "";
        foreach(var item in recipe.items)
        {
            itemsNeeded.text +=
                $"{craftingManager.inventoryManager.GetItemQuantity(item.item)}/{item.quantity}x {item.item.name}\n";
        }
    }
    
    // FIx this sucky sucky function
    public void CraftItem()
    {
        if(!CanCraft()){return;}
        
        foreach(RecipeItem item in recipe.items)
        {
            if (!item.item.IsStackable)
            {
                var inventoryItem = craftingManager.inventoryManager.items.
                    Find(x => x.item.name == item.item.name);
                
                craftingManager.inventoryManager.items.Remove(inventoryItem);
                inventoryItem.slots.ForEach(x =>
                {
                    x.canPlace = true;
                    x.UpdateColor();
                });
                Destroy(inventoryItem.transform.gameObject);
            }
            else
            {
                List<InventoryItem> inventoryItems = craftingManager.inventoryManager.items
                    .FindAll(x => x.item.name == item.item.name).ToList();
                inventoryItems.
                    Sort((a, b) =>
                    {
                        return (a.item as StackableItem).amount.CompareTo((b.item as StackableItem).amount);
                    });
                var count = item.quantity;
                //dangerous, might cause infinite loop
                while (count > 0 && inventoryItems.Count > 0)
                {
                    foreach (InventoryItem invItem in inventoryItems)
                    {
                        var stackableItem = invItem.item as StackableItem;
                        if (stackableItem.amount <= count)
                        {
                            count -= stackableItem.amount;
                            invItem.slots.ForEach(x =>
                            {
                                x.canPlace = true;
                                x.UpdateColor();
                            });
                            craftingManager.inventoryManager.items.Remove(invItem);
                            Destroy(invItem.transform.gameObject);
                        }
                        else
                        {
                            stackableItem.amount--;
                            count--;
                            invItem.count.text = stackableItem.amount.ToString();
                        }
                    }
                }
            }
        }
        
        craftingManager.inventoryManager.AddItem(recipe.output);
        craftingManager.UpdateText();
    }

    private bool CanCraft()
    {
        foreach(var item in recipe.items)
        {
            if (craftingManager.inventoryManager.GetItemQuantity(item.item) < item.quantity)
            {
                return false;
            }
        }
        return true;
    }
}
