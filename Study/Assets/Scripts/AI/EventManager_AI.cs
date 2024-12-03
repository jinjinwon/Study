using System.Collections.Generic;
using UnityEngine;

public class EventManager_AI : MonoBehaviour
{
    private static EventManager_AI _instance;
    public static EventManager_AI Instance => _instance ??= new GameObject("EventManager").AddComponent<EventManager_AI>();

    private List<AIController> _observers = new List<AIController>();

    public void RegisterObserver(AIController observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void NotifyObservers(EventMessage eventMessage, System.Func<AIController, bool> condition = null)
    {
        foreach (var observer in _observers)
        {
            if (condition == null || condition(observer))
            {
                observer.Notify(eventMessage);
                eventMessage.relatedCommand?.Execute(observer.transform);
            }
        }
    }
}
