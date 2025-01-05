using System;
using System.Diagnostics;
using UnityEngine;

public class BubbleSort : MonoBehaviour
{
    public int[] array = { 5, 3, 8, 4, 2 };

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        UnityEngine.Debug.Log("���� ��: " + string.Join(", ", array));

        stopwatch.Start();
        BubbleSortArray();
        stopwatch.Stop();

        UnityEngine.Debug.Log("���� ��: " + string.Join(", ", array));
        UnityEngine.Debug.Log("���� �ð� (Bubble Sort): " + stopwatch.ElapsedMilliseconds + "ms");
    }

    void BubbleSortArray()
    {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (array[j] > array[j + 1])
                {
                    // Swap elements
                    int temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
    }
}
