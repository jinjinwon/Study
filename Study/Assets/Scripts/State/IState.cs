public interface IState
{
    void Enter();   // ���¿� �� �� ȣ��Ǵ� �޼���
    void Execute(); // ���°� �����Ǵ� ���� ȣ��Ǵ� �޼���
    void Exit();    // ���¿��� ���� �� ȣ��Ǵ� �޼���
}
