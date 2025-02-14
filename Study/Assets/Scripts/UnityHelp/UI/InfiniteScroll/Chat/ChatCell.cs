using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelp.UI.InfiniteScroll;

/// <summary>
/// ChatCell은 InfiniteCell을 상속받아 채팅 메시지를 표시하는 셀을 구현하는 클래스
/// InfiniteCell을 상속하므로 무한 스크롤 뷰에서 재사용되는 셀 역할
/// </summary>
public class ChatCell : InfiniteCell
{
    // 발신자(채팅을 보낸 사람)의 이름을 표시하는 UI 텍스트
    public Text speakerText;

    // 채팅 내용을 표시하는 UI 텍스트
    public Text messageText;

    /// <summary>
    /// InfiniteCell에서 상속된 OnUpdate() 메서드를 오버라이드하여 셀의 UI를 갱신하는 역할
    /// </summary>
    public override void OnUpdate()
    {
        // CellData.data를 ChatCellData 타입으로 다운캐스팅
        ChatCellData data = (ChatCellData)CellData.data;
        speakerText.text = data.speaker;
        messageText.text = data.message;

        // 본인이 보낸 메시지 (isSelf == true) → 오른쪽 정렬
        // 다른 사람이 보낸 메시지 (isSelf == false) → 왼쪽 정렬

        speakerText.alignment = data.isSelf ? TextAnchor.UpperRight : TextAnchor.UpperLeft;
        messageText.alignment = data.isSelf ? TextAnchor.UpperRight : TextAnchor.UpperLeft;

        // 셀의 크기를 cellSize 값으로 업데이트하여, 채팅 메시지 길이에 따라 동적으로 셀 크기 조정
        RectTransform.sizeDelta = CellData.cellSize;
    }
}
