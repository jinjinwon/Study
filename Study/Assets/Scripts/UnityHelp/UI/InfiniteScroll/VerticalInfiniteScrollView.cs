using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Analytics.IAnalytic;
using UnityHelp.UI;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    /// InfiniteScrollView를 상속하여, 기본적인 무한 스크롤 로직을 활용하면서, 세로 방향(Vertical) 스크롤 뷰를 구현하는 클래스
    /// </summary>
    public class VerticalInfiniteScrollView : InfiniteScrollView
    {
        // // 콘텐츠가 스크롤뷰의 상단 또는 하단에 가까운지 여부를 나타내는 플래그 변수
        public bool isAtTop = true;
        public bool isAtBottom = true;
        
        /// <summary>
        /// 초기화 메서드
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            isAtTop = true;
            isAtBottom = true;
        }

        /// <summary>
        /// 스크롤이 이동할 때마다 호출되며, 셀을 재활용하거나 배치하는 메서드
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected override void OnValueChanged(Vector2 normalizedPosition)
        {
            // 데이터가 없으면 반환
            if (dataList.Count == 0)
                return;

            // 뷰포트 높이 계산
            float viewportInterval = scrollRect.viewport.rect.height;

            // 현재 스크롤 위치
            float minViewport = scrollRect.content.anchoredPosition.y;

            // 뷰포트의 가시 범위 (+ 여유 범위) 설정
            Vector2 viewportRange = new Vector2(minViewport - extendVisibleRange, minViewport + viewportInterval + extendVisibleRange);

            // 초기 값 세팅
            float contentHeight = padding.x;

            // dataList.Count 만큼 순회
            for (int i = 0; i < dataList.Count; i++)
            {
                // 셀의 세로 범위는 contentHeight에서 시작해 셀의 높이(cellSize.y)만큼 확장
                var visibleRange = new Vector2(contentHeight, contentHeight + dataList[i].cellSize.y);

                // 만약 셀의 visibleRange가 뷰포트 범위(viewportRange) 밖에 있다면
                if (visibleRange.y < viewportRange.x || visibleRange.x > viewportRange.y)
                {
                    RecycleCell(i);
                }
                // 다음 셀의 위치 계산
                contentHeight += dataList[i].cellSize.y + spacing;
            }

            // 초기 값 세팅
            contentHeight = padding.x;

            // dataList.Count 만큼 순회
            for (int i = 0; i < dataList.Count; i++)
            {
                // // 셀의 세로 범위는 contentHeight에서 시작해 셀의 높이(cellSize.y)만큼 확장
                var visibleRange = new Vector2(contentHeight, contentHeight + dataList[i].cellSize.y);

                // 뷰포트 범위 내에 있다면 
                if (visibleRange.y >= viewportRange.x && visibleRange.x <= viewportRange.y)
                {
                    SetupCell(i, new Vector2(0, -contentHeight));

                    // 조건에 따른 Silbing 순서 조정
                    if (visibleRange.y >= viewportRange.x)
                        cellList[i].transform.SetAsLastSibling();
                    else
                        cellList[i].transform.SetAsFirstSibling();
                }
                // 각 행마다 셀의 높이와 spacing을 더해 다음 행의 시작 위치를 결정
                contentHeight += dataList[i].cellSize.y + spacing;
            }

            // 만약 전체 콘텐츠 높이가 뷰포트 높이보다 크다면
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
        /// 스크롤 뷰의 전체 상태를 새로 고치고, 콘텐츠의 크기를 재계산한 후 셀들을 다시 배치하는 메서드
        /// </summary>
        public sealed override void Refresh()
        {
            // 초기화 되지 않았다면 초기화
            if (!IsInitialized)
            {
                Initialize();
            }

            // 만약 뷰포트의 높이가 0이면(레이아웃이 아직 완료되지 않은 경우) DelayToRefresh() 코루틴을 실행
            if (scrollRect.viewport.rect.height == 0)
                StartCoroutine(DelayToRefresh());
            else
                DoRefresh();
        }

        /// <summary>
        /// 전체 콘텐츠의 높이를 계산하고, 현재 활성화된 모든 셀을 회수한 뒤 새로 배치하는 메서드
        /// </summary>
        private void DoRefresh()
        {
            // 시작 높이를 padding.x로 설정한 후, 행 단위로 각 행의 높이(첫 번째 셀의 cellSize.y)와 spacing을 누적
            float height = padding.x;
            for (int i = 0; i < dataList.Count; i++)
            {
                height += dataList[i].cellSize.y + spacing;
            }

            // cellList에 있는 각 셀을 RecycleCell()로 회수하여 풀로 반환
            for (int i = 0; i < cellList.Count; i++)
            {
                RecycleCell(i);
            }
            // 최종 높이에 padding.y를 더해 content의 sizeDelta.y를 업데이트
            height += padding.y;
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, height);

            // OnValueChanged()를 호출하여 현재 스크롤 위치에 맞게 셀들을 재배치하고, onRefresh 이벤트를 호출
            OnValueChanged(scrollRect.normalizedPosition);
            onRefresh?.Invoke();
        }

        /// <summary>
        /// 뷰포트의 높이가 0일 때, 한 프레임 대기한 후 DoRefresh()를 호출하여 새로고침을 진행하는 메서드
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayToRefresh()
        {
            yield return waitEndOfFrame;
            DoRefresh();
        }

        /// <summary>
        /// 주어진 인덱스의 셀이 포함된 행을 기준으로, 콘텐츠를 스냅(부드러운 이동)하여 해당 행이 뷰포트에 나타나도록 하는 메서드
        /// </summary>
        /// <param name="index"></param>
        /// <param name="duration"></param>
        public override void Snap(int index, float duration)
        {

            // 초기화가 안되었다면 return
            if (!IsInitialized)
                return;

            // index 보다 dataList.Count가 작거나 같으면 return
            if (index >= dataList.Count)
                return;

            // 콘텐츠 높이가 뷰포트보다 작으면 return
            if (scrollRect.content.rect.height < scrollRect.viewport.rect.height)
                return;

            // 각 셀의 높이 (dataList[i].cellSize.y)와 셀 간 간격 (spacing)을 누적해서 계산
            float height = padding.x;
            for (int i = 0; i < index; i++)
            {
                height += dataList[i].cellSize.y + spacing;
            }

            // 목표 높이가 최대 가능한 스크롤 범위를 초과하지 않도록 제한
            height = Mathf.Min(scrollRect.content.rect.height - scrollRect.viewport.rect.height, height);

            // 현재 스크롤 위치(anchoredPosition.y)와 height 값이 다를 경우 스냅 이동 실행
            if (scrollRect.content.anchoredPosition.y != height)
            {
                DoSnapping(new Vector2(0, height), duration);
            }
        }

        /// <summary>
        /// 지정된 인덱스의 데이터 셀을 제거한 후, 콘텐츠의 위치를 조정하여 스크롤 뷰를 업데이트하는 메서드
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