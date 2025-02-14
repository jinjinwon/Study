using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelp.UI.InfiniteScroll;

/// <summary>
/// ChatCellData�� ä�� �޽��� �����͸� �����ϴ� �� Ŭ����
/// </summary>
public class ChatCellData
{
    // ä���� ���� ����� �̸�
    public string speaker;

    // ä�� ����
    public string message;

    // �޽����� ������ ���� ������ ����
    public bool isSelf;

    /// <summary>
    /// �⺻ ������
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
