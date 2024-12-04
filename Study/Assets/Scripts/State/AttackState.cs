using UnityEngine;

public class AttackState : IState
{
    public void Enter()
    {
        Debug.Log("Entering Attack State");
    }

    public void Execute()
    {
        Debug.Log("Executing Attack Behavior: Attacking the target");
    }

    public void Exit()
    {
        Debug.Log("Exiting Attack State");
    }
}
