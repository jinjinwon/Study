public interface IState
{
    void Enter();   // 상태에 들어갈 때 호출되는 메서드
    void Execute(); // 상태가 유지되는 동안 호출되는 메서드
    void Exit();    // 상태에서 나갈 때 호출되는 메서드
}
