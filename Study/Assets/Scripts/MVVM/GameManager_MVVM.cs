using UnityEngine;

public class GameManager_MVVM : MonoBehaviour
{
    public PlayerView_MVVM playerView;

    void Start()
    {
        var playerModel = new PlayerModel_MVVM { Name = "Player1", Score = 0 };
        var playerViewModel = new PlayerViewModel(playerModel);

        playerView.Bind(playerViewModel);
    }
}
