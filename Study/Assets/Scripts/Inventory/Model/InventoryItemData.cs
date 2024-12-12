using System;

public class InventoryItemData
{
    public ItemData Item { get; set; }
    public int Quantity { get; set; }

    public Guid UniqueID { get; set; }

    public InventoryItemData(ItemData item, int quantity)
    {
        Item = item;
        Quantity = quantity;
        UniqueID = item == null ? Guid.Empty : Guid.NewGuid();
    }

    public InventoryItemData Clear()
    {
        Item = null;
        Quantity = 0;
        UniqueID = Guid.Empty;

        return this;
    }
}