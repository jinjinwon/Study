using System;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class InventoryModel
{
    private List<InventoryItemData> _items;
    private int _maxSlots; // 최대 슬롯 수

    public InventoryModel(int maxSlots = 20) // 기본값 20으로 설정
    {
        _maxSlots = maxSlots;
        _items = new List<InventoryItemData>();

        // 빈 슬롯 초기화
        for (int i = 0; i < _maxSlots; i++)
        {
            // 초기 상태는 아이템이 없는 빈 슬롯 상태
            _items.Add(new InventoryItemData(null, 0));
        }
    }

    /// <summary>
    /// 현재 인벤토리 아이템 리스트를 반환합니다.
    /// </summary>
    public List<InventoryItemData> GetItems()
    {
        return _items;
    }

    /// <summary>
    /// 최대 슬롯 수를 반환합니다.
    /// </summary>
    public int GetMaxSlots()
    {
        return _maxSlots;
    }

    /// <summary>
    /// 최대 슬롯 수를 설정합니다.
    /// </summary>
    /// <param name="maxSlots">설정할 최대 슬롯 수</param>
    public void SetMaxSlots(int maxSlots)
    {
        _maxSlots = maxSlots;
        // 필요에 따라 슬롯 수가 줄어들 경우 아이템을 제거하거나 조정하는 로직 추가 가능
    }

    /// <summary>
    /// 새로운 아이템을 인벤토리에 추가합니다.
    /// </summary>
    /// <param name="newItem">추가할 아이템</param>
    /// <returns>추가 성공 여부</returns>
    public bool AddItem(InventoryItemData newItem)
    {
        if (newItem.Item == null)
            return false;

        // 스택 가능한 아이템인 경우 기존 스택에 추가
        if (newItem.Item.IsStackable)
        {
            foreach (var item in _items)
            {
                if (item.Item == newItem.Item && item.Quantity < item.Item.MaxStackSize)
                {
                    int availableSpace = item.Item.MaxStackSize - item.Quantity;
                    if (availableSpace >= newItem.Quantity)
                    {
                        item.Quantity += newItem.Quantity;
                        return true;
                    }
                    else
                    {
                        item.Quantity += availableSpace;
                        newItem.Quantity -= availableSpace;
                    }
                }
            }
        }

        // 빈 슬롯 찾기
        for (int i = 0; i < _maxSlots; i++)
        {
            if (_items[i].Item == null)
            {
                _items[i] = newItem;
                return true;
            }
        }

        // 모든 슬롯이 꽉 찬 경우
        return false;
    }

    /// <summary>
    /// 아이템을 인벤토리에서 제거합니다.
    /// </summary>
    /// <param name="item">제거할 아이템</param>
    public void RemoveItem(InventoryItemData item)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == item)
            {
                _items[i].Clear();
                break;
            }
        }
    }

    /// <summary>
    /// 두 인덱스의 아이템을 스왑합니다.
    /// </summary>
    /// <param name="indexA">첫 번째 인덱스</param>
    /// <param name="indexB">두 번째 인덱스</param>
    public void SwapItems(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= _maxSlots || indexB < 0 || indexB >= _maxSlots)
            return;

        var temp = _items[indexA];
        _items[indexA] = _items[indexB];
        _items[indexB] = temp;
    }

    /// <summary>
    /// 인벤토리 아이템을 정렬합니다.
    /// </summary>
    /// <param name="sortType">정렬 기준</param>
    public void SortItems(SortType sortType)
    {
        switch (sortType)
        {
            case SortType.ByName:
                _items.Sort((a, b) =>
                {
                    if (a.Item == null) return 1;
                    if (b.Item == null) return -1;
                    return a.Item.ItemName.CompareTo(b.Item.ItemName);
                });
                break;
            case SortType.ByItemType:
                _items.Sort((a, b) =>
                {
                    if (a.Item == null) return 1;
                    if (b.Item == null) return -1;
                    return a.Item.ItemType.CompareTo(b.Item.ItemType);
                });
                break;
            case SortType.ByQuantity:
                _items.Sort((a, b) =>
                {
                    if (a.Item == null && b.Item == null) return 0;
                    if (a.Item == null) return 1;
                    if (b.Item == null) return -1;
                    return b.Quantity.CompareTo(a.Quantity); // 내림차순 정렬
                });
                break;
                // 추가적인 정렬 기준...
        }
    }

    /// <summary>
    /// 특정 인덱스의 아이템을 가져옵니다.
    /// </summary>
    /// <param name="index">아이템 인덱스</param>
    /// <returns>InventoryItemData</returns>
    public InventoryItemData GetItemAt(int index)
    {
        if (index >= 0 && index < _maxSlots)
            return _items[index];
        return null;
    }

    /// <summary>
    /// 특정 아이템의 인덱스를 가져옵니다.
    /// </summary>
    /// <param name="item">InventoryItemData</param>
    /// <returns>아이템 인덱스</returns>
    public int GetIndexOfItem(InventoryItemData item)
    {
        return _items.IndexOf(item);
    }
}