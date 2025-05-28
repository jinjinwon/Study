using UnityEngine;

public class AttackEnemyAction : GOAPAction
{
    private bool attacked = false;
    private GameObject targetEnemy;

    public Transform Target => targetEnemy?.transform;

    public AttackEnemyAction()
    {
        cost = 2f;
        actionRange = 1.2f;

        AddPrecondition("HealthLow", false);

        // 플래너가 EnemyCount를 1만큼 줄여 계산하도록 설정
        AddEffect("EnemyCount", -1);
    }

    // 실행 시에만 실제 Enemy를 찾아 타겟 지정
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return false;

        float minD = Mathf.Infinity;
        foreach (var e in enemies)
        {
            float d = (e.transform.position - agent.transform.position).sqrMagnitude;
            if (d < minD)
            {
                minD = d;
                targetEnemy = e;
            }
        }
        attacked = false;  // 이전 실행 상태 초기화
        return true;
    }

    public override bool RequiresInRange() => true;

    public override bool Perform(GameObject agent)
    {
        if (!attacked &&
            targetEnemy != null &&
            Vector3.Distance(agent.transform.position, Target.position) <= actionRange)
        {
            Destroy(targetEnemy);
            attacked = true;
        }
        return attacked;
    }

    public override bool IsDone() => attacked;
}
