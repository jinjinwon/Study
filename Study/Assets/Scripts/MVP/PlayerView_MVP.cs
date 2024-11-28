using UnityEngine;
using UnityEngine.UI;

public class PlayerView_MVP : MonoBehaviour
{
    public Text playerNameText;
    public Text playerScoreText;
    public Button scoreButton;

    private PlayerPresenter presenter;

    // Presenter ����
    public void SetPresenter(PlayerPresenter presenter)
    {
        this.presenter = presenter;
    }

    // UI ������Ʈ
    public void DisplayPlayerInfo(string name, int score)
    {
        Debug.Log($"View Update !! {name} {score}");

        playerNameText.text = $"Name: {name}";
        playerScoreText.text = $"Score: {score}";
    }

    private void Start()
    {
        scoreButton.onClick.AddListener(() => { Debug.Log($"User Input Action !!"); });
        // ��ư Ŭ�� �� Presenter�� �̺�Ʈ ����
        scoreButton.onClick.AddListener(() => presenter.OnScoreButtonClicked());
    }
}