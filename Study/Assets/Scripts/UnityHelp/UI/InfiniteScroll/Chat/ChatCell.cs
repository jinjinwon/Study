using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelp.UI.InfiniteScroll;

/// <summary>
/// ChatCell�� InfiniteCell�� ��ӹ޾� ä�� �޽����� ǥ���ϴ� ���� �����ϴ� Ŭ����
/// InfiniteCell�� ����ϹǷ� ���� ��ũ�� �信�� ����Ǵ� �� ����
/// </summary>
public class ChatCell : InfiniteCell
{
    // �߽���(ä���� ���� ���)�� �̸��� ǥ���ϴ� UI �ؽ�Ʈ
    public Text speakerText;

    // ä�� ������ ǥ���ϴ� UI �ؽ�Ʈ
    public Text messageText;

    /// <summary>
    /// InfiniteCell���� ��ӵ� OnUpdate() �޼��带 �������̵��Ͽ� ���� UI�� �����ϴ� ����
    /// </summary>
    public override void OnUpdate()
    {
        // CellData.data�� ChatCellData Ÿ������ �ٿ�ĳ����
        ChatCellData data = (ChatCellData)CellData.data;
        speakerText.text = data.speaker;
        messageText.text = data.message;

        // ������ ���� �޽��� (isSelf == true) �� ������ ����
        // �ٸ� ����� ���� �޽��� (isSelf == false) �� ���� ����

        speakerText.alignment = data.isSelf ? TextAnchor.UpperRight : TextAnchor.UpperLeft;
        messageText.alignment = data.isSelf ? TextAnchor.UpperRight : TextAnchor.UpperLeft;

        // ���� ũ�⸦ cellSize ������ ������Ʈ�Ͽ�, ä�� �޽��� ���̿� ���� �������� �� ũ�� ����
        RectTransform.sizeDelta = CellData.cellSize;
    }
}
