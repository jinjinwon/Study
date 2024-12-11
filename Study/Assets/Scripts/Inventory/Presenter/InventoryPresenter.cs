using UnityEngine;

public class InventoryPresenter
{
    private InventoryModel _model;
    private InventoryView _view;

    public InventoryPresenter(InventoryModel model, InventoryView view)
    {
        _model = model;
        _view = view;

        _view.RefreshInventory(_model.GetItems());
    }

    public void AddItem(InventoryItemData item)
    {
        if (_model.AddItem(item))
        {
            _view.RefreshInventory(_model.GetItems());
        }
        else
        {
            _view.ShowErrorMessage("인벤토리 슬롯이 부족합니다.");
        }
    }

    public void RemoveItem(InventoryItemData item)
    {
        _model.RemoveItem(item);
        _view.RefreshInventory(_model.GetItems());
    }

    public void SwapItems(int dragItemIndex, int dropItemIndex)
    {
        _model.SwapItems(dragItemIndex, dropItemIndex);
        _view.RefreshInventory(_model.GetItems());
    }

    public void SortItems(SortType type)
    {
        _model.SortItems(type);
        _view.RefreshInventory(_model.GetItems());
    }
}

