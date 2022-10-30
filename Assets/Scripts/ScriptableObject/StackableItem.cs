using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stackable Item", menuName = "Stackable Item")]
public class StackableItem : Item
{
    public int maxStackSize = 64;
    public int amount = 1;
    [SerializeField]
    public override bool IsStackable { get { return true; } }

}