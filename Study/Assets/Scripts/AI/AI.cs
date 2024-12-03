using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI", menuName = "AI/AI Default")]
public class AI : ScriptableObject
{
    public string aiName; // AI �̸�
    public bool isDynamic; // ����/���� ����
    public List<Command> commands; // ���� ������ Command ���
    public AIState initialState; // �ʱ� ����
}
