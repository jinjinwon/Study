using UnityEngine;
using UnityEngine.UI;

public class ConcreteObserver_Sound : MonoBehaviour, IObserver
{
    public Text text;
    void IObserver.Update(int state)
    {
        Debug.Log($"[SoundManager] Play sound for state: {state}");
        text.text = $"Sound Update {state}";
    }
}
