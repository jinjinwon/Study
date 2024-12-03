using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI", menuName = "AI/AI Default")]
public class AI : ScriptableObject
{
    public string aiName; // AI 이름
    public bool isDynamic; // 동적/정적 여부
    public List<Command> commands; // 실행 가능한 Command 목록
    public AIState initialState; // 초기 상태
}
