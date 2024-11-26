using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        // 모델 생성
        PlayerModel playerModel = new PlayerModel(initialHealth: 100, initialScore: 1);

        // 컨트롤러 초기화
        playerController.Initialize(playerModel);
    }
}
