using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemQuantity;
    [SerializeField] private GameObject slotInfo;

    public InventoryItemData _itemData;
    public int _index;
    protected InventoryPresenter _inventoryPresenter;
    protected EquipmentPresenter _equipmentPresenter;

    protected Vector3 initialPosition;
    public RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    protected Canvas canvas;

    private void Awake()
    {
        rectTransform = slotInfo.GetComponent<RectTransform>();
        canvasGroup = slotInfo.GetComponent<CanvasGroup>();
    }

    // 슬롯 초기화
    public void Initialize(int index, InventoryPresenter inventoryPresenter, EquipmentPresenter equipmentPresenter = null)
    {
        _index = index;
        _inventoryPresenter = inventoryPresenter;
        _equipmentPresenter = equipmentPresenter;
    }

    // 아이템 세팅
    public virtual void SetItem(InventoryItemData item)
    {
        _itemData = item;
        if (item != null && item.Item != null)
        {
            slotInfo.SetActive(true);
            itemIcon.sprite = item.Item.Icon;
            itemIcon.enabled = true;
            itemName.text = item.Item.ItemName;
            itemQuantity.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
        }
        else
        {
            slotInfo.SetActive(false);
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            itemName.text = "";
            itemQuantity.text = "";
        }
    }

    public void ClearSlot()
    {
        SetItem(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_itemData != null && _itemData.Item != null)
        {
            ItemDetailView.Instance.ShowDetails(_itemData, rectTransform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemDetailView.Instance.gameObject.activeSelf)
        {
            ItemDetailView.Instance.HideDetails(); // 슬롯을 벗어나면 창 닫기
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (_itemData?.Item != null)
        {
            initialPosition = rectTransform.localPosition;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;

            canvas = slotInfo.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_itemData?.Item != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (_itemData?.Item != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1.0f;

            // 드롭 위치 확인
            if (eventData.pointerEnter != null)
            {
                var targetSlot = eventData.pointerEnter.GetComponent<SlotData>();
                if (targetSlot != null && targetSlot != this)
                {
                    // 장비 슬롯인 경우 타입 검증
                    if (targetSlot is EquipmentSlot equipmentSlot)
                    {
                        if (_itemData.Item.EquipType != equipmentSlot.EquipType)
                        {
                            Debug.LogError($"아이템 '{_itemData.Item.ItemName}'은(는) {equipmentSlot.EquipType} 슬롯에 장착할 수 없습니다.");
                            _inventoryPresenter.View.ShowErrorMessage($"이 아이템은 {equipmentSlot.EquipType} 슬롯에 장착할 수 없습니다.");

                            StartCoroutine(ShowInvalidSlotFeedback());

                            // 드래그가 끝난 후 원래 위치로 복귀
                            rectTransform.localPosition = initialPosition;
                            Destroy(canvas);
                            return;
                        }
                    }

                    // 인벤토리 슬롯에서 장비 슬롯으로, 또는 장비 슬롯에서 인벤토리 슬롯으로
                    if (_inventoryPresenter != null && targetSlot._equipmentPresenter != null)
                    {
                        // 인벤토리 -> 장비창
                        _inventoryPresenter.MoveItemToEquipment(_index, targetSlot._index, targetSlot.GetComponent<EquipmentSlot>().EquipType);
                    }
                    else if (_equipmentPresenter != null && targetSlot._inventoryPresenter != null)
                    {
                        // 장비창 -> 인벤토리
                        _equipmentPresenter.MoveItemToInventory(GetComponent<EquipmentSlot>().EquipType, targetSlot._index);
                    }
                    else if (_inventoryPresenter != null && targetSlot._inventoryPresenter != null)
                    {
                        // 인벤토리 슬롯 간 스왑
                        _inventoryPresenter.SwapItems(_index, targetSlot._index);
                    }
                    else if (_equipmentPresenter != null && targetSlot._equipmentPresenter != null)
                    {
                        // 장비 슬롯 간 스왑
                        _equipmentPresenter.SwapItems(_index, targetSlot._index, targetSlot.GetComponent<EquipmentSlot>().EquipType);
                    }
                }
            }

            // 드래그가 끝난 후 원래 위치로 복귀
            rectTransform.localPosition = initialPosition;
            Destroy(canvas);
        }
    }

    private IEnumerator ShowInvalidSlotFeedback()
    {
        // 아이템 아이콘에 빨간색 테두리 추가
        itemIcon.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        itemIcon.color = Color.white;
    }

    // 슬롯 새로고침
    public void Refresh()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        rectTransform.localPosition = initialPosition;
        Destroy(canvas);
    }
}
