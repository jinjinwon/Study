using UnityEngine;

public abstract class Command : ScriptableObject
{
    public string commandName;
    public int priority;

    // ? -> 구조체의 기본적으로 NULL 을 기본 반환형식으로 사용이 불가능한데 이를 가능하도록 만들어주는 기호
    public abstract void Execute(Transform aiTransform, Transform target = null, Vector3? position = null);
    public abstract bool CanExecute(Transform aiTransform, Transform target = null); // 실행 조건
}
