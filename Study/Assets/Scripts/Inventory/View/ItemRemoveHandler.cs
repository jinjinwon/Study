using UnityEngine;
using UnityEngine.EventSystems;

public class ItemRemoveHandler : MonoBehaviour, IDropHandler
{
    private InventoryPresenter _presenter;

    public void Initialize(InventoryPresenter presenter)
    {
        _presenter = presenter;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // �巡�׵� ���� �����͸� Ȯ��
        var slotData = eventData.pointerDrag.GetComponent<SlotData>();
        if (slotData != null && slotData._itemData?.Item != null)
        {
            // Presenter�� ���� ������ ���� ��û
            _presenter.RemoveItem(slotData._itemData);

            // ���� �ʱ�ȭ (��ĭ ����)
            slotData.Refresh();

            Debug.Log($"���� {slotData._index}�� �������� �����Ǿ����ϴ�.");
        }
    }
}
