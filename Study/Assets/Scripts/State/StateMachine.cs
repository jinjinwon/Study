using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState currentState;

    // 상태 변경 메서드
    public void SetState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // 현재 상태에서 나가기
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter(); // 새 상태로 들어가기
        }
    }

    // 현재 상태 유지 및 업데이트
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute(); // 현재 상태 실행
        }
    }
}
