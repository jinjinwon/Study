// MoveToFoodAction.cs
using UnityEngine;

public class MoveToFoodAction : GOAPAction
{
    private bool reached = false;
    private GameObject targetFood;

    public Transform Target => targetFood?.transform;

    public MoveToFoodAction()
    {
        cost = 1f;
        actionRange = 0.1f;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // **여기서** 이전 실행의 reached 상태 초기화
        reached = false;

        var foods = GameObject.FindGameObjectsWithTag("Food");
        if (foods.Length == 0) return false;
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        foreach (var f in foods)
        {
            float d = (f.transform.position - agent.transform.position).sqrMagnitude;
            if (d < minDist) { minDist = d; closest = f; }
        }
        targetFood = closest;
        return true;
    }

    public override bool RequiresInRange() => true;

    public override bool Perform(GameObject agent)
    {
        if (!reached && Target != null &&
            Vector3.Distance(agent.transform.position, Target.position) <= actionRange)
        {
            Object.Destroy(Target.gameObject);
            reached = true;
        }
        return reached;
    }

    public override bool IsDone() => reached;
}
