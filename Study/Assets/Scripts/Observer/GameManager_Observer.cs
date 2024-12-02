using UnityEngine;

public class GameManager_Observer : MonoBehaviour
{
    private ConcreteSubject concreteSubject;

    public ConcreteObserver_UI uiManager;
    public ConcreteObserver_Sound soundManager;

    private int state = 0;

    private void Start()
    {
        // ConcreteSubject 생성
        concreteSubject = new ConcreteSubject();

        // 옵저버 등록
        concreteSubject.Attach(uiManager);
        concreteSubject.Attach(soundManager);

        //// 상태 변경 테스트
        //Debug.Log("Changing GameState to 1...");
        //concreteSubject.GameState = 1;

        //Debug.Log("Changing GameState to 2...");
        //concreteSubject.GameState = 2;

        //// 옵저버 제거 테스트
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
