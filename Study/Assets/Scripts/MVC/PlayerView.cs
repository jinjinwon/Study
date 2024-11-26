using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    // UI ����
    [SerializeField] private Text healthText;
    [SerializeField] private Text scoreText;

    // ���� ������ ���� �ð����� ó�� ���
    public void UpdateHealth(int health)
    {
        healthText.text = $"Health: {health}";
        Debug.Log($"PlayerView UpdateHealth ViewUpdate!! {health}");
    }

    // ���� ������ ���� �ð����� ó�� ���
    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
        Debug.Log($"PlayerView UpdateScore View Update!! {score}");
    }

    // �̺�Ʈ ����
    public void BindModel(PlayerModel model)
    {
        model.OnHealthChanged += UpdateHealth;
        model.OnScoreChanged += UpdateScore;
    }

    // �̺�Ʈ ���� ����
    public void UnbindModel(PlayerModel model)
    {
        model.OnHealthChanged -= UpdateHealth;
        model.OnScoreChanged -= UpdateScore;
    }
}
