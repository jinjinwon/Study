using System;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class InventoryModel
{
    private List<InventoryItemData> _items;
    private int _maxSlots; // �ִ� ���� ��

    public InventoryModel(int maxSlots = 20) // �⺻�� 20���� ����
    {
        _maxSlots = maxSlots;
        _items = new List<InventoryItemData>();

        // �� ���� �ʱ�ȭ
        for (int i = 0; i < _maxSlots; i++)
        {
            // �ʱ� ���´� �������� ���� �� ���� ����
            _items.Add(new InventoryItemData(null, 0));
        }
    }

    /// <summary>
    /// ���� �κ��丮 ������ ����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    public List<InventoryItemData> GetItems()
    {
        return _items;
    }

    /// <summary>
    /// �ִ� ���� ���� ��ȯ�մϴ�.
    /// </summary>
    public int GetMaxSlots()
    {
        return _maxSlots;
    }

    /// <summary>
    /// �ִ� ���� ���� �����մϴ�.
    /// </summary>
    /// <param name="maxSlots">������ �ִ� ���� ��</param>
    public void SetMaxSlots(int maxSlots)
    {
        _maxSlots = maxSlots;
        // �ʿ信 ���� ���� ���� �پ�� ��� �������� �����ϰų� �����ϴ� ���� �߰� ����
    }

    /// <summary>
    /// ���ο� �������� �κ��丮�� �߰��մϴ�.
    /// </summary>
    /// <param name="newItem">�߰��� ������</param>
    /// <returns>�߰� ���� ����</returns>
    public bool AddItem(InventoryItemData newItem)
    {
        if (newItem.Item == null)
            return false;

        // ���� ������ �������� ��� ���� ���ÿ� �߰�
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

        // �� ���� ã��
        for (int i = 0; i < _maxSlots; i++)
        {
            if (_items[i].Item == null)
            {
                _items[i] = newItem;
                return true;
            }
        }

        // ��� ������ �� �� ���
        return false;
    }

    /// <summary>
    /// �������� �κ��丮���� �����մϴ�.
    /// </summary>
    /// <param name="item">������ ������</param>
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
    /// �� �ε����� �������� �����մϴ�.
    /// </summary>
    /// <param name="indexA">ù ��° �ε���</param>
    /// <param name="indexB">�� ��° �ε���</param>
    public void SwapItems(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= _maxSlots || indexB < 0 || indexB >= _maxSlots)
            return;

        var temp = _items[indexA];
        _items[indexA] = _items[indexB];
        _items[indexB] = temp;
    }

    /// <summary>
    /// �κ��丮 �������� �����մϴ�.
    /// </summary>
    /// <param name="sortType">���� ����</param>
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
                    return b.Quantity.CompareTo(a.Quantity); // �������� ����
                });
                break;
                // �߰����� ���� ����...
        }
    }

    /// <summary>
    /// Ư�� �ε����� �������� �����ɴϴ�.
    /// </summary>
    /// <param name="index">������ �ε���</param>
    /// <returns>InventoryItemData</returns>
    public InventoryItemData GetItemAt(int index)
    {
        if (index >= 0 && index < _maxSlots)
            return _items[index];
        return null;
    }

    /// <summary>
    /// Ư�� �������� �ε����� �����ɴϴ�.
    /// </summary>
    /// <param name="item">InventoryItemData</param>
    /// <returns>������ �ε���</returns>
    public int GetIndexOfItem(InventoryItemData item)
    {
        return _items.IndexOf(item);
    }
}