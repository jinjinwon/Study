using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Analytics.IAnalytic;
using UnityHelp.UI;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    ///  InfiniteScrollView�� ���� ���(�� Ǯ ����, Refresh, Snap ��)�� ��ӹ޾� ���� ������ �׸��� ���� ���� ��ũ���� �����ϴ� Ŭ����
    /// </summary>
    public class VerticalGridInfiniteScrollView : InfiniteScrollView
    {
        // �������� ��ũ�Ѻ��� ��� �Ǵ� �ϴܿ� ������� ���θ� ��Ÿ���� �÷��� ����
        public bool isAtTop = true;
        public bool isAtBottom = true;

        // �� ��(row)�� ���� ��(��)�� ���� �ǹ��ϴ� ����
        public int columeCount = 1;

        /// <summary>
        /// ��ũ�Ѻ��� ��ũ�� ��ġ�� �ٲ� ������ ȣ��Ǿ�, ���̴� ������ ���� �����ϰų� ȭ�� �ۿ� �ִ� ������ ȸ���ϴ� �޼���
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected override void OnValueChanged(Vector2 normalizedPosition)
        {
            // 0�̶�� 1�� ����
            if (columeCount <= 0)
            {
                columeCount = 1;
            }

            // ����Ʈ�� ���̸� ������
            float viewportInterval = scrollRect.viewport.rect.height;

            // �������� ���� anchoredPosition.y ���� ����Ͽ�, ��ũ�ѵ� ���� ������ ���
            float minViewport = scrollRect.content.anchoredPosition.y;

            // ���� ����Ʈ�� ��ܰ� �ϴ� ��ġ�� ����
            Vector2 viewportRange = new Vector2(minViewport, minViewport + viewportInterval);

            // �ʱ� �� ����
            float contentHeight = padding.x;

            /// �� ȸ��

            // �ܺ� ������ �� �྿ ó���ϸ�, �ε����� columeCount ������ ����
            for (int i = 0; i < dataList.Count; i += columeCount)
            {
                // �� �࿡ �ִ� ������ ó��
                for (int j = 0; j < columeCount; j++)
                {
                    int index = i + j;

                    // index�� dataList.Count���� ũ�ų� ���ٸ�
                    if (index >= dataList.Count)
                        break;

                    // ���� ���� ������ contentHeight���� ������ ���� ����(cellSize.y)��ŭ Ȯ��
                    var visibleRange = new Vector2(contentHeight, contentHeight + dataList[index].cellSize.y);

                    // ���� ���� visibleRange�� ����Ʈ ����(viewportRange) �ۿ� �ִٸ�
                    if (visibleRange.y < viewportRange.x || visibleRange.x > viewportRange.y)
                    {
                        RecycleCell(index);
                    }
                }

                // �� ���� ù ��° ���� ���̿� spacing�� ���Ͽ� ���� ���� ���� ��ġ�� ���
                contentHeight += dataList[i].cellSize.y + spacing;
            }

            // �ʱ� �� ����
            contentHeight = padding.x;

            /// �� ����

            // �ܺ� ������ �� �྿ ó���ϸ�, �ε����� columeCount ������ ����
            for (int i = 0; i < dataList.Count; i += columeCount)
            {
                // �� �࿡ �ִ� ������ ó��
                for (int j = 0; j < columeCount; j++)
                {
                    int index = i + j;

                    // index�� dataList.Count���� ũ�ų� ���ٸ�
                    if (index >= dataList.Count)
                        break;

                    // ���� ���� ������ contentHeight���� ������ ���� ����(cellSize.y)��ŭ Ȯ��
                    var visibleRange = new Vector2(contentHeight, contentHeight + dataList[index].cellSize.y);

                    // ����Ʈ ���� ���� �ִٸ� 
                    if (visibleRange.y >= viewportRange.x && visibleRange.x <= viewportRange.y)
                    {
                        // x ��ǥ: (cellSize.x + spacing) * j �� �� ��ȣ(j)�� ���� �¿� ��ġ
                        // y ��ǥ: -contentHeight �� ��ܿ��� �Ʒ��� �������� �������� ��ġ.
                        SetupCell(index, new Vector2(padding.x + (dataList[index].cellSize.x + spacing) * j, -contentHeight));

                        // ���ǿ� ���� Sibling ���� ����
                        if (visibleRange.y >= viewportRange.x)
                            cellList[index].transform.SetAsLastSibling();
                        else
                            cellList[index].transform.SetAsFirstSibling();
                    }
                }
                // �� �ึ�� ���� ���̿� spacing�� ���� ���� ���� ���� ��ġ�� ����
                contentHeight += dataList[i].cellSize.y + spacing;
            }

            // ���� ��ü ������ ���̰� ����Ʈ ���̺��� ũ�ٸ�
            if (scrollRect.content.sizeDelta.y > viewportInterval)
            {
                isAtTop = viewportRange.x + extendVisibleRange <= dataList[0].cellSize.y;
                isAtBottom = scrollRect.content.sizeDelta.y - viewportRange.y + extendVisibleRange <= dataList[dataList.Count - 1].cellSize.y;
            }
            else
            {
                isAtTop = true;
                isAtBottom = true;
            }
        }

        /// <summary>
        /// ��ũ�� ���� ��ü ���¸� ���� ��ġ��, �������� ũ�⸦ ������ �� ������ �ٽ� ��ġ�ϴ� �޼���
        /// </summary>
        public sealed override void Refresh()
        {
            // �ʱ�ȭ ���� �ʾҴٸ� �ʱ�ȭ
            if (!IsInitialized)
            {
                Initialize();
            }

            // ���� ����Ʈ�� ���̰� 0�̸�(���̾ƿ��� ���� �Ϸ���� ���� ���) DelayToRefresh() �ڷ�ƾ�� ����
            if (scrollRect.viewport.rect.height == 0)
                StartCoroutine(DelayToRefresh());
            else
                DoRefresh();
        }

        /// <summary>
        /// ��ü �������� ���̸� ����ϰ�, ���� Ȱ��ȭ�� ��� ���� ȸ���� �� ���� ��ġ�ϴ� �޼���
        /// </summary>
        private void DoRefresh()
        {
            // ���� ���̸� padding.x�� ������ ��, �� ������ �� ���� ����(ù ��° ���� cellSize.y)�� spacing�� ����
            float height = padding.x;
            for (int i = 0; i < dataList.Count; i += columeCount)
            {
                height += dataList[i].cellSize.y + spacing;
            }

            // cellList�� �ִ� �� ���� RecycleCell()�� ȸ���Ͽ� Ǯ�� ��ȯ
            for (int i = 0; i < cellList.Count; i++)
            {
                RecycleCell(i);
            }

            // ���� ���̿� padding.y�� ���� content�� sizeDelta.y�� ������Ʈ
            height += padding.y;
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, height);

            // OnValueChanged()�� ȣ���Ͽ� ���� ��ũ�� ��ġ�� �°� ������ ���ġ�ϰ�, onRefresh �̺�Ʈ�� ȣ��
            OnValueChanged(scrollRect.normalizedPosition);
            onRefresh?.Invoke();
        }

        /// <summary>
        /// ����Ʈ�� ���̰� 0�� ��, �� ������ ����� �� DoRefresh()�� ȣ���Ͽ� ���ΰ�ħ�� �����ϴ� �޼���
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayToRefresh()
        {
            yield return waitEndOfFrame;
            DoRefresh();
        }

        /// <summary>
        /// �־��� �ε����� ���� ���Ե� ���� ��������, �������� ����(�ε巯�� �̵�)�Ͽ� �ش� ���� ����Ʈ�� ��Ÿ������ �ϴ� �޼���
        /// </summary>
        /// <param name="index"></param>
        /// <param name="duration"></param>
        public override void Snap(int index, float duration)
        {
            // �ʱ�ȭ�� �ȵǾ��ٸ� return
            if (!IsInitialized)
                return;

            // index ���� dataList.Count�� �۰ų� ������ return
            if (index >= dataList.Count)
                return;

            // rowNumber = index / columeCount��, �ش� ���� �� ��° �࿡ ��ġ�ϴ��� ����
            var rowNumber = index / columeCount;

            // padding.x���� �����Ͽ�, ��ǥ �� ������ ��� ���� ����(�� ���� ù ���� cellSize.y + spacing)�� ����
            var height = padding.x;
            for (int i = 0; i < rowNumber; i++)
            {
                height += dataList[i * columeCount].cellSize.y + spacing;
            }

            // ���� ���̰� �������� �ִ� �̵� ���� ������ ���� �ʵ��� Mathf.Min�� ����� Ŭ����
            height = Mathf.Min(scrollRect.content.rect.height - scrollRect.viewport.rect.height, height);

            // ���� anchoredPosition.y�� ���� ���̰� �ٸ���, DoSnapping()�� ȣ���Ͽ� �ε巯�� ���� �ִϸ��̼����� �������� �̵�
            if (scrollRect.content.anchoredPosition.y != height)
            {
                DoSnapping(new Vector2(0, height), duration);
            }
        }
    }
}