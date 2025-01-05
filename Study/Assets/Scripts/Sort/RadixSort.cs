using System;
using System.Diagnostics;
using UnityEngine;

public class RadixSort : MonoBehaviour
{
    public int[] array = { 170, 45, 75, 90, 802, 24, 2, 66 };

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        UnityEngine.Debug.Log("정렬 전: " + string.Join(", ", array));

        stopwatch.Start();
        RadixSortArray();
        stopwatch.Stop();

        UnityEngine.Debug.Log("정렬 후: " + string.Join(", ", array));
        UnityEngine.Debug.Log("실행 시간 (Radix Sort): " + stopwatch.ElapsedMilliseconds + "ms");
    }

    void RadixSortArray()
    {
        int max = array[0];
        foreach (var num in array)
        {
            if (num > max) max = num;
        }

        for (int exp = 1; max / exp > 0; exp *= 10)
        {
            CountingSortByDigit(exp);
        }
    }

    void CountingSortByDigit(int exp)
    {
        int[] output = new int[array.Length];
        int[] count = new int[10];

        for (int i = 0; i < array.Length; i++)
        {
            count[(array[i] / exp) % 10]++;
        }

        for (int i = 1; i < 10; i++)
        {
            count[i] += count[i - 1];
        }

        for (int i = array.Length - 1; i >= 0; i--)
        {
            output[count[(array[i] / exp) % 10] - 1] = array[i];
            count[(array[i] / exp) % 10]--;
        }

        Array.Copy(output, array, array.Length);
    }
}
