using System.Collections;
using System.Data;
using UnityEngine;

public class AIController : MonoBehaviour, IAI
{
    public AI aiDefinition; // ScriptableObject 연결
    private BehaviorQueue _behaviorQueue = new BehaviorQueue();
    private AIState _currentState;
    private Transform _target;

    void Start()
    {
        if (aiDefinition == null)
        {
            Debug.LogError("AIController: AI Definition is missing!");
            return;
        }

        _currentState = aiDefinition.initialState;
        EventManager_AI.Instance.RegisterObserver(this);

        if (aiDefinition.isDynamic)
        {
            StartCoroutine(ExecuteCommands());
        }
    }

    private IEnumerator ExecuteCommands()
    {
        while (true)
        {
            _behaviorQueue.ExecuteNext(this.transform, _target);
            yield return new WaitForSeconds(1f); // 명령을 주기적으로 실행하도록 설정
        }
    }

    public void Notify(EventMessage eventMessage)
    {
        Debug.Log($"AI {aiDefinition.aiName} received event: {eventMessage.messageName}");

        foreach (var command in aiDefinition.commands)
        {
            if (command.CanExecute(transform, _target))
            {
                _behaviorQueue.AddCommand(command, command.priority);
            }
        }
    }

    public void Interact()
    {
        Debug.Log($"Interacting with AI {aiDefinition.aiName}");
        EventManager_AI.Instance.NotifyObservers(Resources.Load<EventMessage>("PlayerInteracted"));
    }

    public void ChangeState(AIState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        Debug.Log($"AI 상태 변경: {_currentState}");

        switch (_currentState)
        {
            case AIState.Idle:
                _behaviorQueue.AddCommand(new IdleCommand(), 1);
                break;
            case AIState.Chase:
                _behaviorQueue.AddCommand(new ChaseCommand(), 2);
                break;
            case AIState.Attack:
                _behaviorQueue.AddCommand(new AttackCommand(), 3);
                break;
            case AIState.Flee:
                _behaviorQueue.AddCommand(new FleeCommand(), 4);
                break;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
