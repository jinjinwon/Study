using System;
using System.Diagnostics;
using UnityEngine;

public class QuickSort : MonoBehaviour
{
    public int[] array = { 5, 3, 8, 4, 2 };

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        UnityEngine.Debug.Log("정렬 전: " + string.Join(", ", array));

        stopwatch.Start();
        QuickSortArray(array, 0, array.Length - 1);
        stopwatch.Stop();

        UnityEngine.Debug.Log("정렬 후: " + string.Join(", ", array));
        UnityEngine.Debug.Log("실행 시간 (Quick Sort): " + stopwatch.ElapsedMilliseconds + "ms");
    }

    void QuickSortArray(int[] array, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(array, low, high);
            QuickSortArray(array, low, pi - 1);
            QuickSortArray(array, pi + 1, high);
        }
    }

    int Partition(int[] array, int low, int high)
    {
        int pivot = array[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (array[j] < pivot)
            {
                i++;
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        int swapTemp = array[i + 1];
        array[i + 1] = array[high];
        array[high] = swapTemp;

        return i + 1;
    }
}
