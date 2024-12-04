using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState currentState;

    // ���� ���� �޼���
    public void SetState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // ���� ���¿��� ������
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter(); // �� ���·� ����
        }
    }

    // ���� ���� ���� �� ������Ʈ
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute(); // ���� ���� ����
        }
    }
}
