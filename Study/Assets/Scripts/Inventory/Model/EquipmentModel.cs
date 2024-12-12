using System.Collections.Generic;
using UnityEngine;

public class EquipmentModel
{
    private Dictionary<EquipType, InventoryItemData> _equippedItems;

    public EquipmentModel()
    {
        _equippedItems = new Dictionary<EquipType, InventoryItemData>();

        foreach (EquipType equipType in System.Enum.GetValues(typeof(EquipType)))
        {
            if (equipType != EquipType.None)
                _equippedItems[equipType] = new InventoryItemData(null,0);
        }
    }

    // ����
    public bool EquipItem(InventoryItemData item)
    {
        if (item == null || item.Item == null || item.Item.EquipType == EquipType.None)
            return false;

        EquipType equipType = item.Item.EquipType;

        if (_equippedItems.ContainsKey(equipType))
        {
            _equippedItems[equipType] = item;
            return true;
        }

        return false;
    }

    // ���� ����
    public InventoryItemData UnequipItem(EquipType equipType)
    {
        if (_equippedItems.ContainsKey(equipType))
        {
            InventoryItemData unequippedItem = _equippedItems[equipType];
            _equippedItems[equipType] = new InventoryItemData(null, 0); // �� �������� �ʱ�ȭ
            return unequippedItem;
        }

        return null;
    }

    // ���� ���� ������ ��������
    public InventoryItemData GetEquippedItem(EquipType equipType)
    {
        if (_equippedItems.ContainsKey(equipType))
            return _equippedItems[equipType];
        return new InventoryItemData(null, 0);
    }

    // ��� ���� ���� ������ ��������
    public Dictionary<EquipType, InventoryItemData> GetEquippedItems()
    {
        return _equippedItems;
    }

    // �ʱ�ȭ
    public void ClearAllEquippedItems()
    {
        foreach (var key in _equippedItems.Keys)
        {
            _equippedItems[key] = null;
        }
    }
}
