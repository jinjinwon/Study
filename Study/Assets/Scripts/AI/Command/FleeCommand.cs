using UnityEngine;

[CreateAssetMenu(fileName = "FleeCommand", menuName = "AI/Commands/Flee")]
public class FleeCommand : Command
{
    public override void StartExecution(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        // ���� ������ ������ �� �ֵ��� �������� ���� �ʿ�
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return true;
    }

    public override void Cancel()
    {
        // ���� �ൿ �ߴ� �� �ʿ��� ó��
    }
}
