using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class AsyncImageLoader : MonoBehaviour
{
    public Image targetSpriteRenderer; 
    public Sprite defaultSprite;
    public string imageName = "22";  // ��巹������ ����ϴ� ��쿡�� ��巹���� ��θ� ����ϸ� ���� �� �մϴ�
    public int delay = 0;
    public bool bNetworkSuccess;


    //private async void Start()
    //{
    //    SetDefaultImage();

    //    await LoadImageBasedOnNetworkStatus();
    //}

    [ContextMenu("�׽�Ʈ ����")]
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
            Debug.Log("�⺻ �̹����� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("SpriteRenderer �Ǵ� �⺻ �̹����� �������� �ʾҽ��ϴ�.");
        }
    }

    private async UniTask LoadImageBasedOnNetworkStatus()
    {
        try
        {
            // ��Ʈ��ũ ���� Ȯ�� (����� �׳� ������ �׽�Ʈ)
            bool isNetworkConnected = await CheckNetworkConnectionAsync();

            if (!isNetworkConnected)
            {
                Debug.LogWarning("��Ʈ��ũ ������ ������ �ֽ��ϴ�. �⺻ �̹����� �����˴ϴ�.");
                return;
            }

            // �񵿱�� �̹��� �ε�
            Sprite loadedSprite = await LoadImageFromResourcesAsync(imageName);

            // Unity ���� �����忡�� SpriteRenderer�� �̹��� ����
            await UniTask.SwitchToMainThread();
            if (targetSpriteRenderer != null && loadedSprite != null)
            {
                targetSpriteRenderer.sprite = loadedSprite;
                Debug.Log("��Ʈ��ũ �̹����� ���������� �ε��ϰ� �����߽��ϴ�.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"�̹��� �ε� ����: {ex.Message}");
        }
    }

    private async UniTask<bool> CheckNetworkConnectionAsync()
    {
        await UniTask.Delay(delay);

        #region  ��Ʈ��ũ HTTP ��û
        // ��Ʈ��ũ ���� ���¸� Ȯ���ϱ� ���� HTTP ��û
        //try
        //{
        //    using (UnityWebRequest request = UnityWebRequest.Get(testUrl))
        //    {
        //        request.timeout = 5;
        //        await request.SendWebRequest();

        //        if (request.result == UnityWebRequest.Result.Success)
        //        {
        //            Debug.Log("��Ʈ��ũ ���� Ȯ�� ����");
        //            return true; // ���� ����
        //        }
        //        else
        //        {
        //            Debug.LogWarning($"��Ʈ��ũ ���� Ȯ�� ����: {request.error}");
        //            return false; // ���� ����
        //        }
        //    }
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError($"��Ʈ��ũ Ȯ�� �� ���� �߻�: {ex.Message}");
        //    return false;
        //}
        #endregion

        // ������ �׳� ������ üũ
        return bNetworkSuccess;
    }

    private async UniTask<Sprite> LoadImageFromResourcesAsync(string resourceName)
    {
        // ���� ������� ��ȯ -> Resource.Load ������ ����
        await UniTask.SwitchToMainThread();
        Debug.Log($"Resources���� �̹��� �ε� ��... ({resourceName})");

        // ���� �����忡�� `Resources.Load` ȣ��
        Sprite sprite = Resources.Load<Sprite>(resourceName);
        if (sprite == null)
        {
            throw new System.Exception($"Resources���� '{resourceName}'�� ã�� �� �����ϴ�.");
        }
        return sprite;
    }
}
