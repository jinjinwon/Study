using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Analytics.IAnalytic;
using UnityHelp.UI;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    /// InfiniteScrollView�� ����Ͽ�, �⺻���� ���� ��ũ�� ������ Ȱ���ϸ鼭, ���� ����(Vertical) ��ũ�� �並 �����ϴ� Ŭ����
    /// </summary>
    public class VerticalInfiniteScrollView : InfiniteScrollView
    {
        // // �������� ��ũ�Ѻ��� ��� �Ǵ� �ϴܿ� ������� ���θ� ��Ÿ���� �÷��� ����
        public bool isAtTop = true;
        public bool isAtBottom = true;
        
        /// <summary>
        /// �ʱ�ȭ �޼���
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            isAtTop = true;
            isAtBottom = true;
        }

        /// <summary>
        /// ��ũ���� �̵��� ������ ȣ��Ǹ�, ���� ��Ȱ���ϰų� ��ġ�ϴ� �޼���
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected override void OnValueChanged(Vector2 normalizedPosition)
        {
            // �����Ͱ� ������ ��ȯ
            if (dataList.Count == 0)
                return;

            // ����Ʈ ���� ���
            float viewportInterval = scrollRect.viewport.rect.height;

            // ���� ��ũ�� ��ġ
            float minViewport = scrollRect.content.anchoredPosition.y;

            // ����Ʈ�� ���� ���� (+ ���� ����) ����
            Vector2 viewportRange = new Vector2(minViewport - extendVisibleRange, minViewport + viewportInterval + extendVisibleRange);

            // �ʱ� �� ����
            float contentHeight = padding.x;

            // dataList.Count ��ŭ ��ȸ
            for (int i = 0; i < dataList.Count; i++)
            {
                // ���� ���� ������ contentHeight���� ������ ���� ����(cellSize.y)��ŭ Ȯ��
                var visibleRange = new Vector2(contentHeight, contentHeight + dataList[i].cellSize.y);

                // ���� ���� visibleRange�� ����Ʈ ����(viewportRange) �ۿ� �ִٸ�
                if (visibleRange.y < viewportRange.x || visibleRange.x > viewportRange.y)
                {
                    RecycleCell(i);
                }
                // ���� ���� ��ġ ���
                contentHeight += dataList[i].cellSize.y + spacing;
            }

            // �ʱ� �� ����
            contentHeight = padding.x;

            // dataList.Count ��ŭ ��ȸ
            for (int i = 0; i < dataList.Count; i++)
            {
                // // ���� ���� ������ contentHeight���� ������ ���� ����(cellSize.y)��ŭ Ȯ��
                var visibleRange = new Vector2(contentHeight, contentHeight + dataList[i].cellSize.y);

                // ����Ʈ ���� ���� �ִٸ� 
                if (visibleRange.y >= viewportRange.x && visibleRange.x <= viewportRange.y)
                {
                    SetupCell(i, new Vector2(0, -contentHeight));

                    // ���ǿ� ���� Silbing ���� ����
                    if (visibleRange.y >= viewportRange.x)
                        cellList[i].transform.SetAsLastSibling();
                    else
                        cellList[i].transform.SetAsFirstSibling();
                }
                // �� �ึ�� ���� ���̿� spacing�� ���� ���� ���� ���� ��ġ�� ����
                contentHeight += dataList[i].cellSize.y + spacing;
            }

            // ���� ��ü ������ ���̰� ����Ʈ ���̺��� ũ�ٸ�
            if (scrollRect.content.sizeDelta.y > viewportInterval)
            {
                isAtTop = viewportRange.x + extendVisibleRange <= 0.001f;
                isAtBottom = scrollRect.content.sizeDelta.y - viewportRange.y + extendVisibleRange <= 0.001f;
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
            for (int i = 0; i < dataList.Count; i++)
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

            // ������ ���̰� ����Ʈ���� ������ return
            if (scrollRect.content.rect.height < scrollRect.viewport.rect.height)
                return;

            // �� ���� ���� (dataList[i].cellSize.y)�� �� �� ���� (spacing)�� �����ؼ� ���
            float height = padding.x;
            for (int i = 0; i < index; i++)
            {
                height += dataList[i].cellSize.y + spacing;
            }

            // ��ǥ ���̰� �ִ� ������ ��ũ�� ������ �ʰ����� �ʵ��� ����
            height = Mathf.Min(scrollRect.content.rect.height - scrollRect.viewport.rect.height, height);

            // ���� ��ũ�� ��ġ(anchoredPosition.y)�� height ���� �ٸ� ��� ���� �̵� ����
            if (scrollRect.content.anchoredPosition.y != height)
            {
                DoSnapping(new Vector2(0, height), duration);
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
            scrollRect.content.anchoredPosition -= new Vector2(0, removeCell.cellSize.y + spacing);
        }
    }
}