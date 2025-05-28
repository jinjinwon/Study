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

    // 새로 추가된 부분 ↓
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
        // 최신 세계 상태·목표 계산
        var world = GetWorldState();
        var goal = CreateGoalState();

        // 목표가 바뀌었으면 플랜 무효화
        if (lastGoal == null || !DictionaryEquals(goal, lastGoal))
        {
            plan = null;
            lastGoal = new Dictionary<string, object>(goal);
        }

        // 이미 목표 달성 상태면 완료 콜백만
        if (DictionaryEquals(world, goal))
        {
            ActionsFinished();
            return;
        }

        // 플랜이 없으면 새로 생성
        if (plan == null || plan.Count == 0)
        {
            plan = planner.Plan(gameObject, availableActions, world, goal);
            if (plan != null) PlanFound(goal, plan);
            else PlanFailed(goal);
            return;
        }

        // 현재 액션 실행
        var action = plan.Peek();

        // 이동 필요 시: 타겟 갱신 후 이동하고 리턴
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

        // Perform 실행
        bool success = action.Perform(gameObject);
        if (success)
        {
            plan.Dequeue();
            action.inRange = false;
            // 한 액션 끝날 때마다 바로 재계획
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

    // 이전 프레임의 목표상태와 현재 프레임에 목표 상태를 비교하는 함수 (목표가 바뀌었는지 판단)
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
