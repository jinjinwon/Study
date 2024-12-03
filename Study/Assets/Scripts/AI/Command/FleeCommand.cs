using UnityEngine;

[CreateAssetMenu(fileName = "FleeCommand", menuName = "AI/Commands/Flee")]
public class FleeCommand : Command
{
    public override void Execute(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        if (position == null) return;
        Debug.Log($"{aiTransform.name} is fleeing to {position.Value}!");
        aiTransform.position = Vector3.Lerp(aiTransform.position, position.Value, Time.deltaTime * 5);
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return true; // 기본적으로 실행 가능
    }
}
