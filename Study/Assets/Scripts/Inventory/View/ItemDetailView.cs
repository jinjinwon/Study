using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailView : MonoBehaviour
{
    public static ItemDetailView Instance { get; private set; }

    [SerializeField] private GameObject detailPanel;        // 상세정보 창 Panel
    [SerializeField] private Image itemIcon;                // 아이템 아이콘
    [SerializeField] private Text itemType;                 // 아이템 이름
    [SerializeField] private Text itemName;                 // 아이템 이름
    [SerializeField] private Text itemDescription;          // 아이템 설명
    [SerializeField] private Text itemQuantity;             // 아이템 수량
    [SerializeField] private Text itemStat;                 // 아이템 수량

    [SerializeField] private RectTransform detailPanelRect; // 상세정보 창 RectTransform

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        HideDetails();
    }

    public void ShowDetails(InventoryItemData itemData, RectTransform slotRect)
    {
        if (itemData == null || itemData.Item == null)
        {
            HideDetails();
            return;
        }

        // 상세정보 창 업데이트
        itemIcon.sprite = itemData.Item.Icon;
        itemName.text = itemData.Item.ItemName;
        itemType.text = GetItemType(itemData.Item.ItemType);
        itemDescription.text = itemData.Item.Description;
        itemQuantity.text = itemData.Quantity.ToString();
        itemStat.text = GetStat(itemData.Item.StatModifiers);

        InitPos(slotRect);

        detailPanel.SetActive(true);
    }

    public void HideDetails()
    {
        detailPanel.SetActive(false);
    }

    private void InitPos(RectTransform slotRect)
    {
        // 슬롯 기준으로 위치 설정
        Vector3[] slotCorners = new Vector3[4];
        slotRect.GetWorldCorners(slotCorners); // 슬롯의 월드 좌표 가져오기
        Vector3 slotTopRight = slotCorners[2]; // 우측 상단 코너

        // 상세정보 창 크기
        Vector2 panelSize = detailPanelRect.sizeDelta * detailPanelRect.lossyScale;

        // 위치 조정 (우측 상단에 배치, 화면 경계 처리)
        Vector2 adjustedPosition = AdjustPositionToScreenBounds(slotTopRight, panelSize);
        detailPanelRect.position = adjustedPosition;
    }

    private Vector2 AdjustPositionToScreenBounds(Vector3 targetPosition, Vector2 panelSize)
    {
        Vector2 screenBounds = new Vector2(Screen.width, Screen.height);

        // 화면 안에 맞추기
        float x = Mathf.Clamp(targetPosition.x + panelSize.x / 2, panelSize.x / 2, screenBounds.x - panelSize.x / 2);
        float y = Mathf.Clamp(targetPosition.y - panelSize.y / 2, panelSize.y / 2, screenBounds.y - panelSize.y / 2);

        return new Vector2(x, y);
    }

    private string GetItemType(ItemType type)
    {
        switch(type)
        {
            case ItemType.Equipment: return "장비";
            case ItemType.Food: return "음식";
            case ItemType.Etc: return "기타";
            case ItemType.Spacial: return "특별한 도구";
            default: return "";
        }
    }

    private string GetStatType(StatType type)
    {
        switch (type)
        {
            case StatType.Attack: return "공격력";
            case StatType.Defence: return "방어력";
            case StatType.WorkSpeed: return "작업 속도";
            case StatType.HP: return "체력";
            case StatType.Stamina: return "기력";
            case StatType.Weight: return "최대 중량";
            default: return "";
        }
    }

    private string GetStat(List<StatModifier> stats)
    {
        string strTemp = "";
        foreach(var pair in stats)
        {
            strTemp += $"{GetStatType(pair.TargetStat.StatType)} : {pair.ModifierValue} \n";
        }
        return strTemp;
    }    
}
