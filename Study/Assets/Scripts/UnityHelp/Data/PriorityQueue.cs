using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.Data
{
    /// <summary>
    /// �켱���� ť
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
        /// ������ �߰� (Enqueue)
        /// </summary>
        public void Enqueue(T item)
        {
            heap.Add(item);
            int ci = heap.Count - 1;
            // �� �������� ������ ��ġ�� �̵�(����� �� ����)
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // �θ� �ε���
                if (heap[ci].CompareTo(heap[pi]) >= 0)
                    break;
                // �θ�� �ڸ� ��ü
                T tmp = heap[ci];
                heap[ci] = heap[pi];
                heap[pi] = tmp;
                ci = pi;
            }
        }

        /// <summary>
        /// ���� ���� ��(�켱������ ���� ��)�� ��ȯ �� ���� (Dequeue)
        /// </summary>
        public T Dequeue()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("�켱���� ť�� ��� �ֽ��ϴ�.");

            int li = heap.Count - 1;
            T frontItem = heap[0];
            heap[0] = heap[li];
            heap.RemoveAt(li);
            li--;
            int pi = 0;
            // �������� �� ���� ���� (����� �� ����)
            while (true)
            {
                int ci = pi * 2 + 1; // ���� �ڽ�
                if (ci > li)
                    break;
                int rc = ci + 1; // ������ �ڽ�
                if (rc <= li && heap[rc].CompareTo(heap[ci]) < 0)
                    ci = rc;
                if (heap[pi].CompareTo(heap[ci]) <= 0)
                    break;
                // �θ�� �ڽ� ��ü
                T tmp = heap[pi];
                heap[pi] = heap[ci];
                heap[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        /// <summary>
        /// ���� ť�� ����ִ� ������ ��
        /// </summary>
        public int Count
        {
            get { return heap.Count; }
        }
    }
}
