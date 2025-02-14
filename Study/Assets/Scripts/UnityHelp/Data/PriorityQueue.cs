using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.Data
{
    /// <summary>
    /// 우선순위 큐
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> heap;

        public PriorityQueue()
        {
            heap = new List<T>();
        }

        /// <summary>
        /// 아이템 추가 (Enqueue)
        /// </summary>
        public void Enqueue(T item)
        {
            heap.Add(item);
            int ci = heap.Count - 1;
            // 새 아이템을 적절한 위치로 이동(상향식 힙 정렬)
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // 부모 인덱스
                if (heap[ci].CompareTo(heap[pi]) >= 0)
                    break;
                // 부모와 자리 교체
                T tmp = heap[ci];
                heap[ci] = heap[pi];
                heap[pi] = tmp;
                ci = pi;
            }
        }

        /// <summary>
        /// 가장 낮은 값(우선순위가 높은 값)을 반환 및 제거 (Dequeue)
        /// </summary>
        public T Dequeue()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("우선순위 큐가 비어 있습니다.");

            int li = heap.Count - 1;
            T frontItem = heap[0];
            heap[0] = heap[li];
            heap.RemoveAt(li);
            li--;
            int pi = 0;
            // 내려가며 힙 구조 유지 (하향식 힙 정렬)
            while (true)
            {
                int ci = pi * 2 + 1; // 왼쪽 자식
                if (ci > li)
                    break;
                int rc = ci + 1; // 오른쪽 자식
                if (rc <= li && heap[rc].CompareTo(heap[ci]) < 0)
                    ci = rc;
                if (heap[pi].CompareTo(heap[ci]) <= 0)
                    break;
                // 부모와 자식 교체
                T tmp = heap[pi];
                heap[pi] = heap[ci];
                heap[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        /// <summary>
        /// 현재 큐에 들어있는 아이템 수
        /// </summary>
        public int Count
        {
            get { return heap.Count; }
        }
    }
}
