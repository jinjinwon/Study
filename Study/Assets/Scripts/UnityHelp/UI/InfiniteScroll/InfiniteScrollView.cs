using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Diagnostics;

namespace UnityHelp.UI.InfiniteScroll
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class InfiniteScrollView : UIBehaviour
    {
        public int cellPoolSize = 20;                                                               // ũ�� ����
        public float spacing = 0f;                                                                  // �� ���� ����
        public Vector2 padding;                                                                     // ���� ��� ���� ����
        public float extendVisibleRange;                                                            // ���� ����

        public InfiniteCell cellPrefab;                                                             // �� ������
        public ScrollRect scrollRect;                                                               // ScrollRect
        public List<InfiniteCellData> dataList = new List<InfiniteCellData>();                      // ������ �� ����Ʈ
        public List<InfiniteCell> cellList = new List<InfiniteCell>();                              // ���� �� �ν��Ͻ� ����Ʈ
        protected Queue<InfiniteCell> cellPool = new Queue<InfiniteCell>();                         // ���� �����ϱ� ���� ť
        protected YieldInstruction waitEndOfFrame = new WaitForEndOfFrame();                        // �ڷ�ƾ���� ����ϱ� ���� �ν��Ͻ�
        private Coroutine snappingProcesser;                                                        // ���� �ִϸ��̼� �ڷ�ƾ
        public event Action onRectTransformUpdate;                                                  // Rect ������Ʈ �� ȣ�� �̺�Ʈ
        public event Action<InfiniteCell> onCellSelected;                                           // �� ���� �̺�Ʈ
        public Action onRefresh;                                                                    // ��ũ�� �� ������Ʈ �̺�Ʈ

        /// <summary>
        /// ��ũ�� �䰡 �ʱ�ȭ �Ǿ������� ���� ������Ƽ
        /// </summary>
        public bool IsInitialized
        {
            get;
            private set;
        }

        /// <summary>
        /// �ʱ�ȭ �Լ�
        /// </summary>
        public virtual void Initialize()
        {
            if (IsInitialized)
                return;
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener(OnValueChanged);
            for (int i = 0; i < cellPoolSize; i++)
            {
                var newCell = Instantiate(cellPrefab, scrollRect.content);
                newCell.gameObject.SetActive(false);
                cellPool.Enqueue(newCell);
            }
            IsInitialized = true;
        }

        /// <summary>
        /// ��ũ�� ��ġ�� ����� �� ȣ��Ǵ� �޼���
        /// �ڽ� Ŭ�������� ��ü���� �� ��ġ �� ��Ȱ�� ������ �����ϱ� ���� ����
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected abstract void OnValueChanged(Vector2 normalizedPosition);

        /// <summary>
        /// ��ũ�� ���� ���¸� ���� ��ġ�� ����
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Data �߰� �޼���
        /// </summary>
        /// <param name="data"></param>
        public virtual void Add(InfiniteCellData data)
        {
            // �ʱ�ȭ�� ���� �ʾҴٸ� IsInitialized ȣ��
            if (!IsInitialized)
            {
                Initialize();
            }

            // �߰��Ǿ����� Count�� Index�� ����
            data.index = dataList.Count;

            // dataList ������ �߰�
            dataList.Add(data);

            // cellList null �߰� �� �Ҵ� ���� ������ ǥ��
            cellList.Add(null);
        }

        /// <summary>
        /// Data ���� �޼���
        /// </summary>
        /// <param name="index"></param>
        public virtual void Remove(int index)
        {
            // �ʱ�ȭ�� ���� �ʾҴٸ� IsInitialized ȣ��
            if (!IsInitialized)
            {
                Initialize();
            }

            // ������ ����Ʈ�� ������ 0�̶��
            if (dataList.Count == 0)
                return;

            // �ش� �ε����� ������ ����
            dataList.RemoveAt(index);

            // UI ������Ʈ
            Refresh();
        }

        /// <summary>
        /// Ư�� �ε����� ���� ��ũ���� �����ϴ� ����� ����
        /// </summary>
        /// <param name="index"></param>
        /// <param name="duration"></param>
        public abstract void Snap(int index, float duration);

        /// <summary>
        /// ������ ������ ���� �ε巴�� �̵���Ű�� �޼���
        /// </summary>
        /// <param name="duration"></param>
        public void SnapLast(float duration)
        {
            // dataList�� ������ �ε����� ����Ͽ� snap �Լ� ȣ��
            Snap(dataList.Count - 1, duration);
        }

        /// <summary>
        /// ���� �ִϸ��̼��� �����ϴ� �Լ���, content�� anchoredPosition�� target ��ġ�� duration �ð� ���� �ε巴�� �̵���ŵ�ϴ�.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        protected void DoSnapping(Vector2 target, float duration)
        {
            // ������Ʈ�� ��Ȱ��ȭ ���¶�� ����
            if (!gameObject.activeInHierarchy)
                return;

            // ���� ���� �ߴ�
            StopSnapping();

            // ���ο� �ڷ�ƾ�� �����ϰ� ����
            snappingProcesser = StartCoroutine(ProcessSnapping(target, duration));
        }

        /// <summary>
        /// ���� ���� ���� �ڷ�ƾ ����
        /// </summary>
        public void StopSnapping()
        {
            // �������� �ڷ�ƾ�� �ִٸ� �����ϰ� ������ null�� ����
            if (snappingProcesser != null)
            {
                StopCoroutine(snappingProcesser);
                snappingProcesser = null;
            }
        }

        /// <summary>
        /// ���� �ִϸ��̼��� �����ϴ� �ڷ�ƾ
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator ProcessSnapping(Vector2 target, float duration)
        {
            // ScrollRect �ӵ� 0
            scrollRect.velocity = Vector2.zero;

            Vector2 startPos = scrollRect.content.anchoredPosition;
            float t = 0;

            // ���� ����
            while (t < 1f)
            {
                if (duration <= 0)
                    t = 1;
                else
                    t += Time.deltaTime / duration;

                scrollRect.content.anchoredPosition = Vector2.Lerp(startPos, target, t);
                var normalizedPos = scrollRect.normalizedPosition;
                if (normalizedPos.y < 0 || normalizedPos.x > 1)
                {
                    break;
                }
                yield return null;
            }
            if (duration <= 0)
                OnValueChanged(scrollRect.normalizedPosition);
            snappingProcesser = null;
        }

        /// <summary>
        /// Ư�� �ε����� �ش��ϴ� ���� Ǯ���� ���� ������ �Ҵ� �� ������ ��ġ�� ��ġ�ϴ� �޼���
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pos"></param>
        protected void SetupCell(int index, Vector2 pos)
        {
            if (cellList[index] == null)
            {
                var cell = cellPool.Dequeue();
                cell.gameObject.SetActive(true);
                cell.CellData = dataList[index];
                cell.RectTransform.anchoredPosition = pos;
                cellList[index] = cell;
                cell.onSelected += OnCellSelected;
            }
        }

        /// <summary>
        /// �� �̻� ȭ�鿡 ǥ���� �ʿ䰡 ���� ���� ȸ���ϰ� Ǯ�� ���������� �޼���
        /// </summary>
        /// <param name="index"></param>
        protected void RecycleCell(int index)
        {
            if (cellList[index] != null)
            {
                var cell = cellList[index];
                cellList[index] = null;
                cellPool.Enqueue(cell);
                cell.gameObject.SetActive(false);
                cell.onSelected -= OnCellSelected;
            }
        }

        /// <summary>
        /// ���� ���õ� �� ȣ��Ǵ� �޼���
        /// </summary>
        /// <param name="selectedCell"></param>
        private void OnCellSelected(InfiniteCell selectedCell)
        {
            onCellSelected?.Invoke(selectedCell);
        }

        /// <summary>
        /// ��ũ�� ���� ��� �����͸� �����ϰ� ���� �������� ���¸� �ʱ�ȭ�ϴ� �޼���
        /// </summary>
        public virtual void Clear()
        {
            if (IsInitialized == false)
                Initialize();
            scrollRect.velocity = Vector2.zero;
            scrollRect.content.anchoredPosition = Vector2.zero;
            dataList.Clear();
            for (int i = 0; i < cellList.Count; i++)
            {
                RecycleCell(i);
            }
            cellList.Clear();
            Refresh();
        }

        /// <summary>
        /// UI�� Rect ����� �� �ڵ�  ȣ��Ǵ� �޼���
        /// </summary>
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (scrollRect)
            {
                onRectTransformUpdate?.Invoke();
            }
        }
    }
}