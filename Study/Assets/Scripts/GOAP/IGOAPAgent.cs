using System.Collections.Generic;
using UnityEngine;

public interface IGOAPAgent
{
    // 현재 상태(세계 상태)를 반환
    public Dictionary<string, object> GetWorldState();
    // 달성하고자 하는 목표 상태를 반환
    public Dictionary<string, object> CreateGoalState();
    // 플랜 생성 실패 시 호출
    public void PlanFailed(Dictionary<string, object> failedGoal);
    // 플랜 생성 성공 시 호출
    public void PlanFound(Dictionary<string, object> goal, Queue<GOAPAction> actions);
    // 모든 액션을 다 수행했을 때 호출
    public void ActionsFinished();
    // 액션 수행 중 실패 시 호출
    public void PlanAborted(GOAPAction aborter);
    // 다음 액션을 위해 실제 이동 처리
    public bool MoveAgent(GOAPAction nextAction);
}
