using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator MergeSort()
    {
        yield return StartCoroutine(MergeSortArray(sortingVisualizer.heights));
    }

    private IEnumerator MergeSortArray(List<int> list)
    {
        if (list.Count <= 1) yield break;

        int mid = list.Count / 2;
        List<int> left = list.GetRange(0, mid);
        List<int> right = list.GetRange(mid, list.Count - mid);

        yield return StartCoroutine(MergeSortArray(left));
        yield return StartCoroutine(MergeSortArray(right));

        yield return StartCoroutine(Merge(left, right, list));
    }

    private IEnumerator Merge(List<int> left, List<int> right, List<int> result)
    {
        int i = 0, j = 0, k = 0;

        while (i < left.Count && j < right.Count)
        {
            if (left[i] > right[j]) // 내림차순 정렬
            {
                result[k++] = left[i++];
            }
            else
            {
                result[k++] = right[j++];
            }
        }

        while (i < left.Count)
        {
            result[k++] = left[i++];
        }

        while (j < right.Count)
        {
            result[k++] = right[j++];
        }

        sortingVisualizer.UpdateCubePositions();
        yield return new WaitForSeconds(0.1f); // 정렬 진행 상태를 볼 수 있도록 잠시 대기
    }
}
