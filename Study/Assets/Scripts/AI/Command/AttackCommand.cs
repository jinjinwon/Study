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
            Debug.LogWarning("AttackCommand: Target is null! ����� ������� �ʽ��ϴ�.");
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

            Debug.Log($"{aiTransform.name}��(��) {target.name}��(��) �����մϴ�!");
            aiTransform.LookAt(target);

            yield return new WaitForSeconds(1f); // ���� ����
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
