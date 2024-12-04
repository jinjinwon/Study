using System.Collections.Generic;
using UnityEngine;

public class EventManager_AI : MonoBehaviour
{
    private static EventManager_AI _instance;
    public static EventManager_AI Instance => _instance ??= new GameObject("EventManager").AddComponent<EventManager_AI>();

    private Dictionary<string, List<AIController>> _observersByEvent = new Dictionary<string, List<AIController>>();

    public void RegisterObserver(AIController observer, string eventType)
    {
        if (!_observersByEvent.ContainsKey(eventType))
        {
            _observersByEvent[eventType] = new List<AIController>();
        }

        if (!_observersByEvent[eventType].Contains(observer))
        {
            _observersByEvent[eventType].Add(observer);
        }
    }

    public void NotifyObservers(EventMessage eventMessage, System.Func<AIController, bool> condition = null)
    {
        if (_observersByEvent.ContainsKey(eventMessage.messageName))
        {
            foreach (var observer in _observersByEvent[eventMessage.messageName])
            {
                if (condition == null || condition(observer))
                {
                    observer.Notify(eventMessage);
                    eventMessage.relatedCommand?.StartExecution(observer.transform);
                }
            }
        }
    }
}
