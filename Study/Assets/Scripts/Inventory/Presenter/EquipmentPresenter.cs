using UnityEngine;

public class EquipmentPresenter
{
    private EquipmentModel _model;
    private EquipmentView _view;
    private InventoryPresenter _inventoryPresenter; // InventoryPresenter 참조

    public EquipmentPresenter(EquipmentModel model, EquipmentView view, InventoryPresenter inventoryPresenter)
    {
        _model = model;
        _view = view;
        _inventoryPresenter = inventoryPresenter;

        _view.SetPresenter(this);
        RefreshView();
    }

    // View 갱신 요청
    public void RefreshView()
    {
        _view.RefreshEquipment(_model.GetEquippedItems());
    }

    // 장착 함수
    public bool EquipItem(InventoryItemData item)
    {
        if (_model.EquipItem(item))
        {
            return true;
        }
        return false;
    }

    // 장착 해제 함수
    public void UnequipItem(EquipType equipType)
    {
        InventoryItemData unequippedItem = _model.UnequipItem(equipType);
        if (unequippedItem == null)
        {
            _view.ShowErrorMessage("해제할 장비가 없습니다.");
            return;
        }

        // 인벤토리에 아이템을 추가
        if (!_inventoryPresenter.GetModel().AddItem(unequippedItem))
        {
            _view.ShowErrorMessage("인벤토리 슬롯이 부족하여 장비를 해제할 수 없습니다.");
            // 장비창에 다시 장착
            _model.EquipItem(unequippedItem);
            RefreshView();
        }
        else
        {
            _inventoryPresenter.RefreshView();
            RefreshView();
        }
    }

    // 장비 슬롯 <-> 장비 슬롯 스왑 함수
    public void SwapItems(int equipmentIndex, int otherEquipmentIndex, EquipType equipType)
    {
        // 장비 슬롯 인덱스를 EquipType으로 변환
        EquipType firstEquipType = (EquipType)equipmentIndex;
        EquipType secondEquipType = (EquipType)otherEquipmentIndex;

        InventoryItemData firstItem = _model.GetEquippedItem(firstEquipType);
        InventoryItemData secondItem = _model.GetEquippedItem(secondEquipType);

        if (firstItem == null || secondItem == null)
        {
            _view.ShowErrorMessage("스왑할 장비가 충분하지 않습니다.");
            return;
        }

        // 스왑
        _model.UnequipItem(firstEquipType);
        _model.UnequipItem(secondEquipType);

        _model.EquipItem(secondItem);
        _model.EquipItem(firstItem);

        RefreshView();
    }

    // Model 반환 함수
    public EquipmentModel GetModel()
    {
        return _model;
    }

    // 장비창 -> 인벤토리 이동 함수
    public void MoveItemToInventory(EquipType type, int inventoryIndex)
    {
        // EquipType을 장비 슬롯 인덱스로 매핑
        // EquipType equipType = (EquipType)equipmentIndex;

        InventoryItemData equippedItem = _model.GetEquippedItem(type);
        if (equippedItem == null)
        {
            _view.ShowErrorMessage("해제할 장비가 없습니다.");
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
            _view.ShowErrorMessage("인벤토리 슬롯이 부족하여 장비를 해제할 수 없습니다.");
        }
    }
}
