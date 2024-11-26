using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        // �� ����
        PlayerModel playerModel = new PlayerModel(initialHealth: 100, initialScore: 1);

        // ��Ʈ�ѷ� �ʱ�ȭ
        playerController.Initialize(playerModel);
    }
}
