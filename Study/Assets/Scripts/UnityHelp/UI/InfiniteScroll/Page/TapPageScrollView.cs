using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityHelp.UI.InfiniteScroll;

/// <summary>
/// InfiniteScrollView를 사용하여 탭 페이지 스크롤을 구현한 클래스
/// IBeginDragHandler, IEndDragHandler를 구현하여 사용자의 드래그 동작을 감지하고, 자동 스냅 기능을 수행
/// </summary>
public class TabPageScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    // InfiniteScrollView 인스턴스로, 탭 페이지들이 배치되는 스크롤 뷰
    public InfiniteScrollView scrollView;

    //  탭 페이지 내용(string) 배열
    public string[] pageContents;

    // 각 탭을 나타내는 Toggle UI 요소 배열
    public Toggle[] toggles;

    // 모든 토글을 그룹화하는 ToggleGroup
    public ToggleGroup toggleGroup;

    // 각 탭 페이지의 크기
    public Vector2 eachContentSize = new Vector2(600, 400);

    // 스냅이 발동되는 속도 임계값
    public float snapThreshold = 100;

    // 사용자가 드래그를 끝냈는지 여부를 나타내는 플래그
    private bool isEndDragging;

    private void Start()
    {
        // pageContents 배열의 모든 요소를 읽어 InfiniteScrollView에 추가
        foreach (var data in pageContents)
        {
            scrollView.Add(new InfiniteCellData(eachContentSize, new TabPageData { content = data }));
        }
        // scrollView.Refresh()를 호출하여 스크롤 뷰를 초기화
        scrollView.Refresh();
    }

    /// <summary>
    /// 탭 버튼을 클릭하면 해당 페이지로 스냅 이동
    /// </summary>
    /// <param name="index"></param>
    public void OnToggleChange(int index)
    {
        if (toggles[index].isOn)
        {
            scrollView.Snap(index, 0.1f);
        }
    }

    /// <summary>
    /// 사용자가 드래그를 시작할 때 실행됨
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollView.StopSnapping();
        isEndDragging = false;
        toggleGroup.SetAllTogglesOff();
    }

    /// <summary>
    /// 사용자가 드래그를 끝내면 실행 됨
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        isEndDragging = true;
    }

    private void Update()
    {
        // 드래그가 종료되었다면
        if (isEndDragging)
        {
            // 스크롤 속도가 snapThreshold(100) 이하이면 스냅 실행
            if (Mathf.Abs(scrollView.scrollRect.velocity.x) <= snapThreshold)
            {
                isEndDragging = false;
                // scrollRect.content.anchoredPosition.x를 페이지 크기로 나누어 가장 가까운 인덱스를 찾음.
                var clampX = Mathf.Min(0, scrollView.scrollRect.content.anchoredPosition.x);
                int closingIndex = Mathf.Abs(Mathf.RoundToInt(clampX / eachContentSize.x));

                // 해당 인덱스의 toggle을 활성화
                toggles[closingIndex].isOn = true;
            }
        }
    }
}
