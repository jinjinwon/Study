using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityHelp.UI.InfiniteScroll;

/// <summary>
/// InfiniteScrollView�� ����Ͽ� �� ������ ��ũ���� ������ Ŭ����
/// IBeginDragHandler, IEndDragHandler�� �����Ͽ� ������� �巡�� ������ �����ϰ�, �ڵ� ���� ����� ����
/// </summary>
public class TabPageScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    // InfiniteScrollView �ν��Ͻ���, �� ���������� ��ġ�Ǵ� ��ũ�� ��
    public InfiniteScrollView scrollView;

    //  �� ������ ����(string) �迭
    public string[] pageContents;

    // �� ���� ��Ÿ���� Toggle UI ��� �迭
    public Toggle[] toggles;

    // ��� ����� �׷�ȭ�ϴ� ToggleGroup
    public ToggleGroup toggleGroup;

    // �� �� �������� ũ��
    public Vector2 eachContentSize = new Vector2(600, 400);

    // ������ �ߵ��Ǵ� �ӵ� �Ӱ谪
    public float snapThreshold = 100;

    // ����ڰ� �巡�׸� ���´��� ���θ� ��Ÿ���� �÷���
    private bool isEndDragging;

    private void Start()
    {
        // pageContents �迭�� ��� ��Ҹ� �о� InfiniteScrollView�� �߰�
        foreach (var data in pageContents)
        {
            scrollView.Add(new InfiniteCellData(eachContentSize, new TabPageData { content = data }));
        }
        // scrollView.Refresh()�� ȣ���Ͽ� ��ũ�� �並 �ʱ�ȭ
        scrollView.Refresh();
    }

    /// <summary>
    /// �� ��ư�� Ŭ���ϸ� �ش� �������� ���� �̵�
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
    /// ����ڰ� �巡�׸� ������ �� �����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollView.StopSnapping();
        isEndDragging = false;
        toggleGroup.SetAllTogglesOff();
    }

    /// <summary>
    /// ����ڰ� �巡�׸� ������ ���� ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        isEndDragging = true;
    }

    private void Update()
    {
        // �巡�װ� ����Ǿ��ٸ�
        if (isEndDragging)
        {
            // ��ũ�� �ӵ��� snapThreshold(100) �����̸� ���� ����
            if (Mathf.Abs(scrollView.scrollRect.velocity.x) <= snapThreshold)
            {
                isEndDragging = false;
                // scrollRect.content.anchoredPosition.x�� ������ ũ��� ������ ���� ����� �ε����� ã��.
                var clampX = Mathf.Min(0, scrollView.scrollRect.content.anchoredPosition.x);
                int closingIndex = Mathf.Abs(Mathf.RoundToInt(clampX / eachContentSize.x));

                // �ش� �ε����� toggle�� Ȱ��ȭ
                toggles[closingIndex].isOn = true;
            }
        }
    }
}
