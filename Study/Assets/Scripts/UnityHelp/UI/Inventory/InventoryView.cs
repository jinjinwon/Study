using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.UI.Inventory
{
    /// <summary>
    /// ��: �κ��丮 UI ���� �� ����ڿ��� ���ͷ����� ����մϴ�.
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        /// <summary>
        /// �κ��丮 ������ ����� ����մϴ�.
        /// </summary>
        public void DisplayItems(IReadOnlyList<InventoryItem> items)
        {
            Debug.Log("----- �κ��丮 ���� -----");
            foreach (var item in items)
            {
                Debug.Log($"������: {item.Name}, ����: {item.Quantity}");
            }
        }
    }
}
