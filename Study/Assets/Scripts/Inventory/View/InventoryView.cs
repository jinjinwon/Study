using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private int slotSize;                  // ������ ����

    private InventoryPresenter _presenter;
    [SerializeField] private List<SlotData> _slots = new List<SlotData>();   // ���� ����Ʈ

    // �׽�Ʈ ����
    [SerializeField] private ItemData _item;
    [SerializeField] private int _quantity;
    [SerializeField] private ItemRemoveHandler ItemRemoveHandler;

    // �׽�Ʈ �Լ�
    public void OnClickAddItem()
    {
        InventoryItemData item = new InventoryItemData(_item, _quantity);
        _presenter.AddItem(item);
    }

    private void Start()
    {
        // Model�� Presenter �ʱ�ȭ
        var model = new InventoryModel(slotSize);
        _presenter = new InventoryPresenter(model, this);

        InitSlots();

        ItemRemoveHandler.Initialize(_presenter);
    }

    private void InitSlots()
    {
        for (int i = 0; i < slotSize; i++)
        {
            _slots[i].Initialize(i, _presenter);
        }
    }

    public void RefreshInventory(List<InventoryItemData> items)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < items.Count)
            {
                _slots[i].SetItem(items[i]);
            }
            else
            {
                _slots[i].SetItem(items[i].Clear());
            }
        }
    }

    public void ShowErrorMessage(string message) => Debug.LogError(message);

    public void OnClickSortType(int type)
    {
        _presenter.SortItems((SortType)type);
    }
}
