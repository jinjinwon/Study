using System.Collections;
using UnityEngine;

public class BubbleSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator BubbleSort()
    {
        int n = sortingVisualizer.heights.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (sortingVisualizer.heights[j] < sortingVisualizer.heights[j + 1]) // �������� ����
                {
                    // Swap heights
                    int temp = sortingVisualizer.heights[j];
                    sortingVisualizer.heights[j] = sortingVisualizer.heights[j + 1];
                    sortingVisualizer.heights[j + 1] = temp;

                    // ť�� ��ġ ������Ʈ
                    sortingVisualizer.UpdateCubePositions();
                    yield return new WaitForSeconds(0.1f); // ���� ���� ���¸� �� �� �ֵ��� ��� ���
                }
            }
        }
    }
}
