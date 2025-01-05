using System;
using System.Diagnostics;
using UnityEngine;

public class CountingSort : MonoBehaviour
{
    public int[] array = { 5, 3, 8, 4, 2 };

    void Start()
    {
        Stopwatch stopwatch = new Stopwatch();
        UnityEngine.Debug.Log("정렬 전: " + string.Join(", ", array));

        stopwatch.Start();
        CountingSortArray();
        stopwatch.Stop();

        UnityEngine.Debug.Log("정렬 후: " + string.Join(", ", array));
        UnityEngine.Debug.Log("실행 시간 (Counting Sort): " + stopwatch.ElapsedMilliseconds + "ms");
    }

    void CountingSortArray()
    {
        int max = array[0];
        int min = array[0];
        foreach (var num in array)
        {
            if (num > max) max = num;
            if (num < min) min = num;
        }

        int range = max - min + 1;
        int[] count = new int[range];
        int[] output = new int[array.Length];

        foreach (var num in array)
        {
            count[num - min]++;
        }

        for (int i = 1; i < count.Length; i++)
        {
            count[i] += count[i - 1];
        }

        for (int i = array.Length - 1; i >= 0; i--)
        {
            output[count[array[i] - min] - 1] = array[i];
            count[array[i] - min]--;
        }

        Array.Copy(output, array, array.Length);
    }
}
