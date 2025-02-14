using System;
using UnityEngine;

namespace UnityHelp.UI.Inventory
{
    /// <summary>
    /// ��������: �𵨰� �並 �����ϰ� ����Ͻ� ����(������ �߰�, ����, ����, ����)�� ó���մϴ�.
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
        /// ���ο� �������� �߰��� �� �並 ������Ʈ�մϴ�.
        /// </summary>
        public void AddItem(string name, int quantity)
        {
            model.AddItem(new InventoryItem { Name = name, Quantity = quantity });
            UpdateView();
        }

        /// <summary>
        /// �ε����� ���� ������ ���� �� �並 ������Ʈ�մϴ�.
        /// </summary>
        public void RemoveItemAt(int index)
        {
            if (index < 0 || index >= model.Items.Count)
            {
                Debug.LogError("��ȿ���� ���� �ε����Դϴ�.");
                return;
            }

            var item = model.Items[index];
            model.RemoveItem(item);
            UpdateView();
        }

        /// <summary>
        /// �� �������� ��ġ�� ������ �� �並 ������Ʈ�մϴ�.
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
        /// �����۵��� �̸� ������ ������ �� �並 ������Ʈ�մϴ�.
        /// </summary>
        public void SortItemsByName()
        {
            model.SortItemsByName();
            UpdateView();
        }

        /// <summary>
        /// �����۵��� ���� ������ ������ �� �並 ������Ʈ�մϴ�.
        /// </summary>
        public void SortItemsByQuantity()
        {
            model.SortItemsByQuantity();
            UpdateView();
        }

        /// <summary>
        /// �並 �����Ͽ� ���� ���� ���¸� �ݿ��մϴ�.
        /// </summary>
        public void UpdateView()
        {
            view.DisplayItems(model.Items);
        }
    }
}
