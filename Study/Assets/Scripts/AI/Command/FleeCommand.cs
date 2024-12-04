using UnityEngine;

[CreateAssetMenu(fileName = "FleeCommand", menuName = "AI/Commands/Flee")]
public class FleeCommand : Command
{
    public override void StartExecution(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        // 도망 로직을 구현할 수 있도록 지속적인 실행 필요
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return true;
    }

    public override void Cancel()
    {
        // 도망 행동 중단 시 필요한 처리
    }
}
