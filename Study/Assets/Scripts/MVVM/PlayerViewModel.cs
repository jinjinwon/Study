using System;
using UnityEngine;

public class PlayerViewModel
{
    private PlayerModel_MVVM _model;

    public event Action<string> OnNameChanged;
    public event Action<int> OnScoreChanged;

    public PlayerViewModel(PlayerModel_MVVM model)
    {
        _model = model;
    }

    public string PlayerName
    {
        get => _model.Name;
        set
        {
            if (_model.Name != value)
            {
                Debug.Log("View Model Update!!");
                _model.Name = value;
                Debug.Log("Model Value Update!!");
                OnNameChanged?.Invoke(value);
            }
        }
    }

    public int Score
    {
        get => _model.Score;
        set
        {
            if (_model.Score != value)
            {
                Debug.Log("View Model Update!!");
                _model.Score = value;
                Debug.Log("Model Value Update!!");
                OnScoreChanged?.Invoke(value);
            }
        }
    }

    public void AddScore(int value)
    {
        Score += value;
    }

    public void ChangedName(string name)
    {
        PlayerName = name;
    }
}
