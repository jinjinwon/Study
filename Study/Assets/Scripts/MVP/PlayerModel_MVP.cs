using System;
using UnityEngine;

public class PlayerModel_MVP
{
    public string Name { get; private set; }
    public int Score { get; private set; }

    // 이벤트를 통해 Presenter에 상태 변경 알림
    public event Action<int> OnScoreUpdated;

    public PlayerModel_MVP(string name)
    {
        Name = name;
        Score = 0;
    }

    public void AddScore(int points)
    {
        Debug.Log("Presenter -> Model Value Changed!!");
        Score += points;

        // 상태 변경 이벤트 호출
        Debug.Log("Model -> Presenter");
        OnScoreUpdated?.Invoke(Score);
    }
}
