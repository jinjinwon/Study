using UnityEngine;
using System.Collections.Generic;

public class ConcreteSubject : MonoBehaviour, ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    private int gameState;

    public int GameState
    {
        get => gameState;
        set
        {
            gameState = value;
            Notify(); // ���� ���� �� ��� ���������� �˸�
        }
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Update(gameState);
        }
    }
}
