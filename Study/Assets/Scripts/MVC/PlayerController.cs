using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private PlayerView view;

    public void Initialize(PlayerModel model)
    {
        playerModel = model;
        view.BindModel(model);

        playerModel.Refresh();
    }


    private void OnEnable()
    {
        if(playerModel != null)
        // 모델과 뷰를 연결
            view.BindModel(playerModel);
    }

    private void OnDisable()
    {
        // 이벤트 해제
        view.UnbindModel(playerModel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"PlayerController UserInput !!");
            playerModel.AddScore(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log($"PlayerController UserInput !!");
            playerModel.TakeDamage(5);
        }
    }
}
