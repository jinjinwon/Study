using System.Collections;
using UnityEngine;

public class InsertionSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator InsertionSort()
    {
        int n = sortingVisualizer.heights.Count;
        for (int i = 1; i < n; i++)
        {
            int key = sortingVisualizer.heights[i];
            int j = i - 1;
            while (j >= 0 && sortingVisualizer.heights[j] < key) // 내림차순 정렬
            {
                sortingVisualizer.heights[j + 1] = sortingVisualizer.heights[j];
                j--;
            }
            sortingVisualizer.heights[j + 1] = key;

            // 큐브 위치 업데이트
            sortingVisualizer.UpdateCubePositions();
            yield return new WaitForSeconds(0.1f); // 정렬 진행 상태를 볼 수 있도록 잠시 대기
        }
    }
}
