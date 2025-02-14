using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.Pool
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;   // 풀링할 오브젝트 프리팹

        [SerializeField]
        private int poolSize = 10;   // 초기 풀 사이즈

        private Queue<GameObject> poolQueue = new Queue<GameObject>();

        /// <summary>
        /// 풀 초기화를 위한 메서드
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
        /// 풀에서 오브젝트를 꺼내 활성화된 상태로 반환합니다.
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

            // IPooledObject가 구현되어 있다면, OnSpawn() 호출

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
        /// 사용이 끝난 오브젝트를 풀에 반환합니다.
        /// </summary>
        public void ReturnObject(GameObject obj)
        {
            // IPooledObject가 구현되어 있다면, OnReturn() 호출
            if (obj.TryGetComponent(out IPooledObject pooled))
            {
                pooled?.OnReturn();
            }

            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }
}
