using UnityEngine;

[CreateAssetMenu(fileName = "IdleCommand", menuName = "AI/Commands/Idle")]
public class IdleCommand : Command
{
    public override void StartExecution(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        Debug.Log($"{aiTransform.name} is idling.");
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return true;
    }

    public override void Cancel()
    {
        // Idle 상태에서는 특별히 취소할 것이 없음
    }
}
