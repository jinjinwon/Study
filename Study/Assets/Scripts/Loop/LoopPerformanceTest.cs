using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LoopPerformanceTest : MonoBehaviour
{
    public class tempdata
    {
        public string name { get; set; }
    }

    void Start()
    {
        const int iterations = 10_000_000; // 반복 횟수
        int[] array = new int[iterations];
        for (int i = 0; i < iterations; i++) array[i] = i;

        List<tempdata> dataList = new List<tempdata>();

        // 리스트 초기화
        for (int i = 0; i < iterations; i++)
        {
            dataList.Add(new tempdata { name = $"Name_{i}" });
        }

        Stopwatch stopwatch = new Stopwatch();

        // for loop
        stopwatch.Start();
        //for (int i = 0; i < iterations; i++)
        //{
        //    //int temp = array[i] * 2;
        //    float temp = Mathf.Sqrt(array[i]) * Mathf.Sin(array[i]) + Mathf.Log(array[i] + 1);
        //}
        for (int i = 0; i < dataList.Count; i++)
        {
            string name = dataList[i].name;
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"for loop: {stopwatch.ElapsedMilliseconds} ms");

        // while loop
        stopwatch.Reset();
        stopwatch.Start();
        int j = 0;
        //while (j < iterations)
        //{
        //    //int temp = array[j] * 2;
        //    float temp = Mathf.Sqrt(array[j]) * Mathf.Sin(array[j]) + Mathf.Log(array[j] + 1);
        //    j++;
        //}
        while (j < dataList.Count)
        {
            string name = dataList[j].name;
            j++;
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"while loop: {stopwatch.ElapsedMilliseconds} ms");

        // foreach loop
        stopwatch.Reset();
        stopwatch.Start();
        //foreach (var value in array)
        //{
        //    //int temp = value * 2;
        //    float temp = Mathf.Sqrt(value) * Mathf.Sin(value) + Mathf.Log(value + 1);
        //}
        foreach (var data in dataList)
        {
            string name = data.name;
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"foreach loop: {stopwatch.ElapsedMilliseconds} ms");

        // LINQ
        stopwatch.Reset();
        stopwatch.Start();
        //var linqResult = array.Select(n => n * 2).ToArray();
        //var linqResult = array.Select(n => Mathf.Sqrt(n) * Mathf.Sin(n) + Mathf.Log(n + 1)).ToArray();
        var linqResult = dataList.Select(data => data.name).ToList();
        stopwatch.Stop();
        UnityEngine.Debug.Log($"LINQ: {stopwatch.ElapsedMilliseconds} ms");

        // Task
        stopwatch.Reset();
        stopwatch.Start();
        Task.Run(() =>
        {
            //for (int i = 0; i < iterations; i++)
            //{
            //    //int temp = array[i] * 2;
            //    float temp = Mathf.Sqrt(array[i]) * Mathf.Sin(array[i]) + Mathf.Log(array[i] + 1);
            //}
            for (int i = 0; i < dataList.Count; i++)
            {
                //int temp = array[i] * 2;
                //float temp = Mathf.Sqrt(array[i]) * Mathf.Sin(array[i]) + Mathf.Log(array[i] + 1);
                string name = dataList[i].name;
            }
        }).Wait();
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Task: {stopwatch.ElapsedMilliseconds} ms");

        // do-while loop
        stopwatch.Reset();
        stopwatch.Start();
        int k = 0;
        //do
        //{
        //    float temp = Mathf.Sqrt(array[k]) * Mathf.Sin(array[k]) + Mathf.Log(array[k] + 1);
        //    //int temp = array[k] * 2;
        //    k++;
        //} while (k < iterations);
        do
        {
            string name = dataList[k].name;
            k++;
        } while (k < dataList.Count);
        stopwatch.Stop();
        UnityEngine.Debug.Log($"do-while loop: {stopwatch.ElapsedMilliseconds} ms");
    }
}
