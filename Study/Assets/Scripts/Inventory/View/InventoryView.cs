using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [Header("Inventory Slots")]
    [SerializeField] private List<SlotData> inventorySlots; // �κ��丮 ���� UI ����

    [Header("UI Elements")]
    [SerializeField] private GameObject inventoryPanel;          // �κ��丮 �г�
    [SerializeField] private Text errorMessageText;             // ���� �޽��� �ؽ�Ʈ

    private InventoryPresenter _presenter;

    /// <summary>
    /// Presenter�� �����մϴ�.
    /// </summary>
    /// <param name="presenter">InventoryPresenter</param>
    public void SetPresenter(InventoryPresenter presenter)
    {
        _presenter = presenter;
    }

    public void InitSlot(InventoryPresenter presenter)
    {
        // InventoryView�� ���� �ʱ�ȭ
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] != null)
            {
                inventorySlots[i].Initialize(i, presenter, null); // Inventory ���Կ��� EquipmentPresenter�� ����
            }
        }
    }

    /// <summary>
    /// ���� �κ��丮�� UI�� ǥ���մϴ�.
    /// </summary>
    /// <param name="items">�κ��丮 ������ ����Ʈ</param>
    public void RefreshInventory(List<InventoryItemData> items)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < items.Count)
            {
                inventorySlots[i].SetItem(items[i]);
            }
            else
            {
                inventorySlots[i].SetItem(null);
            }
        }
    }

    /// <summary>
    /// ���� �޽����� ǥ���մϴ�.
    /// </summary>
    /// <param name="message">���� �޽��� ����</param>
    public void ShowErrorMessage(string message)
    {
        StartCoroutine(ShowErrorCoroutine(message));
    }

    private System.Collections.IEnumerator ShowErrorCoroutine(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorMessageText.gameObject.SetActive(false);
    }

    /// <summary>
    /// �κ��丮 â�� ���ϴ�.
    /// </summary>
    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }

    /// <summary>
    /// �κ��丮 â�� �ݽ��ϴ�.
    /// </summary>
    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }

    /// <summary>
    /// ���� ��ư Ŭ�� �� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="sortType">���� ����</param>
    public void OnSortButtonClicked(int sortType)
    {
        _presenter.SortItems((SortType)sortType);
    }


    public ItemData item;
    public int count;
    public void TestItemAdd()
    {
        _presenter.AddItem(new InventoryItemData(item, count));
    }
}
