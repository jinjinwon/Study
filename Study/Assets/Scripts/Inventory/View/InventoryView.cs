using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [Header("Inventory Slots")]
    [SerializeField] private List<SlotData> inventorySlots; // 인벤토리 슬롯 UI 참조

    [Header("UI Elements")]
    [SerializeField] private GameObject inventoryPanel;          // 인벤토리 패널
    [SerializeField] private Text errorMessageText;             // 에러 메시지 텍스트

    private InventoryPresenter _presenter;

    /// <summary>
    /// Presenter를 설정합니다.
    /// </summary>
    /// <param name="presenter">InventoryPresenter</param>
    public void SetPresenter(InventoryPresenter presenter)
    {
        _presenter = presenter;
    }

    public void InitSlot(InventoryPresenter presenter)
    {
        // InventoryView의 슬롯 초기화
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] != null)
            {
                inventorySlots[i].Initialize(i, presenter, null); // Inventory 슬롯에는 EquipmentPresenter가 없음
            }
        }
    }

    /// <summary>
    /// 현재 인벤토리를 UI에 표시합니다.
    /// </summary>
    /// <param name="items">인벤토리 아이템 리스트</param>
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
    /// 에러 메시지를 표시합니다.
    /// </summary>
    /// <param name="message">에러 메시지 내용</param>
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
    /// 인벤토리 창을 엽니다.
    /// </summary>
    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }

    /// <summary>
    /// 인벤토리 창을 닫습니다.
    /// </summary>
    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }

    /// <summary>
    /// 정렬 버튼 클릭 시 호출됩니다.
    /// </summary>
    /// <param name="sortType">정렬 기준</param>
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
