// EatFoodAction.cs
using UnityEngine;

public class EatFoodAction : GOAPAction
{
    private bool eaten = false;

    public EatFoodAction()
    {
        cost = 2f;
        AddPrecondition("HasFood", true);
        AddEffect("FoodEaten", true);
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool Perform(GameObject agent)
    {
        Debug.Log("Eating food...");
        eaten = true;
        return true;
    }

    public override bool IsDone()
    {
        return eaten;
    }

    public override bool RequiresInRange()
    {
        return false;
    }
}
