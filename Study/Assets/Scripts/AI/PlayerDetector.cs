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
        }
        else if (distance > detectionRange && playerInRange)
        {
            playerInRange = false;
            EventManager_AI.Instance.NotifyObservers(new EventMessage("PlayerOutOfRange", null));
        }
    }
}
