using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailView : MonoBehaviour
{
    public static ItemDetailView Instance { get; private set; }

    [SerializeField] private GameObject detailPanel;        // ������ â Panel
    [SerializeField] private Image itemIcon;                // ������ ������
    [SerializeField] private Text itemType;                 // ������ �̸�
    [SerializeField] private Text itemName;                 // ������ �̸�
    [SerializeField] private Text itemDescription;          // ������ ����
    [SerializeField] private Text itemQuantity;             // ������ ����
    [SerializeField] private Text itemStat;                 // ������ ����

    [SerializeField] private RectTransform detailPanelRect; // ������ â RectTransform

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

        // ������ â ������Ʈ
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
        // ���� �������� ��ġ ����
        Vector3[] slotCorners = new Vector3[4];
        slotRect.GetWorldCorners(slotCorners); // ������ ���� ��ǥ ��������
        Vector3 slotTopRight = slotCorners[2]; // ���� ��� �ڳ�

        // ������ â ũ��
        Vector2 panelSize = detailPanelRect.sizeDelta * detailPanelRect.lossyScale;

        // ��ġ ���� (���� ��ܿ� ��ġ, ȭ�� ��� ó��)
        Vector2 adjustedPosition = AdjustPositionToScreenBounds(slotTopRight, panelSize);
        detailPanelRect.position = adjustedPosition;
    }

    private Vector2 AdjustPositionToScreenBounds(Vector3 targetPosition, Vector2 panelSize)
    {
        Vector2 screenBounds = new Vector2(Screen.width, Screen.height);

        // ȭ�� �ȿ� ���߱�
        float x = Mathf.Clamp(targetPosition.x + panelSize.x / 2, panelSize.x / 2, screenBounds.x - panelSize.x / 2);
        float y = Mathf.Clamp(targetPosition.y - panelSize.y / 2, panelSize.y / 2, screenBounds.y - panelSize.y / 2);

        return new Vector2(x, y);
    }

    private string GetItemType(ItemType type)
    {
        switch(type)
        {
            case ItemType.Equipment: return "���";
            case ItemType.Food: return "����";
            case ItemType.Etc: return "��Ÿ";
            case ItemType.Spacial: return "Ư���� ����";
            default: return "";
        }
    }

    private string GetStatType(StatType type)
    {
        switch (type)
        {
            case StatType.Attack: return "���ݷ�";
            case StatType.Defence: return "����";
            case StatType.WorkSpeed: return "�۾� �ӵ�";
            case StatType.HP: return "ü��";
            case StatType.Stamina: return "���";
            case StatType.Weight: return "�ִ� �߷�";
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
