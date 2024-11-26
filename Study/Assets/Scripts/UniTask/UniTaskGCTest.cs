using UnityEngine;
using Cysharp.Threading.Tasks;

public class UniTaskGCTest : MonoBehaviour
{
    private const int TaskCount = 100;

    private async void Start()
    {
        Debug.Log("UniTask �׽�Ʈ ����");

        int gcBefore = System.GC.CollectionCount(0);
        long memoryBefore = System.GC.GetTotalMemory(false);

        for (int i = 0; i < TaskCount; i++)
        {
            await UniTask.Create(async () =>
            {
                int result = 0;
                for (int j = 0; j < 10; j++)
                {
                    result += j;
                }
            });
        }
        int gcAfter = System.GC.CollectionCount(0);

        long memoryAfter = System.GC.GetTotalMemory(false);
        Debug.Log($"UniTask: �޸� �Ҵ緮 = {memoryAfter - memoryBefore} bytes");
        Debug.Log($"GC Collection Ƚ��: {gcAfter - gcBefore}");
    }
}
