using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelp.UI.InfiniteScroll;

/// <summary>
/// ChatCellData는 채팅 메시지 데이터를 저장하는 모델 클래스
/// </summary>
public class ChatCellData
{
    // 채팅을 보낸 사람의 이름
    public string speaker;

    // 채팅 내용
    public string message;

    // 메시지가 본인이 보낸 것인지 여부
    public bool isSelf;

    /// <summary>
    /// 기본 생성자
    /// </summary>
    /// <param name="speaker"></param>
    /// <param name="message"></param>
    /// <param name="isSelf"></param>
    public ChatCellData(string speaker, string message, bool isSelf)
    {
        this.speaker = speaker;
        this.message = message;
        this.isSelf = isSelf;
    }
}
