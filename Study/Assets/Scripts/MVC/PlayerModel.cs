using System;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerModel
{
    // 변경 사항 알림을 위한 이벤트 변수
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
            // 이벤트 호출
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
            // 이벤트 호출
            Debug.Log($"PlayerModel -> PlayerView Changed Score {score}");
            OnScoreChanged?.Invoke(score);
        }
    }

    // 변동사항에 따른 처리 방법
    public void TakeDamage(int damage)
    {
        Debug.Log($"PlayerController -> PlayerModel TakeDamage {damage}");
        Health = Math.Max(0, Health - damage);
    }

    // 변동사항에 따른 처리 방법
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
