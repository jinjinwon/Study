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
            while (j >= 0 && sortingVisualizer.heights[j] < key) // �������� ����
            {
                sortingVisualizer.heights[j + 1] = sortingVisualizer.heights[j];
                j--;
            }
            sortingVisualizer.heights[j + 1] = key;

            // ť�� ��ġ ������Ʈ
            sortingVisualizer.UpdateCubePositions();
            yield return new WaitForSeconds(0.1f); // ���� ���� ���¸� �� �� �ֵ��� ��� ���
        }
    }
}
