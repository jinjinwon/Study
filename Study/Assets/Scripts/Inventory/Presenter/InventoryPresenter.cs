using UnityEngine;

public class InventoryPresenter
{
    private InventoryModel _model;
    private InventoryView _view;
    private EquipmentPresenter _equipmentPresenter; // EquipmentPresenter ����

    public InventoryView View { get { return _view; } }

    public InventoryPresenter(InventoryModel model, InventoryView view, EquipmentPresenter equipmentPresenter)
    {
        _model = model;
        _view = view;
        _equipmentPresenter = equipmentPresenter;

        _view.SetPresenter(this);
        RefreshView();
    }

    /// <summary>
    /// EquipmentPresenter�� �����մϴ�.
    /// </summary>
    /// <param name="equipmentPresenter">EquipmentPresenter</param>
    public void SetEquipmentPresenter(EquipmentPresenter equipmentPresenter)
    {
        _equipmentPresenter = equipmentPresenter;
    }

    /// <summary>
    /// �κ��丮�� �������� �߰��մϴ�.
    /// </summary>
    /// <param name="item">�߰��� ������</param>
    public void AddItem(InventoryItemData item)
    {
        if (_model.AddItem(item))
        {
            RefreshView();
        }
        else
        {
            _view.ShowErrorMessage("�κ��丮 ������ �����մϴ�.");
        }
    }

    /// <summary>
    /// �κ��丮���� �������� �����մϴ�.
    /// </summary>
    /// <param name="item">������ ������</param>
    public void RemoveItem(InventoryItemData item)
    {
        _model.RemoveItem(item);
        RefreshView();
    }

    /// <summary>
    /// �� �κ��丮 �������� �����մϴ�.
    /// </summary>
    /// <param name="dragItemIndex">�巡���� �������� �ε���</param>
    /// <param name="dropItemIndex">����� �������� �ε���</param>
    public void SwapItems(int dragItemIndex, int dropItemIndex)
    {
        _model.SwapItems(dragItemIndex, dropItemIndex);
        RefreshView();
    }

    /// <summary>
    /// �κ��丮 �������� �����մϴ�.
    /// </summary>
    /// <param name="type">���� ����</param>
    public void SortItems(SortType type)
    {
        _model.SortItems(type);
        RefreshView();
    }

    /// <summary>
    /// �κ��丮 -> ���â���� �������� �̵��ϰų� �����մϴ�.
    /// </summary>
    /// <param name="inventoryIndex">�κ��丮 ���� �ε���</param>
    /// <param name="equipmentIndex">���â ���� �ε���</param>
    /// <param name="equipType">���â ������ EquipType</param>
    public void MoveItemToEquipment(int inventoryIndex, int equipmentIndex, EquipType equipType)
    {
        InventoryItemData inventoryItem = _model.GetItemAt(inventoryIndex);
        if (inventoryItem == null || inventoryItem.Item == null)
            return;

        // �������� EquipType�� ������ EquipType�� ��ġ�ϴ��� Ȯ��
        if (inventoryItem.Item.EquipType != equipType)
            return;

        // ���� ������ ������ Ȯ��
        InventoryItemData equippedItem = _equipmentPresenter.GetModel().GetEquippedItem(equipType);

        // ���� ������ �������� ������ ����
        if (equippedItem.Item != null)
        {
            _model.AddItem(equippedItem);                     // ���� �������� �κ��丮�� �߰�
            _equipmentPresenter.GetModel().UnequipItem(equipType); // ���� �������� ���â���� ����
        }

        // �κ��丮 �������� �����Ͽ� ���â�� �߰�
        InventoryItemData newItem = new InventoryItemData(inventoryItem.Item, inventoryItem.Quantity);
        _equipmentPresenter.EquipItem(newItem);

        // ���� �������� �κ��丮���� ����
        _model.RemoveItem(inventoryItem);

        // �� ����
        RefreshView();
        _equipmentPresenter.RefreshView();
    }

    /// <summary>
    /// �並 ���ΰ�ħ�մϴ�.
    /// </summary>
    public void RefreshView()
    {
        _view.RefreshInventory(_model.GetItems());
    }

    /// <summary>
    /// ���� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>InventoryModel</returns>
    public InventoryModel GetModel()
    {
        return _model;
    }

    /// <summary>
    /// Ư�� �ε����� �������� �����ɴϴ�.
    /// </summary>
    /// <param name="index">������ �ε���</param>
    /// <returns>InventoryItemData</returns>
    public InventoryItemData GetItemAt(int index)
    {
        return _model.GetItemAt(index);
    }

    /// <summary>
    /// Ư�� �������� �ε����� �����ɴϴ�.
    /// </summary>
    /// <param name="item">InventoryItemData</param>
    /// <returns>������ �ε���</returns>
    public int GetIndexOfItem(InventoryItemData item)
    {
        return _model.GetIndexOfItem(item);
    }
}

