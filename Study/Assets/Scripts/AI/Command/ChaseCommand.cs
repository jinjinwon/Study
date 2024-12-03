using UnityEngine;

[CreateAssetMenu(fileName = "ChaseCommand", menuName = "AI/Commands/Chase")]
public class ChaseCommand : Command
{
    public float chaseSpeed = 5f;

    public override void Execute(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        if (target == null)
        {
            Debug.LogWarning("ChaseCommand: Target is null!");
            return;
        }

        Debug.Log($"{aiTransform.name} is chasing {target.name}.");
        aiTransform.position = Vector3.MoveTowards(aiTransform.position, target.position, chaseSpeed * Time.deltaTime);
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return target != null;
    }
}
