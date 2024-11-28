using UnityEngine;
using UnityEngine.UI;

public class PlayerView_MVP : MonoBehaviour
{
    public Text playerNameText;
    public Text playerScoreText;
    public Button scoreButton;

    private PlayerPresenter presenter;

    // Presenter 설정
    public void SetPresenter(PlayerPresenter presenter)
    {
        this.presenter = presenter;
    }

    // UI 업데이트
    public void DisplayPlayerInfo(string name, int score)
    {
        Debug.Log($"View Update !! {name} {score}");

        playerNameText.text = $"Name: {name}";
        playerScoreText.text = $"Score: {score}";
    }

    private void Start()
    {
        scoreButton.onClick.AddListener(() => { Debug.Log($"User Input Action !!"); });
        // 버튼 클릭 시 Presenter에 이벤트 전달
        scoreButton.onClick.AddListener(() => presenter.OnScoreButtonClicked());
    }
}