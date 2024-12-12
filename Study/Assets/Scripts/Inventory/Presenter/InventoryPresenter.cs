using UnityEngine;

public class InventoryPresenter
{
    private InventoryModel _model;
    private InventoryView _view;
    private EquipmentPresenter _equipmentPresenter; // EquipmentPresenter 참조

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
    /// EquipmentPresenter를 설정합니다.
    /// </summary>
    /// <param name="equipmentPresenter">EquipmentPresenter</param>
    public void SetEquipmentPresenter(EquipmentPresenter equipmentPresenter)
    {
        _equipmentPresenter = equipmentPresenter;
    }

    /// <summary>
    /// 인벤토리에 아이템을 추가합니다.
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    public void AddItem(InventoryItemData item)
    {
        if (_model.AddItem(item))
        {
            RefreshView();
        }
        else
        {
            _view.ShowErrorMessage("인벤토리 슬롯이 부족합니다.");
        }
    }

    /// <summary>
    /// 인벤토리에서 아이템을 제거합니다.
    /// </summary>
    /// <param name="item">제거할 아이템</param>
    public void RemoveItem(InventoryItemData item)
    {
        _model.RemoveItem(item);
        RefreshView();
    }

    /// <summary>
    /// 두 인벤토리 아이템을 스왑합니다.
    /// </summary>
    /// <param name="dragItemIndex">드래그한 아이템의 인덱스</param>
    /// <param name="dropItemIndex">드롭한 아이템의 인덱스</param>
    public void SwapItems(int dragItemIndex, int dropItemIndex)
    {
        _model.SwapItems(dragItemIndex, dropItemIndex);
        RefreshView();
    }

    /// <summary>
    /// 인벤토리 아이템을 정렬합니다.
    /// </summary>
    /// <param name="type">정렬 기준</param>
    public void SortItems(SortType type)
    {
        _model.SortItems(type);
        RefreshView();
    }

    /// <summary>
    /// 인벤토리 -> 장비창으로 아이템을 이동하거나 스왑합니다.
    /// </summary>
    /// <param name="inventoryIndex">인벤토리 슬롯 인덱스</param>
    /// <param name="equipmentIndex">장비창 슬롯 인덱스</param>
    /// <param name="equipType">장비창 슬롯의 EquipType</param>
    public void MoveItemToEquipment(int inventoryIndex, int equipmentIndex, EquipType equipType)
    {
        InventoryItemData inventoryItem = _model.GetItemAt(inventoryIndex);
        if (inventoryItem == null || inventoryItem.Item == null)
            return;

        // 아이템의 EquipType과 슬롯의 EquipType이 일치하는지 확인
        if (inventoryItem.Item.EquipType != equipType)
            return;

        // 기존 장착된 아이템 확인
        InventoryItemData equippedItem = _equipmentPresenter.GetModel().GetEquippedItem(equipType);

        // 기존 장착된 아이템이 있으면 스왑
        if (equippedItem.Item != null)
        {
            _model.AddItem(equippedItem);                     // 기존 아이템을 인벤토리에 추가
            _equipmentPresenter.GetModel().UnequipItem(equipType); // 기존 아이템을 장비창에서 해제
        }

        // 인벤토리 아이템을 복사하여 장비창에 추가
        InventoryItemData newItem = new InventoryItemData(inventoryItem.Item, inventoryItem.Quantity);
        _equipmentPresenter.EquipItem(newItem);

        // 원본 아이템을 인벤토리에서 제거
        _model.RemoveItem(inventoryItem);

        // 뷰 갱신
        RefreshView();
        _equipmentPresenter.RefreshView();
    }

    /// <summary>
    /// 뷰를 새로고침합니다.
    /// </summary>
    public void RefreshView()
    {
        _view.RefreshInventory(_model.GetItems());
    }

    /// <summary>
    /// 모델을 반환합니다.
    /// </summary>
    /// <returns>InventoryModel</returns>
    public InventoryModel GetModel()
    {
        return _model;
    }

    /// <summary>
    /// 특정 인덱스의 아이템을 가져옵니다.
    /// </summary>
    /// <param name="index">아이템 인덱스</param>
    /// <returns>InventoryItemData</returns>
    public InventoryItemData GetItemAt(int index)
    {
        return _model.GetItemAt(index);
    }

    /// <summary>
    /// 특정 아이템의 인덱스를 가져옵니다.
    /// </summary>
    /// <param name="item">InventoryItemData</param>
    /// <returns>아이템 인덱스</returns>
    public int GetIndexOfItem(InventoryItemData item)
    {
        return _model.GetIndexOfItem(item);
    }
}

