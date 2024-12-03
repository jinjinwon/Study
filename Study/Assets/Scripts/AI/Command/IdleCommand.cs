using UnityEngine;

[CreateAssetMenu(fileName = "IdleCommand", menuName = "AI/Commands/Idle")]
public class IdleCommand : Command
{
    public override void Execute(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        Debug.Log($"{aiTransform.name} is idling.");
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return true;
    }
}
