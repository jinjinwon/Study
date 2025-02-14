using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Cinemachine.CinemachineSplineRoll;
using UnityHelp.UI.InfiniteScroll;

/// <summary>
/// TabPageCell은 InfiniteCell을 상속하여 탭 페이지를 표시하는 UI 셀을 구현하는 클래스
/// </summary>
public class TabPageCell : InfiniteCell
{
    public Text text;

    public override void OnUpdate()
    {
        TabPageData data = (TabPageData)CellData.data;
        RectTransform.sizeDelta = CellData.cellSize;
        text.text = data.content;
    }
}
