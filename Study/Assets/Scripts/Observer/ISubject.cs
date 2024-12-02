using UnityEngine;
using System.Collections.Generic;

public interface ISubject
{
    void Attach(IObserver observer); // 옵저버 등록
    void Detach(IObserver observer); // 옵저버 제거
    void Notify();                   // 상태 변경 알림
}
