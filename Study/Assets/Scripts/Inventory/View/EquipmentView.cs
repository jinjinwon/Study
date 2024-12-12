using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour
{
    [Header("Equipment Slots")]
    [SerializeField] private List<EquipmentSlot> equipmentSlots;

    [Header("UI Elements")]
    [SerializeField] private GameObject equipmentPanel; 
    [SerializeField] private Text errorMessageText; 

    private EquipmentPresenter _presenter;

    public void SetPresenter(EquipmentPresenter presenter)
    {
        _presenter = presenter;
    }
    
    // ���� �ʱ�ȭ
    public void InitSlot(InventoryPresenter presenter, EquipmentPresenter presenter1)
    {
        // InventoryView�� ���� �ʱ�ȭ
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            if (equipmentSlots[i] != null)
            {
                equipmentSlots[i].Initialize(i, presenter, presenter1); // Inventory ���Կ��� EquipmentPresenter�� ����
            }
        }
    }

    // ���� ����
    public void RefreshEquipment(Dictionary<EquipType, InventoryItemData> equippedItems)
    {
        foreach (var slot in equipmentSlots)
        {
            if (equippedItems.ContainsKey(slot.EquipType))
            {
                var itemData = equippedItems[slot.EquipType];
                if (itemData != null && itemData.Item != null)
                {
                    slot.SetItem(itemData);
                }
                else
                {
                    slot.ClearSlot();
                }
            }
            else
            {
                slot.ClearSlot();
            }
        }
    }

    // �����޼��� �Լ�
    public void ShowErrorMessage(string message)
    {
        StartCoroutine(ShowErrorCoroutine(message));
    }

    private IEnumerator ShowErrorCoroutine(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorMessageText.gameObject.SetActive(false);
    }
}
