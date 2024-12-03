using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public float detectionRange = 10f; // 감지 거리
    public EventMessage playerNearEvent; // 플레이어 접근 이벤트 메시지

    private AIController _aiController;
    private bool playerInRange = false;

    void Start()
    {
        _aiController = GetComponent<AIController>();
        if (_aiController == null)
        {
            Debug.LogError("PlayerDetector: AIController가 필요합니다!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRange && !playerInRange)
        {
            playerInRange = true;
            EventManager_AI.Instance.NotifyObservers(playerNearEvent);
            _aiController.SetTarget(player);
            _aiController.ChangeState(AIState.Chase); // 상태를 Chase로 변경
        }
        else if (distance > detectionRange && playerInRange)
        {
            playerInRange = false;
            Debug.Log($"{player.name}이(가) 범위를 벗어났습니다.");
            _aiController.SetTarget(null);
            _aiController.ChangeState(AIState.Idle); // 상태를 Idle로 변경
        }
    }
}
