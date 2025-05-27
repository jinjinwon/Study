// FindFoodAction.cs
using UnityEngine;

public class FindFoodAction : GOAPAction
{
    private bool found = false;

    public FindFoodAction()
    {
        cost = 1f;
        AddEffect("HasFood", true);
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // 예: Food 오브젝트 검색
        found = true;
        return true;
    }

    public override bool Perform(GameObject agent)
    {
        found = true;
        return true;
    }

    public override bool IsDone()
    {
        return found;
    }

    public override bool RequiresInRange()
    {
        return false;
    }
}
