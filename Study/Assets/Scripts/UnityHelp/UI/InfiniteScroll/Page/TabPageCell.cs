using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Cinemachine.CinemachineSplineRoll;
using UnityHelp.UI.InfiniteScroll;

/// <summary>
/// TabPageCell�� InfiniteCell�� ����Ͽ� �� �������� ǥ���ϴ� UI ���� �����ϴ� Ŭ����
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
