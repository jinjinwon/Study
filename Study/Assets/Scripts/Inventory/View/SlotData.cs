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

    // ���� �ʱ�ȭ
    public void Initialize(int index, InventoryPresenter inventoryPresenter, EquipmentPresenter equipmentPresenter = null)
    {
        _index = index;
        _inventoryPresenter = inventoryPresenter;
        _equipmentPresenter = equipmentPresenter;
    }

    // ������ ����
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
            ItemDetailView.Instance.HideDetails(); // ������ ����� â �ݱ�
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

            // ��� ��ġ Ȯ��
            if (eventData.pointerEnter != null)
            {
                var targetSlot = eventData.pointerEnter.GetComponent<SlotData>();
                if (targetSlot != null && targetSlot != this)
                {
                    // ��� ������ ��� Ÿ�� ����
                    if (targetSlot is EquipmentSlot equipmentSlot)
                    {
                        if (_itemData.Item.EquipType != equipmentSlot.EquipType)
                        {
                            Debug.LogError($"������ '{_itemData.Item.ItemName}'��(��) {equipmentSlot.EquipType} ���Կ� ������ �� �����ϴ�.");
                            _inventoryPresenter.View.ShowErrorMessage($"�� �������� {equipmentSlot.EquipType} ���Կ� ������ �� �����ϴ�.");

                            StartCoroutine(ShowInvalidSlotFeedback());

                            // �巡�װ� ���� �� ���� ��ġ�� ����
                            rectTransform.localPosition = initialPosition;
                            Destroy(canvas);
                            return;
                        }
                    }

                    // �κ��丮 ���Կ��� ��� ��������, �Ǵ� ��� ���Կ��� �κ��丮 ��������
                    if (_inventoryPresenter != null && targetSlot._equipmentPresenter != null)
                    {
                        // �κ��丮 -> ���â
                        _inventoryPresenter.MoveItemToEquipment(_index, targetSlot._index, targetSlot.GetComponent<EquipmentSlot>().EquipType);
                    }
                    else if (_equipmentPresenter != null && targetSlot._inventoryPresenter != null)
                    {
                        // ���â -> �κ��丮
                        _equipmentPresenter.MoveItemToInventory(GetComponent<EquipmentSlot>().EquipType, targetSlot._index);
                    }
                    else if (_inventoryPresenter != null && targetSlot._inventoryPresenter != null)
                    {
                        // �κ��丮 ���� �� ����
                        _inventoryPresenter.SwapItems(_index, targetSlot._index);
                    }
                    else if (_equipmentPresenter != null && targetSlot._equipmentPresenter != null)
                    {
                        // ��� ���� �� ����
                        _equipmentPresenter.SwapItems(_index, targetSlot._index, targetSlot.GetComponent<EquipmentSlot>().EquipType);
                    }
                }
            }

            // �巡�װ� ���� �� ���� ��ġ�� ����
            rectTransform.localPosition = initialPosition;
            Destroy(canvas);
        }
    }

    private IEnumerator ShowInvalidSlotFeedback()
    {
        // ������ �����ܿ� ������ �׵θ� �߰�
        itemIcon.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        itemIcon.color = Color.white;
    }

    // ���� ���ΰ�ħ
    public void Refresh()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        rectTransform.localPosition = initialPosition;
        Destroy(canvas);
    }
}
