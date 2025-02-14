using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Cinemachine.CinemachineSplineRoll;
using UnityHelp.UI.InfiniteScroll;

public class GridCell : InfiniteCell
{
    public Text text;

    public override void OnUpdate()
    {
        RectTransform.sizeDelta = CellData.cellSize;
        text.text = CellData.index.ToString();
    }

    public void OnClicked()
    {
        InvokeSelected();
    }
}
