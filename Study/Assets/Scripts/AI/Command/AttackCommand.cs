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

        Debug.Log($"{aiTransform.name}이(가) {target.name}을(를) 공격합니다!");
        aiTransform.LookAt(target);
        // 공격 애니메이션 추가 가능
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        if (target == null) return false;
        return Vector3.Distance(aiTransform.position, target.position) <= 10f;
    }
}
