// SimpleGOAPAgent.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GOAPAction))]
public class SimpleGOAPAgent : MonoBehaviour, IGOAPAgent
{
    public float moveSpeed = 3f;
    private Queue<GOAPAction> plan;
    private HashSet<GOAPAction> availableActions = new HashSet<GOAPAction>();
    private GOAPPlanner planner;

    void Start()
    {
        planner = new GOAPPlanner();
        foreach (var a in GetComponents<GOAPAction>())
            availableActions.Add(a);
    }

    void Update()
    {
        // 1) 남은 Food 없으면 멈춤
        if (GameObject.FindGameObjectsWithTag("Food").Length == 0) return;

        // 2) 플랜 없거나 소진됐으면 새로 짜기
        if (plan == null || plan.Count == 0)
        {
            plan = planner.Plan(gameObject, availableActions, GetWorldState(), CreateGoalState());
            if (plan != null) PlanFound(CreateGoalState(), plan);
            else PlanFailed(CreateGoalState());
            return;
        }

        var action = plan.Peek();

        // 3) 이동이 필요하면 타겟 찾고 이동만 수행
        if (action.RequiresInRange() && !action.IsInRange())
        {
            if (!action.CheckProceduralPrecondition(gameObject))
            {
                PlanAborted(action);
                plan = null;
                return;
            }
            MoveAgent(action);
            return;  // 이동만 한 뒤 바로 리턴
        }

        // 4) 사거리 안이거나 이동 불필요 시 Perform
        bool success = action.Perform(gameObject);
        if (success)
        {
            // 완료된 액션 제거
            plan.Dequeue();
            action.inRange = false;

            // **남은 액션이 없으면** 모든 액션 완료 콜백
            if (plan.Count == 0)
            {
                ActionsFinished();
                plan = null;
            }
            // **남은 액션이 있으면** 다음 큐브를 위해 재계획
            else
            {
                plan = null;
            }
        }
        else
        {
            // 수행 실패 시 재계획
            PlanAborted(action);
            plan = null;
        }
    }


    public bool MoveAgent(GOAPAction nextAction)
    {
        if (nextAction is MoveToFoodAction move)
        {
            var tgt = move.Target;
            if (tgt == null) return false;
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, tgt.position, step);
            nextAction.inRange = Vector3.Distance(transform.position, tgt.position) <= nextAction.actionRange;
        }
        return true;
    }

    public Dictionary<string, object> GetWorldState()
    {
        int count = GameObject.FindGameObjectsWithTag("Food").Length;
        return new Dictionary<string, object> { { "FoodCount", count } };
    }

    public Dictionary<string, object> CreateGoalState()
    {
        return new Dictionary<string, object> { { "FoodCount", 0 } };
    }

    public void PlanFailed(Dictionary<string, object> failedGoal)
    {
        Debug.Log("Plan failed");
    }

    public void PlanFound(Dictionary<string, object> goal, Queue<GOAPAction> actions)
    {
        Debug.Log("Plan found: " + actions.Count + " actions");
    }

    public void ActionsFinished()
    {
        Debug.Log("All actions completed");
    }

    public void PlanAborted(GOAPAction aborter)
    {
        Debug.Log("Plan aborted: " + aborter);
    }
}
