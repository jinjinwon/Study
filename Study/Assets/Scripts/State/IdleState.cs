using UnityEngine;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("Entering Idle State");
    }

    public void Execute()
    {
        Debug.Log("Executing Idle Behavior: Standing still");
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
