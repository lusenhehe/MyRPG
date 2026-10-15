using System;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int stackSize;
    public InventorySlot(ItemData _itemInThisSlot)
    {
        item = _itemInThisSlot;
        AddStack();
    }

    public void AddStack()
    {
        stackSize++;
    }

    public void RemoveStack()
    {
        stackSize--;
    }
}
