using UnityEngine;
using System;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    /// 무한 스크롤 뷰에 표시되는 셀이며 각 셀의 데이터를 할당받아 UI 업데이트를 할 수 있는 클래스
    /// </summary>
    public class InfiniteCell : MonoBehaviour
    {
        public event Action<InfiniteCell> onSelected;               // 셀 선택 이벤트

        private RectTransform rectTransform;                        // 자기 자신의 Rect

        /// <summary>
        /// 외부에서 Rect에 접근하기 위한 프로퍼티
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
        /// 셀에 할당될 데이터를 저장하는 변수
        /// </summary>
        private InfiniteCellData cellData;

        /// <summary>
        /// 외부에서 접근하기 위한 프로퍼티
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
        /// UI 업데이트 로직 메서드
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// 셀이 선택될 때 외부에 알리기 위한 메서드
        /// </summary>
        public void InvokeSelected()
        {
            if (onSelected != null)
                onSelected.Invoke(this);
        }
    }
}

