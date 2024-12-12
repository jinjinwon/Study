using UnityEngine;

public class EquipmentPresenter
{
    private EquipmentModel _model;
    private EquipmentView _view;
    private InventoryPresenter _inventoryPresenter; // InventoryPresenter ����

    public EquipmentPresenter(EquipmentModel model, EquipmentView view, InventoryPresenter inventoryPresenter)
    {
        _model = model;
        _view = view;
        _inventoryPresenter = inventoryPresenter;

        _view.SetPresenter(this);
        RefreshView();
    }

    // View ���� ��û
    public void RefreshView()
    {
        _view.RefreshEquipment(_model.GetEquippedItems());
    }

    // ���� �Լ�
    public bool EquipItem(InventoryItemData item)
    {
        if (_model.EquipItem(item))
        {
            return true;
        }
        return false;
    }

    // ���� ���� �Լ�
    public void UnequipItem(EquipType equipType)
    {
        InventoryItemData unequippedItem = _model.UnequipItem(equipType);
        if (unequippedItem == null)
        {
            _view.ShowErrorMessage("������ ��� �����ϴ�.");
            return;
        }

        // �κ��丮�� �������� �߰�
        if (!_inventoryPresenter.GetModel().AddItem(unequippedItem))
        {
            _view.ShowErrorMessage("�κ��丮 ������ �����Ͽ� ��� ������ �� �����ϴ�.");
            // ���â�� �ٽ� ����
            _model.EquipItem(unequippedItem);
            RefreshView();
        }
        else
        {
            _inventoryPresenter.RefreshView();
            RefreshView();
        }
    }

    // ��� ���� <-> ��� ���� ���� �Լ�
    public void SwapItems(int equipmentIndex, int otherEquipmentIndex, EquipType equipType)
    {
        // ��� ���� �ε����� EquipType���� ��ȯ
        EquipType firstEquipType = (EquipType)equipmentIndex;
        EquipType secondEquipType = (EquipType)otherEquipmentIndex;

        InventoryItemData firstItem = _model.GetEquippedItem(firstEquipType);
        InventoryItemData secondItem = _model.GetEquippedItem(secondEquipType);

        if (firstItem == null || secondItem == null)
        {
            _view.ShowErrorMessage("������ ��� ������� �ʽ��ϴ�.");
            return;
        }

        // ����
        _model.UnequipItem(firstEquipType);
        _model.UnequipItem(secondEquipType);

        _model.EquipItem(secondItem);
        _model.EquipItem(firstItem);

        RefreshView();
    }

    // Model ��ȯ �Լ�
    public EquipmentModel GetModel()
    {
        return _model;
    }

    // ���â -> �κ��丮 �̵� �Լ�
    public void MoveItemToInventory(EquipType type, int inventoryIndex)
    {
        // EquipType�� ��� ���� �ε����� ����
        // EquipType equipType = (EquipType)equipmentIndex;

        InventoryItemData equippedItem = _model.GetEquippedItem(type);
        if (equippedItem == null)
        {
            _view.ShowErrorMessage("������ ��� �����ϴ�.");
            return;
        }

        if (_inventoryPresenter.GetModel().AddItem(equippedItem))
        {
            _model.UnequipItem(type);
            RefreshView();
            _inventoryPresenter.RefreshView();
        }
        else
        {
            _view.ShowErrorMessage("�κ��丮 ������ �����Ͽ� ��� ������ �� �����ϴ�.");
        }
    }
}
