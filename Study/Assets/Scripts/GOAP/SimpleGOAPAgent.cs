using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class SimpleGOAPAgent : MonoBehaviour, IGOAPAgent
{
    public float moveSpeed = 4f;

    private Queue<GOAPAction> plan;
    private HashSet<GOAPAction> availableActions;
    private GOAPPlanner planner;
    private PlayerHealth health;

    // ���� �߰��� �κ� ��
    private Dictionary<string, object> lastGoal;

    private void Start()
    {
        planner = new GOAPPlanner();
        health = GetComponent<PlayerHealth>();
        availableActions = new HashSet<GOAPAction>(GetComponents<GOAPAction>());
        lastGoal = null;
    }

    private void Update()
    {
        // �ֽ� ���� ���¡���ǥ ���
        var world = GetWorldState();
        var goal = CreateGoalState();

        // ��ǥ�� �ٲ������ �÷� ��ȿȭ
        if (lastGoal == null || !DictionaryEquals(goal, lastGoal))
        {
            plan = null;
            lastGoal = new Dictionary<string, object>(goal);
        }

        // �̹� ��ǥ �޼� ���¸� �Ϸ� �ݹ鸸
        if (DictionaryEquals(world, goal))
        {
            ActionsFinished();
            return;
        }

        // �÷��� ������ ���� ����
        if (plan == null || plan.Count == 0)
        {
            plan = planner.Plan(gameObject, availableActions, world, goal);
            if (plan != null) PlanFound(goal, plan);
            else PlanFailed(goal);
            return;
        }

        // ���� �׼� ����
        var action = plan.Peek();

        // �̵� �ʿ� ��: Ÿ�� ���� �� �̵��ϰ� ����
        if (action.RequiresInRange() && !action.inRange)
        {
            if (!action.CheckProceduralPrecondition(gameObject))
            {
                PlanAborted(action);
                plan = null;
                return;
            }
            MoveAgent(action);
            return;
        }

        // Perform ����
        bool success = action.Perform(gameObject);
        if (success)
        {
            plan.Dequeue();
            action.inRange = false;
            // �� �׼� ���� ������ �ٷ� ���ȹ
            plan = null;
        }
        else
        {
            PlanAborted(action);
            plan = null;
        }
    }

    public bool MoveAgent(GOAPAction nextAction)
    {
        Transform t = null;
        if (nextAction is AttackEnemyAction atk) t = atk.Target;
        else if (nextAction is FleeAction flee) t = flee.Target;

        if (t == null) return false;
        transform.position = Vector3.MoveTowards(
            transform.position,
            t.position,
            moveSpeed * Time.deltaTime
        );
        nextAction.inRange = Vector3.Distance(transform.position, t.position) <= nextAction.actionRange;
        return true;
    }

    public Dictionary<string, object> GetWorldState()
    {
        int enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        bool healthLow = health.CurrentHealth < 30;
        return new Dictionary<string, object> {
            { "EnemyCount", enemies },
            { "HealthLow",   healthLow }
        };
    }

    public Dictionary<string, object> CreateGoalState()
    {
        var goal = new Dictionary<string, object>();
        if (health.CurrentHealth < 30)
            goal["AtSafeZone"] = true;
        else
            goal["EnemyCount"] = 0;
        return goal;
    }

    public void PlanFailed(Dictionary<string, object> failedGoal)
    {
        Debug.Log("GOAP: Plan failed");
    }

    public void PlanFound(Dictionary<string, object> goal, Queue<GOAPAction> actions)
    {
        Debug.Log("GOAP: Plan found, " + actions.Count + " actions");
    }

    public void ActionsFinished()
    {
        Debug.Log("GOAP: All actions completed");
    }

    public void PlanAborted(GOAPAction aborter)
    {
        Debug.Log("GOAP: Plan aborted because of " + aborter);
    }

    // ���� �������� ��ǥ���¿� ���� �����ӿ� ��ǥ ���¸� ���ϴ� �Լ� (��ǥ�� �ٲ������ �Ǵ�)
    private bool DictionaryEquals(Dictionary<string, object> a,Dictionary<string, object> b)
    {
        if (a.Count != b.Count) return false;
        foreach (var kv in a)
        {
            if (!b.TryGetValue(kv.Key, out var bv)) return false;
            if (!kv.Value.Equals(bv)) return false;
        }
        return true;
    }
}
