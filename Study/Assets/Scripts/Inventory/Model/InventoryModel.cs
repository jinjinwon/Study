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

        // 빈 슬롯 초기화
        for (int i = 0; i < maxSlots; i++)
        {
            // 초기 상태는 아이템이 없는 빈 슬롯 상태
            Items.Add(new InventoryItemData(null, 0));
        }
    }

    // 인벤토리 아이템 추가 함수
    public bool AddItem(InventoryItemData item)
    {
        if (item.Item.IsStackable)
        {
            // 중첩 가능한 아이템 찾기
            var existingItem = Items.Find(entry => entry.Item == item.Item && entry.Quantity < entry.Item.MaxStackSize);
            if (existingItem != null && existingItem.Item != null)
            {
                int newQuantity = existingItem.Quantity + item.Quantity;

                if (newQuantity > item.Item.MaxStackSize)
                {
                    int remainingQuantity = newQuantity - item.Item.MaxStackSize;
                    existingItem.Quantity = item.Item.MaxStackSize;

                    // 빈 슬롯에 초과분 추가
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
                // 중첩 가능한 아이템이지만 인벤토리에 없는 경우
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

        // 빈 슬롯 찾기
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

    // 인벤토리 아이템 삭제 함수
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

    // Drag Drop으로 인한 아이템 위치 변경 함수
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
            // 빈 슬롯 처리: 빈 슬롯은 항상 뒤로
            if (a.Item == null && b.Item == null) return 0;
            if (a.Item == null) return 1;
            if (b.Item == null) return -1;

            switch (type)
            {
                case SortType.ByName:
                    // 이름 순 정렬
                    return string.Compare(a.Item.ItemName, b.Item.ItemName, StringComparison.Ordinal);

                case SortType.ByQuantity:
                    // 수량 순 정렬
                    quantityComparison = b.Quantity.CompareTo(a.Quantity);
                    if (quantityComparison == 0)
                    {
                        return string.Compare(a.Item.ItemName, b.Item.ItemName, StringComparison.Ordinal);
                    }
                    return quantityComparison;

                case SortType.ByItemType:
                    // 타입 순 정렬
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

    // Items 리스트 반환
    public List<InventoryItemData> GetItems()
    {
        return Items;
    }
}