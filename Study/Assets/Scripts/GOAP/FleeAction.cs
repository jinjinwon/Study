using UnityEngine;

public class FleeAction : GOAPAction
{
    private bool fled = false;
    private GameObject safeZone;

    public Transform Target => safeZone != null ? safeZone.transform : null;

    public FleeAction()
    {
        cost = 1f;
        actionRange = 0.5f;
        AddPrecondition("HealthLow", true);
        AddEffect("AtSafeZone", true);
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        safeZone = GameObject.FindWithTag("SafeZone");
        if (safeZone == null) return false;
        fled = false;
        return true;
    }

    public override bool RequiresInRange() => true;

    public override bool Perform(GameObject agent)
    {
        if (!fled && safeZone != null &&
            Vector3.Distance(agent.transform.position, safeZone.transform.position) <= actionRange)
        {
            fled = true;
        }
        return fled;
    }

    public override bool IsDone() => fled;
}
