using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeapSortVisualizer : MonoBehaviour
{
    public SortingVisualizer sortingVisualizer;

    public IEnumerator HeapSort()
    {
        yield return StartCoroutine(HeapSortArray(sortingVisualizer.heights));
    }

    private IEnumerator HeapSortArray(List<int> list)
    {
        int n = list.Count;

        // Step 1: Build a min-heap (부모 노드는 항상 자식 노드보다 작도록)
        for (int i = n / 2 - 1; i >= 0; i--)  // 힙을 만들 때는 마지막 비자식 노드부터 시작
        {
            yield return StartCoroutine(Heapify(list, n, i));  // 부모가 항상 자식보다 작도록 힙 유지
        }

        // Step 2: Extract elements one by one from the heap (가장 작은 값을 루트로부터 빼내어 배열의 끝으로 이동)
        for (int i = n - 1; i >= 0; i--)
        {
            // Swap the root (min value) with the last element
            int temp = list[0];
            list[0] = list[i];
            list[i] = temp;

            // Call Heapify on the reduced heap
            yield return StartCoroutine(Heapify(list, i, 0));  // 힙 크기를 줄이면서 재정렬

            // 큐브 위치 업데이트
            sortingVisualizer.UpdateCubePositions();
            yield return new WaitForSeconds(0.1f);  // 정렬 진행 상태를 볼 수 있도록 잠시 대기
        }
    }

    private IEnumerator Heapify(List<int> list, int n, int i)
    {
        int smallest = i;  // 루트를 가장 작은 값으로 초기화
        int left = 2 * i + 1;  // 왼쪽 자식
        int right = 2 * i + 2; // 오른쪽 자식

        // 왼쪽 자식이 루트보다 작으면 smallest를 왼쪽 자식으로 업데이트
        if (left < n && list[left] < list[smallest])
            smallest = left;

        // 오른쪽 자식이 smallest보다 작으면 smallest를 오른쪽 자식으로 업데이트
        if (right < n && list[right] < list[smallest])
            smallest = right;

        // smallest가 루트와 다르면, root와 smallest를 교환하고, 계속해서 힙 속성을 유지하도록 heapify를 재귀적으로 호출
        if (smallest != i)
        {
            int swap = list[i];
            list[i] = list[smallest];
            list[smallest] = swap;

            // Heapify를 재귀적으로 호출하여 힙 속성을 유지
            yield return StartCoroutine(Heapify(list, n, smallest));
        }
    }
}
