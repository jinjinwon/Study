using UnityEngine;

public interface IAI
{
    void Notify(EventMessage eventMessage); // 이벤트 수신
    void Interact(); // 상호작용 처리
}
