using System.Collections;
using System.Data;
using UnityEngine;

public class AIController : MonoBehaviour, IAI
{
    public AI aiDefinition;
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
        EventManager_AI.Instance.RegisterObserver(this, aiDefinition.eventType);
        EventManager_AI.Instance.RegisterObserver(this, "PlayerOutOfRange");

        if (aiDefinition.isDynamic)
        {
            StartCoroutine(ExecuteCommands());
        }
    }

    private IEnumerator ExecuteCommands()
    {
        while (true)
        {
            _behaviorQueue.ExecuteNext(transform, _target);
            yield return null; // 매 프레임마다 명령 실행 시도
        }
    }

    public void Notify(EventMessage eventMessage)
    {
        Debug.Log($"AI {aiDefinition.aiName} received event: {eventMessage.messageName}");

        if (eventMessage.messageName == "PlayerNear")
        {
            SetTarget(GameObject.FindWithTag("Player").transform);
            ChangeState(AIState.Chase);
        }
        else if (eventMessage.messageName == "PlayerOutOfRange")
        {
            SetTarget(null);
            ChangeState(AIState.Idle);
        }

        foreach (var command in aiDefinition.commands)
        {
            if (command.CanExecute(transform, _target))
            {
                _behaviorQueue.AddCommand(command, command.priority);
            }
        }
    }

    public void ChangeState(AIState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        Debug.Log($"AI 상태 변경: {_currentState}");

        _behaviorQueue = new BehaviorQueue();

        switch (_currentState)
        {
            case AIState.Idle:
                _behaviorQueue.AddCommand(ScriptableObject.CreateInstance<IdleCommand>(), 1);
                break;
            case AIState.Chase:
                _behaviorQueue.AddCommand(ScriptableObject.CreateInstance<ChaseCommand>(), 2);
                break;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
