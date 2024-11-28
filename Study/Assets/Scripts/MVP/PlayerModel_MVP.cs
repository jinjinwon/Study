using System;
using UnityEngine;

public class PlayerModel_MVP
{
    public string Name { get; private set; }
    public int Score { get; private set; }

    // �̺�Ʈ�� ���� Presenter�� ���� ���� �˸�
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

        // ���� ���� �̺�Ʈ ȣ��
        Debug.Log("Model -> Presenter");
        OnScoreUpdated?.Invoke(Score);
    }
}
