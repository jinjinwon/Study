using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;
using UnityEngine.UI;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    /// InfiniteScrollView를 상속받아 수평 방향의 그리드 형태 무한 스크롤 뷰를 구현하는 클래스
    /// </summary>
    public class HorizontalGridInfiniteScrollView : InfiniteScrollView
    {
        // 한 열에 들어갈 셀의 개수
        public int rowCount = 1;

        // 스크롤 뷰의 콘텐츠가 좌측 또는 우측 끝에 가까운지 여부를 나타내는 플래그 변수
        public bool isAtLeft = true;
        public bool isAtRight = true;

        /// <summary>
        /// 스크롤 뷰의 스크롤 위치가 변경될 때마다 호출되어 셀을 재활용하거나 설정하는 핵심 로직
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected override void OnValueChanged(Vector2 normalizedPosition)
        {
            // 0이라면 1로 바꿔줌
            if (rowCount <= 0)
            {
                rowCount = 1;
            }

            // 뷰포트의 가시 너비를 저장
            float viewportInterval = scrollRect.viewport.rect.width;

            // 음수를 반전하여 뷰포트의 실제 시작 위치를 계산
            float minViewport = -scrollRect.content.anchoredPosition.x;

            // 여유 영역을 포함해 뷰포트의 좌우 범위를 확장
            Vector2 viewportRange = new Vector2(minViewport - extendVisibleRange, minViewport + viewportInterval + extendVisibleRange);

            // 초기 값 세팅
            float contentWidth = padding.x;

            /// 셀 재활용
            
            // rowCount 단위로 데이터를 순회
            for (int i = 0; i < dataList.Count; i += rowCount)
            {
                // 각 열 내 rowCount만큼 셀을 순회
                for (int j = 0; j < rowCount; j++)
                {
                    int index = i + j;

                    // 인덱스가 dataList.Count보다 크다면 return
                    if (index >= dataList.Count)
                        break;

                    // 각 셀이 차지하는 가로 범위는 (contentWidth, contentWidth + 셀의 너비)로 계산
                    var visibleRange = new Vector2(contentWidth, contentWidth + dataList[index].cellSize.x);

                    // 해당 셀의 visibleRange가 뷰포트의 확장 범위 밖에 있다면, 셀을 RecycleCell()을 통해 비활성화하고 풀로 회수
                    if (visibleRange.y < viewportRange.x || visibleRange.x > viewportRange.y)
                    {
                        RecycleCell(index);
                    }
                }

                // 한 열의 첫 셀의 너비와 spacing만큼 contentWidth를 증가시켜 다음 열의 시작 위치를 계산
                contentWidth += dataList[i].cellSize.x + spacing;
            }

            // 초기 값 세팅
            contentWidth = padding.x;

            /// 셀 설정

            // rowCount 단위로 데이터를 순회
            for (int i = 0; i < dataList.Count; i += rowCount)
            {
                // 각 열 내 rowCount만큼 셀을 순회
                for (int j = 0; j < rowCount; j++)
                {
                    int index = i + j;

                    // index가 dataList.Count 보다 크다면 return
                    if (index >= dataList.Count)
                        break;

                    // 각 셀이 차지하는 가로 범위는 (contentWidth, contentWidth + 셀의 너비)로 계산
                    var visibleRange = new Vector2(contentWidth, contentWidth + dataList[index].cellSize.x);

                    // 각 셀의 visibleRange가 뷰포트 범위 안에 있다면 SetupCell()을 호출해 셀을 활성화하고 배치
                    if (visibleRange.y >= viewportRange.x && visibleRange.x <= viewportRange.y)
                    {
                        SetupCell(index, new Vector2(contentWidth, -padding.y + (dataList[index].cellSize.y + spacing) * -j));

                        // 조건에 따라 Sibling 순서 조정
                        if (visibleRange.y >= viewportRange.x)
                            cellList[index].transform.SetAsLastSibling();
                        else
                            cellList[index].transform.SetAsFirstSibling();
                    }
                }
                // 다음 열을 위해 contentWidth에 해당 열의 첫 셀의 너비와 spacing을 더함
                contentWidth += dataList[i].cellSize.x + spacing;
            }

            // 스크롤 가능한 전체 콘텐츠 너비가 뷰포트 너비보다 크면 좌우 끝 상태를 개별적으로 계산
            if (scrollRect.content.sizeDelta.x > viewportInterval)
            {
                isAtLeft = viewportRange.x + extendVisibleRange <= dataList[0].cellSize.x;
                isAtRight = scrollRect.content.sizeDelta.x - viewportRange.y + extendVisibleRange <= dataList[dataList.Count - 1].cellSize.x;
            }
            // 콘텐츠가 뷰포트보다 작으면, 양쪽 끝 상태를 모두 true로 설정
            else
            {
                isAtLeft = true;
                isAtRight = true;
            }
        }

        /// <summary>
        /// 스크롤 뷰의 상태를 새로 고침
        /// </summary>
        public sealed override void Refresh()
        {
            // 초기화 하지 않았다면 초기화
            if (!IsInitialized)
            {
                Initialize();
            }

            // 만약 뷰포트의 너비가 0이면 DelayToRefresh() 코루틴을 시작
            if (scrollRect.viewport.rect.width == 0)
                StartCoroutine(DelayToRefresh());
            else
                DoRefresh();
        }

        /// <summary>
        /// 콘텐츠 전체 너비를 다시 계산하고, 모든 셀을 리셋한 후 스크롤 뷰를 업데이트하는 메서드
        /// </summary>
        private void DoRefresh()
        {
            // 초기 값 세팅
            float width = padding.x;

            // 데이터 리스트를 rowCount 단위로 순회하며 각 열의 첫 셀의 너비와 spacing을 누적
            for (int i = 0; i < dataList.Count; i += rowCount)
            {
                width += dataList[i].cellSize.x + spacing;
            }

            // cellList에 있는 모든 셀을 RecycleCell() 호출로 회수하여 풀로 돌려보내기
            for (int i = 0; i < cellList.Count; i++)
            {
                RecycleCell(i);
            }

            // width에 padding.y를 더해 scrollRect.content.sizeDelta.x를 설정
            width += padding.y;
            scrollRect.content.sizeDelta = new Vector2(width, scrollRect.content.sizeDelta.y);

            // OnValueChanged()를 호출하여 새로 계산된 normalizedPosition을 바탕으로 셀들을 재배치하고, onRefresh 이벤트를 호출해 추가 작업 진행
            OnValueChanged(scrollRect.normalizedPosition);
            onRefresh?.Invoke();
        }

        /// <summary>
        /// 뷰포트의 크기가 아직 0일 때, 한 프레임 대기 후 DoRefresh()를 호출하여 새로고침을 진행하는 코루틴
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayToRefresh()
        {
            yield return waitEndOfFrame;
            DoRefresh();
        }

        /// <summary>
        /// 특정 인덱스의 셀을 기준으로 콘텐츠를 스냅하여 해당 셀이 뷰포트에 나타나도록 하는 메서드
        /// </summary>
        /// <param name="index"></param>
        /// <param name="duration"></param>
        public override void Snap(int index, float duration)
        {
            // 초기화가 안되었다면 return
            if (!IsInitialized)
                return;

            // index가 dataList.Count보다 크다면 return
            if (index >= dataList.Count)
                return;

            // 주어진 index를 rowCount로 나누어, 몇 번째 열에 위치하는지 계산
            var columeNumber = index / rowCount;

            // 초기 값 세팅
            var width = padding.x;

            // 해당 열 이전의 모든 열의 첫 셀 너비와 spacing을 누적하여 목표 x 위치를 계산
            for (int i = 0; i < columeNumber; i++)
            {
                width += dataList[i * rowCount].cellSize.x + spacing;
            }

            // width가 콘텐츠의 최대 이동 가능 범위(전체 콘텐츠 너비 - 뷰포트 너비)를 넘지 않도록 Mathf.Min을 사용해 제한
            width = Mathf.Min(scrollRect.content.rect.width - scrollRect.viewport.rect.width, width);

            // 현재 콘텐츠의 anchoredPosition.x와 계산된 width가 다르면, DoSnapping()을 호출하여 target 위치로 부드럽게 이동
            // DoSnapping() 호출 시 x 값은 음수로 전달되는데, 이는 스크롤 콘텐츠가 왼쪽으로 이동할 때 음수 값을 갖기 때문임
            if (scrollRect.content.anchoredPosition.x != width)
            {
                DoSnapping(new Vector2(-width, 0), duration);
            }
        }
    }
}

