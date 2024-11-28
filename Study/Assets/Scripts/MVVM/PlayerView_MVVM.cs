using UnityEngine;
using UnityEngine.UI;

public class PlayerView_MVVM : MonoBehaviour
{
    public Text playerNameText;
    public Text scoreText;
    private PlayerViewModel _viewModel;

    public void Bind(PlayerViewModel viewModel)
    {
        _viewModel = viewModel;

        _viewModel.OnNameChanged += UpdatePlayerName;
        _viewModel.OnScoreChanged += UpdateScore;

        UpdatePlayerName(_viewModel.PlayerName);
        UpdateScore(_viewModel.Score);
    }

    private void UpdatePlayerName(string name)
    {
        Debug.Log("View Update!!");
        playerNameText.text = name;
    }

    private void UpdateScore(int score)
    {
        Debug.Log("View Update!!");
        scoreText.text = $"Score: {score}";
    }

    public void OnAddScoreButtonClicked()
    {
        Debug.Log("User Input Action !!!");
        _viewModel.AddScore(10);
    }

    public void OnChangedNameButtonClicked()
    {
        Debug.Log("User Input Action !!!");
        _viewModel.ChangedName($"Player {_viewModel.Score}");
    }

    private void OnDestroy()
    {
        if (_viewModel != null)
        {
            _viewModel.OnNameChanged -= UpdatePlayerName;
            _viewModel.OnScoreChanged -= UpdateScore;
        }
    }
}
