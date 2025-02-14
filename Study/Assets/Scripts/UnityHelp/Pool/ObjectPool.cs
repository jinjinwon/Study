using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.Pool
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;   // Ǯ���� ������Ʈ ������

        [SerializeField]
        private int poolSize = 10;   // �ʱ� Ǯ ������

        private Queue<GameObject> poolQueue = new Queue<GameObject>();

        /// <summary>
        /// Ǯ �ʱ�ȭ�� ���� �޼���
        /// </summary>
        public void SetupPool(GameObject prefab, int poolSize)
        {
            this.prefab = prefab;
            this.poolSize = poolSize;

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }
        }

        /// <summary>
        /// Ǯ���� ������Ʈ�� ���� Ȱ��ȭ�� ���·� ��ȯ�մϴ�.
        /// </summary>
        public GameObject GetObject()
        {
            GameObject obj;
            if (poolQueue.Count > 0)
            {
                obj = poolQueue.Dequeue();
                obj.SetActive(true);
            }
            else
            {
                obj = Instantiate(prefab);
            }

            // IPooledObject�� �����Ǿ� �ִٸ�, OnSpawn() ȣ��

            if(obj.TryGetComponent(out IPooledObject pooled))
            {
                pooled?.OnSpawn();
            }

            if (obj.TryGetComponent(out IAutoReturn autoReturn))
            {
                autoReturn?.StartAutoReturn();
            }

            return obj;
        }

        /// <summary>
        /// ����� ���� ������Ʈ�� Ǯ�� ��ȯ�մϴ�.
        /// </summary>
        public void ReturnObject(GameObject obj)
        {
            // IPooledObject�� �����Ǿ� �ִٸ�, OnReturn() ȣ��
            if (obj.TryGetComponent(out IPooledObject pooled))
            {
                pooled?.OnReturn();
            }

            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }
}
