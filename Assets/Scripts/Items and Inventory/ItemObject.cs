using UnityEngine;
public class ItemObject : MapElement
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData item;
    private void FixedUpdate()
    {
        if (gameObject.transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }
    protected override void Start()
    {
        SetupItemIconAndName();
        base.Start();
    }

    public void SetupItemDrop(ItemData _item, Vector2 _dropVelocity)
    {
        item = _item;
        rb.velocity = _dropVelocity;

        SetupItemIconAndName();
    }
    public void PickupItem()
    {
        if (!InventoryManager.instance.CanAddEquipmentToInventory() && item.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 5);
            if (LanguageManager.instance.localeID == 0)
            {
                GameManager.instance.consule_UI.AddDebug("No more space in inventory!");
            }
            else if (LanguageManager.instance.localeID == 1)
            {
                GameManager.instance.consule_UI.AddDebug("<玩家>:没有多余的空间了!");
            }
            return;
        }
        InventoryManager.instance.AddItem(item);
        AudioManager.instance.PlaySFX(18, transform);
        GameManager.instance.UsedMapElementIDList.Add(mapElementID);
        GameManager.instance.consule_UI.AddDebug($"<玩家>:捡起了 {item.itemName}");
        Destroy(gameObject);
    }
    private void SetupItemIconAndName()
    {
        if (item == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = item.icon;
        gameObject.name = $"物品 - {item.name}";
    }
    private void OnValidate()
    {
        if(item == null)
            return;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = item.icon;
            gameObject.name = $"物品 - {item.name}";
    }
}