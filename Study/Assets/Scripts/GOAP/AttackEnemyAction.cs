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

        // �÷��ʰ� EnemyCount�� 1��ŭ �ٿ� ����ϵ��� ����
        AddEffect("EnemyCount", -1);
    }

    // ���� �ÿ��� ���� Enemy�� ã�� Ÿ�� ����
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
        attacked = false;  // ���� ���� ���� �ʱ�ȭ
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
