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
    private InventoryPresenter _presenter;

    private Vector3 initialPosition;              
    private RectTransform rectTransform;          
    private CanvasGroup canvasGroup;

    private Canvas canvas;

    private void Start()
    {
        rectTransform = slotInfo.GetComponent<RectTransform>();
        canvasGroup = slotInfo.GetComponent<CanvasGroup>();
    }

    public void Initialize(int index, InventoryPresenter presenter)
    {
        _index = index;
        _presenter = presenter;
    }

    public void SetItem(InventoryItemData item)
    {
        _itemData = item;
        if (item.Item != null)
        {
            slotInfo.SetActive(true);
            itemIcon.sprite = item.Item.Icon;
            itemName.text = item.Item.ItemName;
            itemQuantity.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
        }
        else
        {
            slotInfo.SetActive(false);
            itemIcon.sprite = null;
            itemName.text = "";
            itemQuantity.text = "";
        }
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_itemData?.Item != null)                        // �������� �ִ� ��쿡�� �巡�� ����
        {
            initialPosition = rectTransform.localPosition; // ���� ��ġ ����
            canvasGroup.blocksRaycasts = false;            // �巡�� �� �浹 ��Ȱ��ȭ
            canvasGroup.alpha = 0.6f;                      // ���� ����
            canvas = slotInfo.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_itemData?.Item != null)
        {
            // ���콺 ��ġ�� ���� SlotInfo �̵�
            rectTransform.anchoredPosition += eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_itemData?.Item != null)
        {
            canvasGroup.blocksRaycasts = true;     // �浹 ����
            canvasGroup.alpha = 1.0f;              // ���� ����

            // ��� ��ġ Ȯ��
            if (eventData.pointerEnter != null)
            {
                var targetSlot = eventData.pointerEnter.GetComponent<SlotData>();
                if (targetSlot != null && targetSlot != this)
                {
                    // Swap
                    _presenter.SwapItems(_index, targetSlot._index);
                }
            }

            // �巡�װ� ���� �� ���� ��ġ�� ����
            rectTransform.localPosition = initialPosition;
            Destroy(canvas);
        }
    }

    public void Refresh()
    {
        canvasGroup.blocksRaycasts = true;     // �浹 ����
        canvasGroup.alpha = 1.0f;              // ���� ����

        // �巡�װ� ���� �� ���� ��ġ�� ����
        rectTransform.localPosition = initialPosition;
        Destroy(canvas);
    }
}
