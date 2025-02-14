using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Analytics.IAnalytic;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    /// 무한 스크롤 뷰의 기본 기능을 제공하는 InfiniteScrollView를 상속 받아 수평 스크롤 전용으로 동작하는 클래스
    /// </summary>
    public class HorizontalInfiniteScrollView : InfiniteScrollView
    {
        public bool isAtLeft = true;                // 스크롤 뷰가 왼쪽 끝에 가까운지 여부
        public bool isAtRight = true;               // 스크롤 뷰가 오른쪽 끝에 가까운지 여부

        /// <summary>
        /// 초기화 메서드
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            isAtLeft = true;
            isAtRight = true;
        }

        /// <summary>
        /// 스크롤 뷰의 수평 스크롤 위치가 바뀔 때마다 호출되는 메서드
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected override void OnValueChanged(Vector2 normalizedPosition)
        {
            // dataList가 없다면
            if (dataList.Count == 0)
                return;

            /// 뷰포트 영역 계산
            // 너비 계산
            float viewportInterval = scrollRect.viewport.rect.width;        

            // x좌표를 반전하여 뷰포트의 현재 왼쪽 경계 위치
            float minViewport = -scrollRect.content.anchoredPosition.x;     

            // 실제 가시 범위에 여유 영역을 더한 범위
            Vector2 viewportRange = new Vector2(minViewport - extendVisibleRange, minViewport + viewportInterval + extendVisibleRange);

            // 위치 리셋
            float contentWidth = padding.x;                                 

            /// 셀 재활용
            for (int i = 0; i < dataList.Count; i++)
            {
                // 각 데이터 항목에 대해, 해당 셀이 차지하는 가로 영역(visibleRange)을 계산
                var visibleRange = new Vector2(contentWidth, contentWidth + dataList[i].cellSize.x);

                // 만약 이 visibleRange가 뷰포트 범위밖에 있다면, RecycleCell(i)를 호출하여 해당 셀을 회수
                if (visibleRange.y < viewportRange.x || visibleRange.x > viewportRange.y)
                {
                    RecycleCell(i);
                }

                // 각 셀의 너비와 spacing만큼 contentWidth를 증가시켜 다음 셀의 시작 위치를 계산합니다.
                contentWidth += dataList[i].cellSize.x + spacing;
            }

            // 다시 위치 리셋
            contentWidth = padding.x;             
            
            /// 셀 설정
            for (int i = 0; i < dataList.Count; i++)
            {
                // 각 데이터 항목에 대해, 해당 셀이 차지하는 가로 영역(visibleRange)을 계산
                var visibleRange = new Vector2(contentWidth, contentWidth + dataList[i].cellSize.x);

                // visibleRange가 뷰포트 내에 존재하면 SetupCell(i, new Vector2(contentWidth, 0))를 호출하여 해당 셀을 활성화하고 지정된 위치에 배치.
                if (visibleRange.y >= viewportRange.x && visibleRange.x <= viewportRange.y)
                {
                    SetupCell(i, new Vector2(contentWidth, 0));

                    // 조건에 따라 sibling 순서를 조정
                    if (visibleRange.y >= viewportRange.x)
                        cellList[i].transform.SetAsLastSibling();
                    else
                        cellList[i].transform.SetAsFirstSibling();
                }

                // contentWidth를 각 셀의 너비 + spacing만큼 증가
                contentWidth += dataList[i].cellSize.x + spacing;
            }

            // 스크롤 가능한 전체 콘텐츠 너비가 뷰포트 너비보다 큰 경우
            if (scrollRect.content.sizeDelta.x > viewportInterval)
            {
                isAtLeft = viewportRange.x + extendVisibleRange <= dataList[0].cellSize.x;
                isAtRight = scrollRect.content.sizeDelta.x - viewportRange.y + extendVisibleRange <= dataList[dataList.Count - 1].cellSize.x;
            }
            // 만약 콘텐츠가 뷰포트보다 작으면 양쪽 끝 모두 true로 설정
            else
            {
                isAtLeft = true;
                isAtRight = true;
            }
        }

        /// <summary>
        /// 스크롤 뷰의 상태를 새로 고치는 메서드
        /// </summary>
        public sealed override void Refresh()
        {
            // 초기화 안한 경우 초기화
            if (!IsInitialized)
            {
                Initialize();
            }

            // 만약 뷰포트의 너비가 0이면 코루틴 DelayToRefresh()를 통해 다음 프레임에 새로고침을 진행합니다.
            if (scrollRect.viewport.rect.width == 0)
                StartCoroutine(DelayToRefresh());
            // 그렇지 않으면 바로 DoRefresh()를 호출
            else
                DoRefresh();
        }

        /// <summary>
        /// 콘텐츠 전체의 너비를 다시 계산하고, 모든 셀을 리셋한 후 뷰를 업데이트하는 메서드
        /// </summary>
        private void DoRefresh()
        {
            // 초기 너비 계산
            float width = padding.x;

            // 각 데이터 항목의 셀 너비와 spacing을 누적하여 전체 너비를 계산
            for (int i = 0; i < dataList.Count; i++)
            {
                width += dataList[i].cellSize.x + spacing;
            }

            // cellList에 있는 모든 셀을 순회하면서 RecycleCell()을 호출해, 현재 활성화된 셀을 모두 풀로 회수
            for (int i = 0; i < cellList.Count; i++)
            {
                RecycleCell(i);
            }

            // 계산된 width에 padding.y 더함
            width += padding.y;

            // 해당 값을 content의 sizeDelta.x로 설정
            scrollRect.content.sizeDelta = new Vector2(width, scrollRect.content.sizeDelta.y);

            // 새로 계산된 normalizedPosition을 기반으로 OnValueChanged()를 호출
            OnValueChanged(scrollRect.normalizedPosition);

            // onRefresh 이벤트를 Invoke하여 추가 갱신 작업 실행
            onRefresh?.Invoke();
        }

        /// <summary>
        /// 뷰포트의 크기가 0인 상황에 대해, 한 프레임 대기 후 DoRefresh()를 호출하는 메서드
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayToRefresh()
        {
            yield return waitEndOfFrame;
            DoRefresh();
        }

        /// <summary>
        /// 지정된 인덱스의 셀을 기준으로 스크롤 뷰의 content를 부드럽게 이동시켜 해당 셀이 뷰포트에 나타나도록 하는 메서드
        /// </summary>
        /// <param name="index"></param>
        /// <param name="duration"></param>
        public override void Snap(int index, float duration)
        {
            // 초기화가 안되었다면 return
            if (!IsInitialized)
                return;

            // 찾으려는 인덱스가 dataList.Count보다 높다면 return
            if (index >= dataList.Count)
                return;

            // 콘텐츠 너비가 뷰포트보다 크다면 return
            if (scrollRect.content.rect.width < scrollRect.viewport.rect.width)
                return;

            // 초기 값 세팅
            float width = padding.x;

            // index 이전의 모든 셀의 너비와 spacing을 누적해 목표 x 위치를 계산
            for (int i = 0; i < index; i++)
            {
                width += dataList[i].cellSize.x + spacing;
            }

            // 콘텐츠가 뷰포트 너비를 초과하는 최대 값과 비교하여 width를 클램프
            width = Mathf.Min(scrollRect.content.rect.width - scrollRect.viewport.rect.width, width);

            // 약 현재 content의 anchoredPosition.x가 계산된 width와 다르다면
            if (scrollRect.content.anchoredPosition.x != width)
            {
                DoSnapping(new Vector2(-width, 0), duration);
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
            scrollRect.content.anchoredPosition -= new Vector2(removeCell.cellSize.x + spacing, 0);
        }
    }
}