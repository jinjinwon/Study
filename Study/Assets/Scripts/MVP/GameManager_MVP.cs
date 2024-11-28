using UnityEngine;

public class GameManager_MVP : MonoBehaviour
{
    public PlayerView_MVP playerView;

    private void Start()
    {
        var playerModel = new PlayerModel_MVP("Player1");
        var playerPresenter = new PlayerPresenter(playerModel, playerView);
    }
}
