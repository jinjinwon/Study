using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public float detectionRange = 10f; // ���� �Ÿ�
    public EventMessage playerNearEvent; // �÷��̾� ���� �̺�Ʈ �޽���

    private AIController _aiController;
    private bool playerInRange = false;

    void Start()
    {
        _aiController = GetComponent<AIController>();
        if (_aiController == null)
        {
            Debug.LogError("PlayerDetector: AIController�� �ʿ��մϴ�!");
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
            _aiController.ChangeState(AIState.Chase); // ���¸� Chase�� ����
        }
        else if (distance > detectionRange && playerInRange)
        {
            playerInRange = false;
            Debug.Log($"{player.name}��(��) ������ ������ϴ�.");
            _aiController.SetTarget(null);
            _aiController.ChangeState(AIState.Idle); // ���¸� Idle�� ����
        }
    }
}
