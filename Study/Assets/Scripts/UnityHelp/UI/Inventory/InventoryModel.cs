using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityHelp.UI.Inventory
{
    /// <summary>
    /// 모델: 인벤토리 데이터와 조작 기능을 제공합니다.
    /// </summary>
    public class InventoryModel
    {
        private List<InventoryItem> items = new List<InventoryItem>();

        /// <summary>
        /// 현재 아이템 목록 (읽기 전용)
        /// </summary>
        public IReadOnlyList<InventoryItem> Items => items.AsReadOnly();

        /// <summary>
        /// 아이템 추가
        /// </summary>
        public void AddItem(InventoryItem item)
        {
            items.Add(item);
        }

        /// <summary>
        /// 아이템 제거
        /// </summary>
        public bool RemoveItem(InventoryItem item)
        {
            return items.Remove(item);
        }

        /// <summary>
        /// 두 아이템 위치 스왑 (인덱스 기준)
        /// </summary>
        public void SwapItems(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= items.Count || indexB < 0 || indexB >= items.Count)
                throw new ArgumentOutOfRangeException("인덱스가 유효하지 않습니다.");

            var temp = items[indexA];
            items[indexA] = items[indexB];
            items[indexB] = temp;
        }

        /// <summary>
        /// 아이템 이름 오름차순 정렬
        /// </summary>
        public void SortItemsByName()
        {
            items.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        }

        /// <summary>
        /// 아이템 수량 오름차순 정렬
        /// </summary>
        public void SortItemsByQuantity()
        {
            items.Sort((a, b) => a.Quantity.CompareTo(b.Quantity));
        }
    }
}
