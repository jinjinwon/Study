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
        // 1) ���� Food ������ ����
        if (GameObject.FindGameObjectsWithTag("Food").Length == 0) return;

        // 2) �÷� ���ų� ���������� ���� ¥��
        if (plan == null || plan.Count == 0)
        {
            plan = planner.Plan(gameObject, availableActions, GetWorldState(), CreateGoalState());
            if (plan != null) PlanFound(CreateGoalState(), plan);
            else PlanFailed(CreateGoalState());
            return;
        }

        var action = plan.Peek();

        // 3) �̵��� �ʿ��ϸ� Ÿ�� ã�� �̵��� ����
        if (action.RequiresInRange() && !action.IsInRange())
        {
            if (!action.CheckProceduralPrecondition(gameObject))
            {
                PlanAborted(action);
                plan = null;
                return;
            }
            MoveAgent(action);
            return;  // �̵��� �� �� �ٷ� ����
        }

        // 4) ��Ÿ� ���̰ų� �̵� ���ʿ� �� Perform
        bool success = action.Perform(gameObject);
        if (success)
        {
            // �Ϸ�� �׼� ����
            plan.Dequeue();
            action.inRange = false;

            // **���� �׼��� ������** ��� �׼� �Ϸ� �ݹ�
            if (plan.Count == 0)
            {
                ActionsFinished();
                plan = null;
            }
            // **���� �׼��� ������** ���� ť�긦 ���� ���ȹ
            else
            {
                plan = null;
            }
        }
        else
        {
            // ���� ���� �� ���ȹ
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
