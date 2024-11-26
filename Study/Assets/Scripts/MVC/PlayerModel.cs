using System;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerModel
{
    // ���� ���� �˸��� ���� �̺�Ʈ ����
    public event Action<int> OnHealthChanged;
    public event Action<int> OnScoreChanged;

    public PlayerModel(int initialHealth, int initialScore)
    {
        health = initialHealth;
        score = initialScore;
    }


    private int health;
    private int score;

    public int Health
    {
        get => health;
        private set
        {
            health = value;
            // �̺�Ʈ ȣ��
            Debug.Log($"PlayerModel -> PlayerView Changed Health {health}");
            OnHealthChanged?.Invoke(health);
        }
    }

    public int Score
    {
        get => score;
        private set
        {
            score = value;
            // �̺�Ʈ ȣ��
            Debug.Log($"PlayerModel -> PlayerView Changed Score {score}");
            OnScoreChanged?.Invoke(score);
        }
    }

    // �������׿� ���� ó�� ���
    public void TakeDamage(int damage)
    {
        Debug.Log($"PlayerController -> PlayerModel TakeDamage {damage}");
        Health = Math.Max(0, Health - damage);
    }

    // �������׿� ���� ó�� ���
    public void AddScore(int points)
    {
        Debug.Log($"PlayerController -> PlayerModel AddScore {points}");
        Score += points;
    }

    public void Refresh()
    {
        OnHealthChanged?.Invoke(health);
        OnScoreChanged?.Invoke(score);
    }
}
