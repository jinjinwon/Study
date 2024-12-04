using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "AttackCommand", menuName = "AI/Commands/Attack")]
public class AttackCommand : Command
{
    private Coroutine attackRoutine;
    private Transform transform;

    public override void StartExecution(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        if (target == null)
        {
            Debug.LogWarning("AttackCommand: Target is null! 명령이 실행되지 않습니다.");
            return;
        }

        if (attackRoutine == null)
        {
            transform = aiTransform;
            attackRoutine = aiTransform.GetComponent<MonoBehaviour>().StartCoroutine(AttackRoutine(aiTransform, target));
        }
    }

    private IEnumerator AttackRoutine(Transform aiTransform, Transform target)
    {
        while (true)
        {
            Animator animator = aiTransform.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            Debug.Log($"{aiTransform.name}이(가) {target.name}을(를) 공격합니다!");
            aiTransform.LookAt(target);

            yield return new WaitForSeconds(1f); // 공격 간격
        }
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return target != null && Vector3.Distance(aiTransform.position, target.position) <= 10f;
    }

    public override void Cancel()
    {
        if (attackRoutine != null)
        {
            transform.GetComponent<MonoBehaviour>().StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }
}
