using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.Pool
{
    public class PoolManager : MonoBehaviour
    {
        // 각 프리팹 풀을 string 키(예: prefab의 이름 또는 지정한 식별자)로 관리합니다.
        private Dictionary<string, ObjectPool> poolDictionary = new Dictionary<string, ObjectPool>();

        /// <summary>
        /// 지정한 키와 프리팹으로 풀을 생성합니다.
        /// </summary>
        public void CreatePool(string poolKey, GameObject prefab, int poolSize)
        {
            if (!poolDictionary.ContainsKey(poolKey))
            {
                GameObject poolObj = new GameObject(poolKey + "Pool");
                poolObj.transform.parent = transform;
                ObjectPool pool = poolObj.AddComponent<ObjectPool>();
                pool.SetupPool(prefab, poolSize);
                poolDictionary.Add(poolKey, pool);
            }
            else
            {
                Debug.LogWarning($"풀 키 '{poolKey}'가 이미 존재합니다.");
            }
        }

        /// <summary>
        /// 지정한 키에 해당하는 풀에서 오브젝트를 스폰합니다.
        /// </summary>
        public GameObject Spawn(string poolKey, Vector3 position, Quaternion rotation)
        {
            if (poolDictionary.ContainsKey(poolKey))
            {
                GameObject obj = poolDictionary[poolKey].GetObject();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj;
            }
            else
            {
                Debug.LogError($"풀 키 '{poolKey}'가 존재하지 않습니다.");
                return null;
            }
        }

        /// <summary>
        /// 사용이 끝난 오브젝트를 지정한 풀에 반환합니다.
        /// </summary>
        public void ReturnToPool(string poolKey, GameObject obj)
        {
            if (poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary[poolKey].ReturnObject(obj);
            }
            else
            {
                Debug.LogWarning($"풀 키 '{poolKey}'가 없으므로 오브젝트를 제거합니다.");
                Destroy(obj);
            }
        }
    }
}
