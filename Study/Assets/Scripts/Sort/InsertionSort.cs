using System;
using System.Diagnostics;
using UnityEngine;

public class InsertionSort : MonoBehaviour
{
    public int[] array = { 5, 3, 8, 4, 2 };

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        UnityEngine.Debug.Log("���� ��: " + string.Join(", ", array));

        stopwatch.Start();
        InsertionSortArray();
        stopwatch.Stop();

        UnityEngine.Debug.Log("���� ��: " + string.Join(", ", array));
        UnityEngine.Debug.Log("���� �ð� (Insertion Sort): " + stopwatch.ElapsedMilliseconds + "ms");
    }

    void InsertionSortArray()
    {
        int n = array.Length;
        for (int i = 1; i < n; i++)
        {
            int key = array[i];
            int j = i - 1;
            while (j >= 0 && array[j] > key)
            {
                array[j + 1] = array[j];
                j--;
            }
            array[j + 1] = key;
        }
    }
}
