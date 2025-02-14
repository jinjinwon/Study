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
        public int cellPoolSize = 20;                                                               // 크기 지정
        public float spacing = 0f;                                                                  // 셀 간의 간격
        public Vector2 padding;                                                                     // 좌측 사단 여백 조절
        public float extendVisibleRange;                                                            // 여유 범위

        public InfiniteCell cellPrefab;                                                             // 셀 프리팹
        public ScrollRect scrollRect;                                                               // ScrollRect
        public List<InfiniteCellData> dataList = new List<InfiniteCellData>();                      // 데이터 모델 리스트
        public List<InfiniteCell> cellList = new List<InfiniteCell>();                              // 현재 셀 인스턴스 리스트
        protected Queue<InfiniteCell> cellPool = new Queue<InfiniteCell>();                         // 셀을 관리하기 위한 큐
        protected YieldInstruction waitEndOfFrame = new WaitForEndOfFrame();                        // 코루틴에서 사용하기 위한 인스턴스
        private Coroutine snappingProcesser;                                                        // 스냅 애니메이션 코루틴
        public event Action onRectTransformUpdate;                                                  // Rect 업데이트 시 호출 이벤트
        public event Action<InfiniteCell> onCellSelected;                                           // 셀 선택 이벤트
        public Action onRefresh;                                                                    // 스크롤 뷰 업데이트 이벤트

        /// <summary>
        /// 스크롤 뷰가 초기화 되었는지에 대한 프로퍼티
        /// </summary>
        public bool IsInitialized
        {
            get;
            private set;
        }

        /// <summary>
        /// 초기화 함수
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
        /// 스크롤 위치가 변경될 때 호출되는 메서드
        /// 자식 클래스에서 구체적인 셀 배치 및 재활용 로직을 강요하기 위한 ㅇㅇ
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected abstract void OnValueChanged(Vector2 normalizedPosition);

        /// <summary>
        /// 스크롤 뷰의 상태를 새로 고치는 역할
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Data 추가 메서드
        /// </summary>
        /// <param name="data"></param>
        public virtual void Add(InfiniteCellData data)
        {
            // 초기화가 되지 않았다면 IsInitialized 호출
            if (!IsInitialized)
            {
                Initialize();
            }

            // 추가되었으니 Count를 Index로 설정
            data.index = dataList.Count;

            // dataList 데이터 추가
            dataList.Add(data);

            // cellList null 추가 및 할당 되지 않음을 표시
            cellList.Add(null);
        }

        /// <summary>
        /// Data 삭제 메서드
        /// </summary>
        /// <param name="index"></param>
        public virtual void Remove(int index)
        {
            // 초기화가 되지 않았다면 IsInitialized 호출
            if (!IsInitialized)
            {
                Initialize();
            }

            // 데이터 리스트의 개수가 0이라면
            if (dataList.Count == 0)
                return;

            // 해당 인덱스의 데이터 제거
            dataList.RemoveAt(index);

            // UI 업데이트
            Refresh();
        }

        /// <summary>
        /// 특정 인덱스의 셀로 스크롤을 스냅하는 기능을 구현
        /// </summary>
        /// <param name="index"></param>
        /// <param name="duration"></param>
        public abstract void Snap(int index, float duration);

        /// <summary>
        /// 마지막 데이터 셀로 부드럽게 이동시키는 메서드
        /// </summary>
        /// <param name="duration"></param>
        public void SnapLast(float duration)
        {
            // dataList의 마지막 인덱스를 계산하여 snap 함수 호출
            Snap(dataList.Count - 1, duration);
        }

        /// <summary>
        /// 스냅 애니메이션을 시작하는 함수로, content의 anchoredPosition을 target 위치로 duration 시간 동안 부드럽게 이동시킵니다.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        protected void DoSnapping(Vector2 target, float duration)
        {
            // 오브젝트가 비활성화 상태라면 안함
            if (!gameObject.activeInHierarchy)
                return;

            // 기존 스냅 중단
            StopSnapping();

            // 새로운 코루틴을 시작하고 저장
            snappingProcesser = StartCoroutine(ProcessSnapping(target, duration));
        }

        /// <summary>
        /// 진행 중인 스냅 코루틴 정지
        /// </summary>
        public void StopSnapping()
        {
            // 진행중인 코루틴이 있다면 정지하고 참조를 null로 설정
            if (snappingProcesser != null)
            {
                StopCoroutine(snappingProcesser);
                snappingProcesser = null;
            }
        }

        /// <summary>
        /// 스냅 애니메이션을 진행하는 코루틴
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator ProcessSnapping(Vector2 target, float duration)
        {
            // ScrollRect 속도 0
            scrollRect.velocity = Vector2.zero;

            Vector2 startPos = scrollRect.content.anchoredPosition;
            float t = 0;

            // 보간 진행
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
        /// 특정 인덱스에 해당하는 셀을 풀에서 꺼내 데이터 할당 후 지정된 위치에 배치하는 메서드
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
        /// 더 이상 화면에 표시할 필요가 없는 셀을 회수하고 풀로 돌려보내는 메서드
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
        /// 셀이 선택될 시 호출되는 메서드
        /// </summary>
        /// <param name="selectedCell"></param>
        private void OnCellSelected(InfiniteCell selectedCell)
        {
            onCellSelected?.Invoke(selectedCell);
        }

        /// <summary>
        /// 스크롤 뷰의 모든 데이터를 삭제하고 셀과 컨텐츠의 상태를 초기화하는 메서드
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
        /// UI의 Rect 변경될 때 자동  호출되는 메서드
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