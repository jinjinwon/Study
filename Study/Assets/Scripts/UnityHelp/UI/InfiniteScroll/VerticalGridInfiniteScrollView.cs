using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Analytics.IAnalytic;
using UnityHelp.UI;

namespace UnityHelp.UI.InfiniteScroll
{
    /// <summary>
    ///  InfiniteScrollView의 공통 기능(셀 풀 관리, Refresh, Snap 등)을 상속받아 수직 방향의 그리드 형태 무한 스크롤을 구현하는 클래스
    /// </summary>
    public class VerticalGridInfiniteScrollView : InfiniteScrollView
    {
        // 콘텐츠가 스크롤뷰의 상단 또는 하단에 가까운지 여부를 나타내는 플래그 변수
        public bool isAtTop = true;
        public bool isAtBottom = true;

        // 한 행(row)에 들어가는 셀(열)의 수를 의미하는 변수
        public int columeCount = 1;

        /// <summary>
        /// 스크롤뷰의 스크롤 위치가 바뀔 때마다 호출되어, 보이는 셀들을 새로 설정하거나 화면 밖에 있는 셀들을 회수하는 메서드
        /// </summary>
        /// <param name="normalizedPosition"></param>
        protected override void OnValueChanged(Vector2 normalizedPosition)
        {
            // 0이라면 1로 고정
            if (columeCount <= 0)
            {
                columeCount = 1;
            }

            // 뷰포트의 높이를 가져옴
            float viewportInterval = scrollRect.viewport.rect.height;

            // 콘텐츠의 현재 anchoredPosition.y 값을 사용하여, 스크롤된 수직 오프셋 얻기
            float minViewport = scrollRect.content.anchoredPosition.y;

            // 현재 뷰포트의 상단과 하단 위치를 정의
            Vector2 viewportRange = new Vector2(minViewport, minViewport + viewportInterval);

            // 초기 값 세팅
            float contentHeight = padding.x;

            /// 셀 회수

            // 외부 루프는 한 행씩 처리하며, 인덱스를 columeCount 단위로 증가
            for (int i = 0; i < dataList.Count; i += columeCount)
            {
                // 각 행에 있는 셀들을 처리
                for (int j = 0; j < columeCount; j++)
                {
                    int index = i + j;

                    // index가 dataList.Count보다 크거나 같다면
                    if (index >= dataList.Count)
                        break;

                    // 셀의 세로 범위는 contentHeight에서 시작해 셀의 높이(cellSize.y)만큼 확장
                    var visibleRange = new Vector2(contentHeight, contentHeight + dataList[index].cellSize.y);

                    // 만약 셀의 visibleRange가 뷰포트 범위(viewportRange) 밖에 있다면
                    if (visibleRange.y < viewportRange.x || visibleRange.x > viewportRange.y)
                    {
                        RecycleCell(index);
                    }
                }

                // 한 행의 첫 번째 셀의 높이와 spacing을 더하여 다음 행의 시작 위치를 계산
                contentHeight += dataList[i].cellSize.y + spacing;
            }

            // 초기 값 세팅
            contentHeight = padding.x;

            /// 셀 설정

            // 외부 루프는 한 행씩 처리하며, 인덱스를 columeCount 단위로 증가
            for (int i = 0; i < dataList.Count; i += columeCount)
            {
                // 각 행에 있는 셀들을 처리
                for (int j = 0; j < columeCount; j++)
                {
                    int index = i + j;

                    // index가 dataList.Count보다 크거나 같다면
                    if (index >= dataList.Count)
                        break;

                    // 셀의 세로 범위는 contentHeight에서 시작해 셀의 높이(cellSize.y)만큼 확장
                    var visibleRange = new Vector2(contentHeight, contentHeight + dataList[index].cellSize.y);

                    // 뷰포트 범위 내에 있다면 
                    if (visibleRange.y >= viewportRange.x && visibleRange.x <= viewportRange.y)
                    {
                        // x 좌표: (cellSize.x + spacing) * j → 열 번호(j)에 따라 좌우 배치
                        // y 좌표: -contentHeight → 상단에서 아래로 내려가는 방향으로 배치.
                        SetupCell(index, new Vector2(padding.x + (dataList[index].cellSize.x + spacing) * j, -contentHeight));

                        // 조건에 따른 Sibling 순서 조정
                        if (visibleRange.y >= viewportRange.x)
                            cellList[index].transform.SetAsLastSibling();
                        else
                            cellList[index].transform.SetAsFirstSibling();
                    }
                }
                // 각 행마다 셀의 높이와 spacing을 더해 다음 행의 시작 위치를 결정
                contentHeight += dataList[i].cellSize.y + spacing;
            }

            // 만약 전체 콘텐츠 높이가 뷰포트 높이보다 크다면
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
            for (int i = 0; i < dataList.Count; i += columeCount)
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

            // rowNumber = index / columeCount로, 해당 셀이 몇 번째 행에 위치하는지 결정
            var rowNumber = index / columeCount;

            // padding.x에서 시작하여, 목표 행 이전의 모든 행의 높이(각 행의 첫 셀의 cellSize.y + spacing)를 누적
            var height = padding.x;
            for (int i = 0; i < rowNumber; i++)
            {
                height += dataList[i * columeCount].cellSize.y + spacing;
            }

            // 계산된 높이가 콘텐츠의 최대 이동 가능 범위를 넘지 않도록 Mathf.Min을 사용해 클램핑
            height = Mathf.Min(scrollRect.content.rect.height - scrollRect.viewport.rect.height, height);

            // 현재 anchoredPosition.y와 계산된 높이가 다르면, DoSnapping()을 호출하여 부드러운 스냅 애니메이션으로 콘텐츠를 이동
            if (scrollRect.content.anchoredPosition.y != height)
            {
                DoSnapping(new Vector2(0, height), duration);
            }
        }
    }
}