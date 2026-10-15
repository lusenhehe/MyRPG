using UnityEngine.EventSystems;

public class EquippedEquipmentSlot_UI : InventorySlot_UI
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = equipmentType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (inventorySlot == null || inventorySlot.item == null)
        {
            return;
        }

        InventoryManager.instance.UnequipEquipmentWithoutAddingBackToInventory(inventorySlot.item as ItemData_Equipment);
        InventoryManager.instance.AddItem(inventorySlot.item as ItemData_Equipment);
        CleanUpInventorySlotUI();  //redundant cuz UpdateAllSlotUI() is already called in AddItem()

        ui.itemToolTip.HideToolTip();
    }
}
