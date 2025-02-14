using UnityEngine;

namespace UnityHelp.UI.InfiniteScroll
{

    /// <summary>
    /// �� ���� �Ҵ�� �����͸� ��� �� Ŭ����
    /// </summary>
    public class InfiniteCellData
    {
        public int index;                   // �ε���
        public Vector2 cellSize;            // ���� ũ��
        public object data;                 // ���� ���� ǥ���� ������

        /// <summary>
        ///  �⺻ ������
        /// </summary>
        public InfiniteCellData()
        {

        }

        /// <summary>
        /// �� ũ�⸦ �޴� ������
        /// </summary>
        /// <param name="cellSize"></param>
        public InfiniteCellData(Vector2 cellSize)
        {
            this.cellSize = cellSize;
        }

        /// <summary>
        /// �� ũ�� �� ǥ���� �����͸� �޴� ������
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