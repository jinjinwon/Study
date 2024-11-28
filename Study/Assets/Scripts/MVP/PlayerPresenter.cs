using UnityEngine;

public class PlayerPresenter
{
    private PlayerModel_MVP model;
    private PlayerView_MVP view;

    public PlayerPresenter(PlayerModel_MVP model, PlayerView_MVP view)
    {
        this.model = model;
        this.view = view;

        // View에 Presenter 설정
        view.SetPresenter(this);

        // Model의 이벤트 구독
        model.OnScoreUpdated += OnScoreUpdated;

        // 초기 View 업데이트
        UpdateView();
    }

    public void OnScoreButtonClicked()
    {
        Debug.Log($"View -> Presenter");
        // Model 데이터 업데이트
        model.AddScore(10);
    }

    private void OnScoreUpdated(int newScore)
    {
        Debug.Log("Model Value(Presenter) -> View Update!!");
        // Model 상태 변경 시 View 업데이트
        view.DisplayPlayerInfo(model.Name, newScore);
    }

    private void UpdateView()
    {
        // 초기 View 상태 설정
        view.DisplayPlayerInfo(model.Name, model.Score);
    }
}
