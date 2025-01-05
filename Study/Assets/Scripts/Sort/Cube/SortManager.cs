using UnityEngine;

public class SortManager : MonoBehaviour
{
    public BubbleSortVisualizer bubbleSortVisualizer;
    public SelectionSortVisualizer selectionSortVisualizer;
    public InsertionSortVisualizer insertionSortVisualizer;
    public MergeSortVisualizer mergeSortVisualizer;
    public QuickSortVisualizer quickSortVisualizer;
    public HeapSortVisualizer heapSortVisualizer;
    public CountingSortVisualizer countingSortVisualizer;
    public RadixSortVisualizer radixSortVisualizer;
    public TimSortVisualizer timSortVisualizer;

    private void Start()
    {
        Invoke("LateStart", 2f);
    }

    private void LateStart()
    {
        StartCoroutine(bubbleSortVisualizer.BubbleSort());
        StartCoroutine(selectionSortVisualizer.SelectionSort());
        StartCoroutine(insertionSortVisualizer.InsertionSort());
        StartCoroutine(mergeSortVisualizer.MergeSort());
        StartCoroutine(quickSortVisualizer.QuickSort());
        StartCoroutine(heapSortVisualizer.HeapSort());
        StartCoroutine(countingSortVisualizer.CountingSort());
        StartCoroutine(radixSortVisualizer.RadixSort());
        StartCoroutine(timSortVisualizer.TimSort());
    }
}
