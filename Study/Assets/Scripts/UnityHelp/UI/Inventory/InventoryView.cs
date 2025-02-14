using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.UI.Inventory
{
    /// <summary>
    /// 뷰: 인벤토리 UI 갱신 및 사용자와의 인터랙션을 담당합니다.
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        /// <summary>
        /// 인벤토리 아이템 목록을 출력합니다.
        /// </summary>
        public void DisplayItems(IReadOnlyList<InventoryItem> items)
        {
            Debug.Log("----- 인벤토리 상태 -----");
            foreach (var item in items)
            {
                Debug.Log($"아이템: {item.Name}, 수량: {item.Quantity}");
            }
        }
    }
}
