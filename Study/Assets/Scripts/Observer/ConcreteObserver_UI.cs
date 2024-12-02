using UnityEngine;
using UnityEngine.UI;

public class ConcreteObserver_UI : MonoBehaviour, IObserver
{
    public Text text;

    void IObserver.Update(int state)
    {
        Debug.Log($"[UIManager] UI Updated. Current Game State: {state}");
        text.text = $"UI State {state}";
    }
}
