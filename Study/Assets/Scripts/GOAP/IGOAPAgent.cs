// IGOAPAgent.cs
using System.Collections.Generic;
using UnityEngine;

public interface IGOAPAgent
{
    Dictionary<string, object> GetWorldState();
    Dictionary<string, object> CreateGoalState();
    void PlanFailed(Dictionary<string, object> failedGoal);
    void PlanFound(Dictionary<string, object> goal, Queue<GOAPAction> actions);
    void ActionsFinished();
    void PlanAborted(GOAPAction aborter);
    bool MoveAgent(GOAPAction nextAction);
}
