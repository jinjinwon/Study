using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    // UI 변수
    [SerializeField] private Text healthText;
    [SerializeField] private Text scoreText;

    // 변경 통지에 따른 시각적인 처리 방법
    public void UpdateHealth(int health)
    {
        healthText.text = $"Health: {health}";
        Debug.Log($"PlayerView UpdateHealth ViewUpdate!! {health}");
    }

    // 변경 통지에 따른 시각적인 처리 방법
    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
        Debug.Log($"PlayerView UpdateScore View Update!! {score}");
    }

    // 이벤트 구독
    public void BindModel(PlayerModel model)
    {
        model.OnHealthChanged += UpdateHealth;
        model.OnScoreChanged += UpdateScore;
    }

    // 이벤트 구독 해제
    public void UnbindModel(PlayerModel model)
    {
        model.OnHealthChanged -= UpdateHealth;
        model.OnScoreChanged -= UpdateScore;
    }
}
