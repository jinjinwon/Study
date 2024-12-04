using UnityEngine;

public class GameManager_State : MonoBehaviour
{
    private StateMachine stateMachine; // ���� �ӽ�
    private IdleState idleState;       // Idle ����
    private AttackState attackState;   // Attack ����

    void Start()
    {
        // ���� �ӽ� �� ���� ��ü �ʱ�ȭ
        stateMachine = new StateMachine();
        idleState = new IdleState();
        attackState = new AttackState();

        // �ʱ� ���¸� Idle�� ����
        stateMachine.SetState(idleState);
    }

    void Update()
    {
        // ���� ���� ������Ʈ
        stateMachine.Update();

        // ���� ��ȯ ���� - Ű �Է¿� ���� ���� ����
        if (Input.GetKeyDown(KeyCode.A))
        {
            stateMachine.SetState(attackState); // ���� ���·� ��ȯ
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            stateMachine.SetState(idleState); // Idle ���·� ��ȯ
        }
    }
}
