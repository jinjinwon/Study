using UnityEngine;

public class GameManager_State : MonoBehaviour
{
    private StateMachine stateMachine; // 상태 머신
    private IdleState idleState;       // Idle 상태
    private AttackState attackState;   // Attack 상태

    void Start()
    {
        // 상태 머신 및 상태 객체 초기화
        stateMachine = new StateMachine();
        idleState = new IdleState();
        attackState = new AttackState();

        // 초기 상태를 Idle로 설정
        stateMachine.SetState(idleState);
    }

    void Update()
    {
        // 현재 상태 업데이트
        stateMachine.Update();

        // 상태 전환 조건 - 키 입력에 따라 상태 변경
        if (Input.GetKeyDown(KeyCode.A))
        {
            stateMachine.SetState(attackState); // 공격 상태로 전환
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            stateMachine.SetState(idleState); // Idle 상태로 전환
        }
    }
}
