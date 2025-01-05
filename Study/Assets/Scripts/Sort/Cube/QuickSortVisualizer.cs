using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator QuickSort()
    {
        yield return StartCoroutine(QuickSortArray(sortingVisualizer.heights, 0, sortingVisualizer.heights.Count - 1));
    }

    private IEnumerator QuickSortArray(List<int> list, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(list, low, high);
            yield return StartCoroutine(QuickSortArray(list, low, pi - 1));
            yield return StartCoroutine(QuickSortArray(list, pi + 1, high));
        }
    }

    private int Partition(List<int> list, int low, int high)
    {
        int pivot = list[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (list[j] > pivot) // 내림차순 정렬
            {
                i++;
                int temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        int swapTemp = list[i + 1];
        list[i + 1] = list[high];
        list[high] = swapTemp;

        sortingVisualizer.UpdateCubePositions();
        return i + 1;
    }
}
