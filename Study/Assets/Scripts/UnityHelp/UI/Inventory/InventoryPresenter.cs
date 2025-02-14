using System;
using UnityEngine;

namespace UnityHelp.UI.Inventory
{
    /// <summary>
    /// 프리젠터: 모델과 뷰를 연결하고 비즈니스 로직(아이템 추가, 제거, 정렬, 스왑)을 처리합니다.
    /// </summary>
    public class InventoryPresenter
    {
        private InventoryModel model;
        private InventoryView view;

        public InventoryPresenter(InventoryModel model, InventoryView view)
        {
            this.model = model;
            this.view = view;
        }

        /// <summary>
        /// 새로운 아이템을 추가한 후 뷰를 업데이트합니다.
        /// </summary>
        public void AddItem(string name, int quantity)
        {
            model.AddItem(new InventoryItem { Name = name, Quantity = quantity });
            UpdateView();
        }

        /// <summary>
        /// 인덱스를 통해 아이템 제거 후 뷰를 업데이트합니다.
        /// </summary>
        public void RemoveItemAt(int index)
        {
            if (index < 0 || index >= model.Items.Count)
            {
                Debug.LogError("유효하지 않은 인덱스입니다.");
                return;
            }

            var item = model.Items[index];
            model.RemoveItem(item);
            UpdateView();
        }

        /// <summary>
        /// 두 아이템의 위치를 스왑한 후 뷰를 업데이트합니다.
        /// </summary>
        public void SwapItems(int indexA, int indexB)
        {
            try
            {
                model.SwapItems(indexA, indexB);
                UpdateView();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// 아이템들을 이름 순으로 정렬한 후 뷰를 업데이트합니다.
        /// </summary>
        public void SortItemsByName()
        {
            model.SortItemsByName();
            UpdateView();
        }

        /// <summary>
        /// 아이템들을 수량 순으로 정렬한 후 뷰를 업데이트합니다.
        /// </summary>
        public void SortItemsByQuantity()
        {
            model.SortItemsByQuantity();
            UpdateView();
        }

        /// <summary>
        /// 뷰를 갱신하여 현재 모델의 상태를 반영합니다.
        /// </summary>
        public void UpdateView()
        {
            view.DisplayItems(model.Items);
        }
    }
}
