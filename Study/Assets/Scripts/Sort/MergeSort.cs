using System;
using System.Diagnostics;
using UnityEngine;

public class MergeSort : MonoBehaviour
{
    public int[] array = { 5, 3, 8, 4, 2 };

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        UnityEngine.Debug.Log("정렬 전: " + string.Join(", ", array));

        stopwatch.Start();
        MergeSortArray(array);
        stopwatch.Stop();

        UnityEngine.Debug.Log("정렬 후: " + string.Join(", ", array));
        UnityEngine.Debug.Log("실행 시간 (Merge Sort): " + stopwatch.ElapsedMilliseconds + "ms");
    }

    void MergeSortArray(int[] array)
    {
        if (array.Length <= 1) return;

        int mid = array.Length / 2;
        int[] left = new int[mid];
        int[] right = new int[array.Length - mid];

        System.Array.Copy(array, 0, left, 0, mid);
        System.Array.Copy(array, mid, right, 0, array.Length - mid);

        MergeSortArray(left);
        MergeSortArray(right);

        Merge(left, right, array);
    }

    void Merge(int[] left, int[] right, int[] result)
    {
        int i = 0, j = 0, k = 0;
        while (i < left.Length && j < right.Length)
        {
            if (left[i] <= right[j])
                result[k++] = left[i++];
            else
                result[k++] = right[j++];
        }

        while (i < left.Length)
            result[k++] = left[i++];

        while (j < right.Length)
            result[k++] = right[j++];
    }
}
