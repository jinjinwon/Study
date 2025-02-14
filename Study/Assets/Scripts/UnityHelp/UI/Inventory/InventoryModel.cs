using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityHelp.UI.Inventory
{
    /// <summary>
    /// ��: �κ��丮 �����Ϳ� ���� ����� �����մϴ�.
    /// </summary>
    public class InventoryModel
    {
        private List<InventoryItem> items = new List<InventoryItem>();

        /// <summary>
        /// ���� ������ ��� (�б� ����)
        /// </summary>
        public IReadOnlyList<InventoryItem> Items => items.AsReadOnly();

        /// <summary>
        /// ������ �߰�
        /// </summary>
        public void AddItem(InventoryItem item)
        {
            items.Add(item);
        }

        /// <summary>
        /// ������ ����
        /// </summary>
        public bool RemoveItem(InventoryItem item)
        {
            return items.Remove(item);
        }

        /// <summary>
        /// �� ������ ��ġ ���� (�ε��� ����)
        /// </summary>
        public void SwapItems(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= items.Count || indexB < 0 || indexB >= items.Count)
                throw new ArgumentOutOfRangeException("�ε����� ��ȿ���� �ʽ��ϴ�.");

            var temp = items[indexA];
            items[indexA] = items[indexB];
            items[indexB] = temp;
        }

        /// <summary>
        /// ������ �̸� �������� ����
        /// </summary>
        public void SortItemsByName()
        {
            items.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        }

        /// <summary>
        /// ������ ���� �������� ����
        /// </summary>
        public void SortItemsByQuantity()
        {
            items.Sort((a, b) => a.Quantity.CompareTo(b.Quantity));
        }
    }
}
