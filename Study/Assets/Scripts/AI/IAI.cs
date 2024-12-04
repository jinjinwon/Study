using UnityEngine;

public interface IAI
{
    void Notify(EventMessage eventMessage);
    void ChangeState(AIState newState);
}
