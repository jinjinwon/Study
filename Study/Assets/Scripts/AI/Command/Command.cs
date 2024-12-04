using UnityEngine;

public abstract class Command : ScriptableObject
{
    public string commandName;
    public int priority;

    // ? -> ����ü�� �⺻������ NULL �� �⺻ ��ȯ�������� ����� �Ұ����ѵ� �̸� �����ϵ��� ������ִ� ��ȣ
    public abstract void StartExecution(Transform aiTransform, Transform target = null, Vector3? position = null);
    public abstract bool CanExecute(Transform aiTransform, Transform target = null);
    public abstract void Cancel();
}
