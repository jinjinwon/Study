using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Analytics.IAnalytic;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    /// ���� ��ũ�� ���� �⺻ ����� �����ϴ� InfiniteScrollView�� ��� �޾� ���� ��ũ�� �������� �����ϴ� Ŭ����
    /// </summary>
    public class HorizontalInfiniteScrollView : InfiniteScrollView
    {
        public bool isAtLeft = true;                // ��ũ�� �䰡 ���� ���� ������� ����
        public bool isAtRight = true;               // ��ũ�� �䰡 ������ ���� ������� ����

        /// <summary>
        /// �ʱ�ȭ �޼���
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            isAtLeft = true;
            isAtRight = true;
        }

        /// <summary>
        /// ��ũ�� ���� ���� ��ũ�� ��ġ�� �ٲ� ������ ȣ��Ǵ� �޼���
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected override void OnValueChanged(Vector2 normalizedPosition)
        {
            // dataList�� ���ٸ�
            if (dataList.Count == 0)
                return;

            /// ����Ʈ ���� ���
            // �ʺ� ���
            float viewportInterval = scrollRect.viewport.rect.width;        

            // x��ǥ�� �����Ͽ� ����Ʈ�� ���� ���� ��� ��ġ
            float minViewport = -scrollRect.content.anchoredPosition.x;     

            // ���� ���� ������ ���� ������ ���� ����
            Vector2 viewportRange = new Vector2(minViewport - extendVisibleRange, minViewport + viewportInterval + extendVisibleRange);

            // ��ġ ����
            float contentWidth = padding.x;                                 

            /// �� ��Ȱ��
            for (int i = 0; i < dataList.Count; i++)
            {
                // �� ������ �׸� ����, �ش� ���� �����ϴ� ���� ����(visibleRange)�� ���
                var visibleRange = new Vector2(contentWidth, contentWidth + dataList[i].cellSize.x);

                // ���� �� visibleRange�� ����Ʈ �����ۿ� �ִٸ�, RecycleCell(i)�� ȣ���Ͽ� �ش� ���� ȸ��
                if (visibleRange.y < viewportRange.x || visibleRange.x > viewportRange.y)
                {
                    RecycleCell(i);
                }

                // �� ���� �ʺ�� spacing��ŭ contentWidth�� �������� ���� ���� ���� ��ġ�� ����մϴ�.
                contentWidth += dataList[i].cellSize.x + spacing;
            }

            // �ٽ� ��ġ ����
            contentWidth = padding.x;             
            
            /// �� ����
            for (int i = 0; i < dataList.Count; i++)
            {
                // �� ������ �׸� ����, �ش� ���� �����ϴ� ���� ����(visibleRange)�� ���
                var visibleRange = new Vector2(contentWidth, contentWidth + dataList[i].cellSize.x);

                // visibleRange�� ����Ʈ ���� �����ϸ� SetupCell(i, new Vector2(contentWidth, 0))�� ȣ���Ͽ� �ش� ���� Ȱ��ȭ�ϰ� ������ ��ġ�� ��ġ.
                if (visibleRange.y >= viewportRange.x && visibleRange.x <= viewportRange.y)
                {
                    SetupCell(i, new Vector2(contentWidth, 0));

                    // ���ǿ� ���� sibling ������ ����
                    if (visibleRange.y >= viewportRange.x)
                        cellList[i].transform.SetAsLastSibling();
                    else
                        cellList[i].transform.SetAsFirstSibling();
                }

                // contentWidth�� �� ���� �ʺ� + spacing��ŭ ����
                contentWidth += dataList[i].cellSize.x + spacing;
            }

            // ��ũ�� ������ ��ü ������ �ʺ� ����Ʈ �ʺ񺸴� ū ���
            if (scrollRect.content.sizeDelta.x > viewportInterval)
            {
                isAtLeft = viewportRange.x + extendVisibleRange <= dataList[0].cellSize.x;
                isAtRight = scrollRect.content.sizeDelta.x - viewportRange.y + extendVisibleRange <= dataList[dataList.Count - 1].cellSize.x;
            }
            // ���� �������� ����Ʈ���� ������ ���� �� ��� true�� ����
            else
            {
                isAtLeft = true;
                isAtRight = true;
            }
        }

        /// <summary>
        /// ��ũ�� ���� ���¸� ���� ��ġ�� �޼���
        /// </summary>
        public sealed override void Refresh()
        {
            // �ʱ�ȭ ���� ��� �ʱ�ȭ
            if (!IsInitialized)
            {
                Initialize();
            }

            // ���� ����Ʈ�� �ʺ� 0�̸� �ڷ�ƾ DelayToRefresh()�� ���� ���� �����ӿ� ���ΰ�ħ�� �����մϴ�.
            if (scrollRect.viewport.rect.width == 0)
                StartCoroutine(DelayToRefresh());
            // �׷��� ������ �ٷ� DoRefresh()�� ȣ��
            else
                DoRefresh();
        }

        /// <summary>
        /// ������ ��ü�� �ʺ� �ٽ� ����ϰ�, ��� ���� ������ �� �並 ������Ʈ�ϴ� �޼���
        /// </summary>
        private void DoRefresh()
        {
            // �ʱ� �ʺ� ���
            float width = padding.x;

            // �� ������ �׸��� �� �ʺ�� spacing�� �����Ͽ� ��ü �ʺ� ���
            for (int i = 0; i < dataList.Count; i++)
            {
                width += dataList[i].cellSize.x + spacing;
            }

            // cellList�� �ִ� ��� ���� ��ȸ�ϸ鼭 RecycleCell()�� ȣ����, ���� Ȱ��ȭ�� ���� ��� Ǯ�� ȸ��
            for (int i = 0; i < cellList.Count; i++)
            {
                RecycleCell(i);
            }

            // ���� width�� padding.y ����
            width += padding.y;

            // �ش� ���� content�� sizeDelta.x�� ����
            scrollRect.content.sizeDelta = new Vector2(width, scrollRect.content.sizeDelta.y);

            // ���� ���� normalizedPosition�� ������� OnValueChanged()�� ȣ��
            OnValueChanged(scrollRect.normalizedPosition);

            // onRefresh �̺�Ʈ�� Invoke�Ͽ� �߰� ���� �۾� ����
            onRefresh?.Invoke();
        }

        /// <summary>
        /// ����Ʈ�� ũ�Ⱑ 0�� ��Ȳ�� ����, �� ������ ��� �� DoRefresh()�� ȣ���ϴ� �޼���
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayToRefresh()
        {
            yield return waitEndOfFrame;
            DoRefresh();
        }

        /// <summary>
        /// ������ �ε����� ���� �������� ��ũ�� ���� content�� �ε巴�� �̵����� �ش� ���� ����Ʈ�� ��Ÿ������ �ϴ� �޼���
        /// </summary>
        /// <param name="index"></param>
        /// <param name="duration"></param>
        public override void Snap(int index, float duration)
        {
            // �ʱ�ȭ�� �ȵǾ��ٸ� return
            if (!IsInitialized)
                return;

            // ã������ �ε����� dataList.Count���� ���ٸ� return
            if (index >= dataList.Count)
                return;

            // ������ �ʺ� ����Ʈ���� ũ�ٸ� return
            if (scrollRect.content.rect.width < scrollRect.viewport.rect.width)
                return;

            // �ʱ� �� ����
            float width = padding.x;

            // index ������ ��� ���� �ʺ�� spacing�� ������ ��ǥ x ��ġ�� ���
            for (int i = 0; i < index; i++)
            {
                width += dataList[i].cellSize.x + spacing;
            }

            // �������� ����Ʈ �ʺ� �ʰ��ϴ� �ִ� ���� ���Ͽ� width�� Ŭ����
            width = Mathf.Min(scrollRect.content.rect.width - scrollRect.viewport.rect.width, width);

            // �� ���� content�� anchoredPosition.x�� ���� width�� �ٸ��ٸ�
            if (scrollRect.content.anchoredPosition.x != width)
            {
                DoSnapping(new Vector2(-width, 0), duration);
            }
        }

        /// <summary>
        /// ������ �ε����� ������ ���� ������ ��, �������� ��ġ�� �����Ͽ� ��ũ�� �並 ������Ʈ�ϴ� �޼���
        /// </summary>
        /// <param name="index"></param>
        public override void Remove(int index)
        {
            var removeCell = dataList[index];
            base.Remove(index);
            scrollRect.content.anchoredPosition -= new Vector2(removeCell.cellSize.x + spacing, 0);
        }
    }
}