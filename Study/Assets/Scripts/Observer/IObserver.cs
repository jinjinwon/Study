using UnityEngine;

public interface IObserver
{
    void Update(int state); // 상태 변경 시 수행할 작업
}
