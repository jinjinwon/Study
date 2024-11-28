using UnityEngine;

public class PlayerPresenter
{
    private PlayerModel_MVP model;
    private PlayerView_MVP view;

    public PlayerPresenter(PlayerModel_MVP model, PlayerView_MVP view)
    {
        this.model = model;
        this.view = view;

        // View�� Presenter ����
        view.SetPresenter(this);

        // Model�� �̺�Ʈ ����
        model.OnScoreUpdated += OnScoreUpdated;

        // �ʱ� View ������Ʈ
        UpdateView();
    }

    public void OnScoreButtonClicked()
    {
        Debug.Log($"View -> Presenter");
        // Model ������ ������Ʈ
        model.AddScore(10);
    }

    private void OnScoreUpdated(int newScore)
    {
        Debug.Log("Model Value(Presenter) -> View Update!!");
        // Model ���� ���� �� View ������Ʈ
        view.DisplayPlayerInfo(model.Name, newScore);
    }

    private void UpdateView()
    {
        // �ʱ� View ���� ����
        view.DisplayPlayerInfo(model.Name, model.Score);
    }
}
