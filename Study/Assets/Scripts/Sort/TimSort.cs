using System;
using System.Diagnostics;
using UnityEngine;

public class TimSort : MonoBehaviour
{
    public int[] array = { 5, 3, 8, 4, 2 };

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        UnityEngine.Debug.Log("정렬 전: " + string.Join(", ", array));

        stopwatch.Start();
        TimSortArray();
        stopwatch.Stop();

        UnityEngine.Debug.Log("정렬 후: " + string.Join(", ", array));
        UnityEngine.Debug.Log("실행 시간 (Tim Sort): " + stopwatch.ElapsedMilliseconds + "ms");
    }

    void TimSortArray()
    {
        int run = 32;
        for (int i = 0; i < array.Length; i += run)
        {
            InsertionSort(array, i, Math.Min((i + run - 1), (array.Length - 1)));
        }

        for (int size = run; size < array.Length; size = 2 * size)
        {
            for (int left = 0; left < array.Length; left += 2 * size)
            {
                int mid = Math.Min(array.Length - 1, left + size - 1);
                int right = Math.Min((left + 2 * size - 1), (array.Length - 1));
                if (mid < right)
                    Merge(array, left, mid, right);
            }
        }
    }

    void InsertionSort(int[] array, int left, int right)
    {
        for (int i = left + 1; i <= right; i++)
        {
            int key = array[i];
            int j = i - 1;
            while (j >= left && array[j] > key)
            {
                array[j + 1] = array[j];
                j--;
            }
            array[j + 1] = key;
        }
    }

    void Merge(int[] array, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;

        int[] L = new int[n1];
        int[] R = new int[n2];

        Array.Copy(array, left, L, 0, n1);
        Array.Copy(array, mid + 1, R, 0, n2);

        int i = 0, j = 0, k = left;
        while (i < n1 && j < n2)
        {
            if (L[i] <= R[j])
            {
                array[k++] = L[i++];
            }
            else
            {
                array[k++] = R[j++];
            }
        }

        while (i < n1)
        {
            array[k++] = L[i++];
        }

        while (j < n2)
        {
            array[k++] = R[j++];
        }
    }
}
