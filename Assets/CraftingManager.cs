using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    GameObject item {get{return Resources.Load<GameObject>("UI/CraftingItem");}}

    public List<Recipe> recipes = new List<Recipe>();
    public Transform craftingPanel;

    private void Start()
    {
        recipes = Resources.LoadAll<Recipe>("Recipes").ToList();

        foreach (Recipe recipe in recipes)
        {
            GameObject obj = Instantiate(item, craftingPanel.position, Quaternion.identity, craftingPanel);
            obj.GetComponent<CraftableItem>().recipe = recipe;
            obj.GetComponent<CraftableItem>().craftingManager = this;
        }
    }

    public void UpdateText()
    {
        foreach(CraftableItem craftableItem in craftingPanel.GetComponentsInChildren<CraftableItem>())
        {
            craftableItem.UpdateText();
        }
    }
}
