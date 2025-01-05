using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator CountingSort()
    {
        yield return StartCoroutine(CountingSortArray(sortingVisualizer.heights));
    }

    private IEnumerator CountingSortArray(List<int> list)
    {
        int max = list[0];
        int min = list[0];

        foreach (var num in list)
        {
            if (num > max) max = num;
            if (num < min) min = num;
        }

        int range = max - min + 1;
        int[] count = new int[range];
        int[] output = new int[list.Count];

        foreach (var num in list)
        {
            count[num - min]++;
        }

        for (int i = count.Length - 2; i >= 0; i--) // 내림차순으로 카운트 배열을 계산
        {
            count[i] += count[i + 1];
        }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            output[count[list[i] - min] - 1] = list[i];
            count[list[i] - min]--;
        }

        for (int i = 0; i < list.Count; i++)
        {
            list[i] = output[i];
        }

        sortingVisualizer.UpdateCubePositions();
        yield return new WaitForSeconds(0.1f); // 정렬 진행 상태를 볼 수 있도록 잠시 대기
    }
}
