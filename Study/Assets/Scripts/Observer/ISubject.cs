using UnityEngine;
using System.Collections.Generic;

public interface ISubject
{
    void Attach(IObserver observer); // ������ ���
    void Detach(IObserver observer); // ������ ����
    void Notify();                   // ���� ���� �˸�
}