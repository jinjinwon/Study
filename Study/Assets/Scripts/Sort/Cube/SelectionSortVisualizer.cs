using System.Collections;
using UnityEngine;

public class SelectionSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator SelectionSort()
    {
        int n = sortingVisualizer.heights.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int maxIdx = i;
            for (int j = i + 1; j < n; j++)
            {
                if (sortingVisualizer.heights[j] > sortingVisualizer.heights[maxIdx]) // 내림차순 정렬
                {
                    maxIdx = j;
                }
            }

            // Swap heights
            int temp = sortingVisualizer.heights[i];
            sortingVisualizer.heights[i] = sortingVisualizer.heights[maxIdx];
            sortingVisualizer.heights[maxIdx] = temp;

            // 큐브 위치 업데이트
            sortingVisualizer.UpdateCubePositions();
            yield return new WaitForSeconds(0.1f); // 정렬 진행 상태를 볼 수 있도록 잠시 대기
        }
    }
}
