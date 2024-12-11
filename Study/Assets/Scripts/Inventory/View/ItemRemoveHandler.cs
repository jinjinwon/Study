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
        // 드래그된 슬롯 데이터를 확인
        var slotData = eventData.pointerDrag.GetComponent<SlotData>();
        if (slotData != null && slotData._itemData?.Item != null)
        {
            // Presenter를 통해 아이템 삭제 요청
            _presenter.RemoveItem(slotData._itemData);

            // 슬롯 초기화 (빈칸 유지)
            slotData.Refresh();

            Debug.Log($"슬롯 {slotData._index}의 아이템이 삭제되었습니다.");
        }
    }
}
