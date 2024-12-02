using UnityEngine;

public class GameManager_Observer : MonoBehaviour
{
    private ConcreteSubject concreteSubject;

    public ConcreteObserver_UI uiManager;
    public ConcreteObserver_Sound soundManager;

    private int state = 0;

    private void Start()
    {
        // ConcreteSubject ����
        concreteSubject = new ConcreteSubject();

        // ������ ���
        concreteSubject.Attach(uiManager);
        concreteSubject.Attach(soundManager);

        //// ���� ���� �׽�Ʈ
        //Debug.Log("Changing GameState to 1...");
        //concreteSubject.GameState = 1;

        //Debug.Log("Changing GameState to 2...");
        //concreteSubject.GameState = 2;

        //// ������ ���� �׽�Ʈ
        //Debug.Log("Detaching UIManager...");
        //concreteSubject.Detach(uiManager);

        //Debug.Log("Changing GameState to 3...");
        //concreteSubject.GameState = 3;
    }

    public void OnClickAddState()
    {
        state++;
        Debug.Log($"Changing GameState to {state}...");
        concreteSubject.GameState = state;
    }

    public void OnDetachUI()
    {
        Debug.Log("Detaching UIManager...");
        concreteSubject.Detach(uiManager);
    }
}
