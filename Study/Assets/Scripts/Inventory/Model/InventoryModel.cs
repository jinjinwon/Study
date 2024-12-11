using System;
using System.Collections.Generic;
using static UnityEditor.Progress;

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

public class InventoryModel
{
    public List<InventoryItemData> Items { get; private set; } = new List<InventoryItemData>();
    public int MaxSlots { get; private set; } = 20;

    public InventoryModel(int maxSlots)
    {
        MaxSlots = maxSlots;

        // �� ���� �ʱ�ȭ
        for (int i = 0; i < maxSlots; i++)
        {
            // �ʱ� ���´� �������� ���� �� ���� ����
            Items.Add(new InventoryItemData(null, 0));
        }
    }

    // �κ��丮 ������ �߰� �Լ�
    public bool AddItem(InventoryItemData item)
    {
        if (item.Item.IsStackable)
        {
            // ��ø ������ ������ ã��
            var existingItem = Items.Find(entry => entry.Item == item.Item && entry.Quantity < entry.Item.MaxStackSize);
            if (existingItem != null && existingItem.Item != null)
            {
                int newQuantity = existingItem.Quantity + item.Quantity;

                if (newQuantity > item.Item.MaxStackSize)
                {
                    int remainingQuantity = newQuantity - item.Item.MaxStackSize;
                    existingItem.Quantity = item.Item.MaxStackSize;

                    // �� ���Կ� �ʰ��� �߰�
                    var emptySlot = Items.Find(entry => entry.Item == null);
                    if (emptySlot != null)
                    {
                        emptySlot.Item = item.Item;
                        emptySlot.Quantity = remainingQuantity;
                        emptySlot.UniqueID = Guid.NewGuid();
                        return true;
                    }
                    return false;
                }

                existingItem.Quantity = newQuantity;
                return true;
            }
            else
            {
                // ��ø ������ ������������ �κ��丮�� ���� ���
                var emptySlot = Items.Find(entry => entry.Item == null);
                if (emptySlot != null)
                {
                    emptySlot.Item = item.Item;
                    emptySlot.Quantity = item.Quantity;
                    emptySlot.UniqueID = Guid.NewGuid();
                    return true;
                }
            }
        }

        // �� ���� ã��
        var slot = Items.Find(entry => entry.Item == null);
        if (slot != null)
        {
            slot.Item = item.Item;
            slot.Quantity = item.Quantity;
            slot.UniqueID = Guid.NewGuid();
            return true;
        }

        return false;
    }

    // �κ��丮 ������ ���� �Լ�
    public void RemoveItem(InventoryItemData item)
    {
        var existingItem = Items.Find(entry => entry.UniqueID == item.UniqueID);

        if (existingItem != null && existingItem.Item != null)
        {
            if (existingItem.Quantity > item.Quantity)
            {
                existingItem.Quantity -= item.Quantity;
            }
            else
            {
                existingItem.Clear();
            }
        }
    }

    // Drag Drop���� ���� ������ ��ġ ���� �Լ�
    public void SwapItems(int dragItemIndex, int dropItemIndex)
    {
        if (dragItemIndex >= 0 && dragItemIndex < Items.Count &&
            dropItemIndex >= 0 && dropItemIndex < Items.Count)
        {
            var temp = Items[dropItemIndex];
            Items[dropItemIndex] = Items[dragItemIndex];
            Items[dragItemIndex] = temp;
        }
    }

    public void SortItems(SortType type)
    {
        int quantityComparison = 0;

        Items.Sort((a, b) =>
        {
            // �� ���� ó��: �� ������ �׻� �ڷ�
            if (a.Item == null && b.Item == null) return 0;
            if (a.Item == null) return 1;
            if (b.Item == null) return -1;

            switch (type)
            {
                case SortType.ByName:
                    // �̸� �� ����
                    return string.Compare(a.Item.ItemName, b.Item.ItemName, StringComparison.Ordinal);

                case SortType.ByQuantity:
                    // ���� �� ����
                    quantityComparison = b.Quantity.CompareTo(a.Quantity);
                    if (quantityComparison == 0)
                    {
                        return string.Compare(a.Item.ItemName, b.Item.ItemName, StringComparison.Ordinal);
                    }
                    return quantityComparison;

                case SortType.ByItemType:
                    // Ÿ�� �� ����
                    quantityComparison = a.Item.ItemType.CompareTo(b.Item.ItemType);
                    if (quantityComparison == 0)
                    {
                        return string.Compare(a.Item.ItemName, b.Item.ItemName, StringComparison.Ordinal);
                    }
                    return quantityComparison;

                default:
                    return 0;
            }
        });
    }

    // Items ����Ʈ ��ȯ
    public List<InventoryItemData> GetItems()
    {
        return Items;
    }
}