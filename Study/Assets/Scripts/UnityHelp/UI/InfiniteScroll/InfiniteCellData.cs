using UnityEngine;

namespace UnityHelp.UI.InfiniteScroll
{

    /// <summary>
    /// 각 셀에 할당될 데이터를 담는 모델 클래스
    /// </summary>
    public class InfiniteCellData
    {
        public int index;                   // 인덱스
        public Vector2 cellSize;            // 셀의 크기
        public object data;                 // 실제 셀에 표시할 데이터

        /// <summary>
        ///  기본 생성자
        /// </summary>
        public InfiniteCellData()
        {

        }

        /// <summary>
        /// 셀 크기를 받는 생성자
        /// </summary>
        /// <param name="cellSize"></param>
        public InfiniteCellData(Vector2 cellSize)
        {
            this.cellSize = cellSize;
        }

        /// <summary>
        /// 셀 크기 및 표시할 데이터를 받는 생성자
        /// </summary>
        /// <param name="cellSize"></param>
        /// <param name="data"></param>
        public InfiniteCellData(Vector2 cellSize, object data)
        {
            this.cellSize = cellSize;
            this.data = data;
        }
    }
}