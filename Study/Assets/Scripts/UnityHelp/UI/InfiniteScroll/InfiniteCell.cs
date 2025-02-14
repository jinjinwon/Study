using UnityEngine;
using System;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    /// ���� ��ũ�� �信 ǥ�õǴ� ���̸� �� ���� �����͸� �Ҵ�޾� UI ������Ʈ�� �� �� �ִ� Ŭ����
    /// </summary>
    public class InfiniteCell : MonoBehaviour
    {
        public event Action<InfiniteCell> onSelected;               // �� ���� �̺�Ʈ

        private RectTransform rectTransform;                        // �ڱ� �ڽ��� Rect

        /// <summary>
        /// �ܺο��� Rect�� �����ϱ� ���� ������Ƽ
        /// </summary>
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        /// <summary>
        /// ���� �Ҵ�� �����͸� �����ϴ� ����
        /// </summary>
        private InfiniteCellData cellData;

        /// <summary>
        /// �ܺο��� �����ϱ� ���� ������Ƽ
        /// </summary>
        public InfiniteCellData CellData
        {
            set
            {
                cellData = value;
                OnUpdate();
            }
            get
            {
                return cellData;
            }
        }

        /// <summary>
        /// UI ������Ʈ ���� �޼���
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// ���� ���õ� �� �ܺο� �˸��� ���� �޼���
        /// </summary>
        public void InvokeSelected()
        {
            if (onSelected != null)
                onSelected.Invoke(this);
        }
    }
}

