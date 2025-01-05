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
                if (sortingVisualizer.heights[j] > sortingVisualizer.heights[maxIdx]) // �������� ����
                {
                    maxIdx = j;
                }
            }

            // Swap heights
            int temp = sortingVisualizer.heights[i];
            sortingVisualizer.heights[i] = sortingVisualizer.heights[maxIdx];
            sortingVisualizer.heights[maxIdx] = temp;

            // ť�� ��ġ ������Ʈ
            sortingVisualizer.UpdateCubePositions();
            yield return new WaitForSeconds(0.1f); // ���� ���� ���¸� �� �� �ֵ��� ��� ���
        }
    }
}
