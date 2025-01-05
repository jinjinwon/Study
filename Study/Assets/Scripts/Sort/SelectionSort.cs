using System;
using System.Diagnostics;
using UnityEngine;

public class SelectionSort : MonoBehaviour
{
    public int[] array = { 5, 3, 8, 4, 2 };

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        UnityEngine.Debug.Log("정렬 전: " + string.Join(", ", array));

        stopwatch.Start();
        SelectionSortArray();
        stopwatch.Stop();

        UnityEngine.Debug.Log("정렬 후: " + string.Join(", ", array));
        UnityEngine.Debug.Log("실행 시간 (Selection Sort): " + stopwatch.ElapsedMilliseconds + "ms");
    }

    void SelectionSortArray()
    {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (array[j] < array[minIndex])
                {
                    minIndex = j;
                }
            }
            // Swap the minimum element with the first element
            int temp = array[minIndex];
            array[minIndex] = array[i];
            array[i] = temp;
        }
    }
}
