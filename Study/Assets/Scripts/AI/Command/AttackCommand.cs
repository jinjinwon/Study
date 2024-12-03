using UnityEngine;

[CreateAssetMenu(fileName = "AttackCommand", menuName = "AI/Commands/Attack")]
public class AttackCommand : Command
{
    public override void Execute(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        if (target == null)
        {
            Debug.LogWarning("AttackCommand: Target is null!");
            return;
        }

        Debug.Log($"{aiTransform.name}��(��) {target.name}��(��) �����մϴ�!");
        aiTransform.LookAt(target);
        // ���� �ִϸ��̼� �߰� ����
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        if (target == null) return false;
        return Vector3.Distance(aiTransform.position, target.position) <= 10f;
    }
}
