using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator TimSort()
    {
        int run = 32;
        for (int i = 0; i < sortingVisualizer.heights.Count; i += run)
        {
            InsertionSort(sortingVisualizer.heights, i, Mathf.Min(i + run - 1, sortingVisualizer.heights.Count - 1));
        }

        for (int size = run; size < sortingVisualizer.heights.Count; size *= 2)
        {
            for (int left = 0; left < sortingVisualizer.heights.Count; left += 2 * size)
            {
                int mid = Mathf.Min(sortingVisualizer.heights.Count - 1, left + size - 1);
                int right = Mathf.Min(sortingVisualizer.heights.Count - 1, left + 2 * size - 1);
                if (mid < right)
                    Merge(sortingVisualizer.heights, left, mid, right);
            }
        }

        sortingVisualizer.UpdateCubePositions();
        yield return new WaitForSeconds(0.1f); // 정렬 진행 상태를 볼 수 있도록 잠시 대기
    }

    void InsertionSort(List<int> list, int left, int right)
    {
        for (int i = left + 1; i <= right; i++)
        {
            int key = list[i];
            int j = i - 1;
            while (j >= left && list[j] < key)
            {
                list[j + 1] = list[j];
                j--;
            }
            list[j + 1] = key;
        }
    }

    void Merge(List<int> list, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;
        List<int> L = new List<int>();
        List<int> R = new List<int>();

        for (int q = 0; q < n1; q++)
            L.Add(list[left + q]);
        for (int w = 0; w < n2; w++)
            R.Add(list[mid + 1 + w]);

        int i = 0, j = 0, k = left;
        while (i < n1 && j < n2)
        {
            if (L[i] >= R[j])
            {
                list[k++] = L[i++];
            }
            else
            {
                list[k++] = R[j++];
            }
        }

        while (i < n1)
            list[k++] = L[i++];

        while (j < n2)
            list[k++] = R[j++];
    }
}
