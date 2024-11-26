using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class AsyncImageLoader : MonoBehaviour
{
    public Image targetSpriteRenderer; 
    public Sprite defaultSprite;
    public string imageName = "22";  // 어드레서블을 사용하는 경우에는 어드레서블 경로를 사용하면 좋을 듯 합니다
    public int delay = 0;
    public bool bNetworkSuccess;


    //private async void Start()
    //{
    //    SetDefaultImage();

    //    await LoadImageBasedOnNetworkStatus();
    //}

    [ContextMenu("테스트 시작")]
    private async void TestStart()
    {
        SetDefaultImage();

        await LoadImageBasedOnNetworkStatus();
    }

    private void SetDefaultImage()
    {
        if (targetSpriteRenderer != null && defaultSprite != null)
        {
            targetSpriteRenderer.sprite = defaultSprite;
            Debug.Log("기본 이미지가 설정되었습니다.");
        }
        else
        {
            Debug.LogError("SpriteRenderer 또는 기본 이미지가 설정되지 않았습니다.");
        }
    }

    private async UniTask LoadImageBasedOnNetworkStatus()
    {
        try
        {
            // 네트워크 상태 확인 (현재는 그냥 변수로 테스트)
            bool isNetworkConnected = await CheckNetworkConnectionAsync();

            if (!isNetworkConnected)
            {
                Debug.LogWarning("네트워크 연결이 끊어져 있습니다. 기본 이미지로 유지됩니다.");
                return;
            }

            // 비동기로 이미지 로드
            Sprite loadedSprite = await LoadImageFromResourcesAsync(imageName);

            // Unity 메인 스레드에서 SpriteRenderer에 이미지 설정
            await UniTask.SwitchToMainThread();
            if (targetSpriteRenderer != null && loadedSprite != null)
            {
                targetSpriteRenderer.sprite = loadedSprite;
                Debug.Log("네트워크 이미지를 성공적으로 로드하고 설정했습니다.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"이미지 로드 실패: {ex.Message}");
        }
    }

    private async UniTask<bool> CheckNetworkConnectionAsync()
    {
        await UniTask.Delay(delay);

        #region  네트워크 HTTP 요청
        // 네트워크 연결 상태를 확인하기 위해 HTTP 요청
        //try
        //{
        //    using (UnityWebRequest request = UnityWebRequest.Get(testUrl))
        //    {
        //        request.timeout = 5;
        //        await request.SendWebRequest();

        //        if (request.result == UnityWebRequest.Result.Success)
        //        {
        //            Debug.Log("네트워크 연결 확인 성공");
        //            return true; // 연결 성공
        //        }
        //        else
        //        {
        //            Debug.LogWarning($"네트워크 연결 확인 실패: {request.error}");
        //            return false; // 연결 실패
        //        }
        //    }
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError($"네트워크 확인 중 예외 발생: {ex.Message}");
        //    return false;
        //}
        #endregion

        // 지금은 그냥 변수로 체크
        return bNetworkSuccess;
    }

    private async UniTask<Sprite> LoadImageFromResourcesAsync(string resourceName)
    {
        // 메인 스레드로 전환 -> Resource.Load 접근을 위한
        await UniTask.SwitchToMainThread();
        Debug.Log($"Resources에서 이미지 로드 중... ({resourceName})");

        // 메인 스레드에서 `Resources.Load` 호출
        Sprite sprite = Resources.Load<Sprite>(resourceName);
        if (sprite == null)
        {
            throw new System.Exception($"Resources에서 '{resourceName}'를 찾을 수 없습니다.");
        }
        return sprite;
    }
}
