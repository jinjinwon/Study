using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadixSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator RadixSort()
    {
        yield return StartCoroutine(RadixSortArray(sortingVisualizer.heights));
    }

    private IEnumerator RadixSortArray(List<int> list)
    {
        int max = list[0];
        foreach (var num in list)
        {
            if (num > max) max = num;
        }

        for (int exp = 1; max / exp > 0; exp *= 10)
        {
            yield return StartCoroutine(CountingSortByDigit(list, exp));
        }
    }

    private IEnumerator CountingSortByDigit(List<int> list, int exp)
    {
        int[] output = new int[list.Count];
        int[] count = new int[10];

        for (int i = 0; i < list.Count; i++)
        {
            count[(list[i] / exp) % 10]++;
        }

        for (int i = 8; i >= 0; i--) // �������� ������ ���ؼ� count �迭�� �Ųٷ� ������
        {
            count[i] += count[i + 1];
        }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            output[count[(list[i] / exp) % 10] - 1] = list[i];
            count[(list[i] / exp) % 10]--;
        }

        for (int i = 0; i < list.Count; i++)
        {
            list[i] = output[i];
        }

        sortingVisualizer.UpdateCubePositions();
        yield return new WaitForSeconds(0.1f); // ���� ���� ���¸� �� �� �ֵ��� ��� ���
    }
}
