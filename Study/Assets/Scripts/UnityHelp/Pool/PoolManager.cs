using System.Collections.Generic;
using UnityEngine;

namespace UnityHelp.Pool
{
    public class PoolManager : MonoBehaviour
    {
        // �� ������ Ǯ�� string Ű(��: prefab�� �̸� �Ǵ� ������ �ĺ���)�� �����մϴ�.
        private Dictionary<string, ObjectPool> poolDictionary = new Dictionary<string, ObjectPool>();

        /// <summary>
        /// ������ Ű�� ���������� Ǯ�� �����մϴ�.
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
                Debug.LogWarning($"Ǯ Ű '{poolKey}'�� �̹� �����մϴ�.");
            }
        }

        /// <summary>
        /// ������ Ű�� �ش��ϴ� Ǯ���� ������Ʈ�� �����մϴ�.
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
                Debug.LogError($"Ǯ Ű '{poolKey}'�� �������� �ʽ��ϴ�.");
                return null;
            }
        }

        /// <summary>
        /// ����� ���� ������Ʈ�� ������ Ǯ�� ��ȯ�մϴ�.
        /// </summary>
        public void ReturnToPool(string poolKey, GameObject obj)
        {
            if (poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary[poolKey].ReturnObject(obj);
            }
            else
            {
                Debug.LogWarning($"Ǯ Ű '{poolKey}'�� �����Ƿ� ������Ʈ�� �����մϴ�.");
                Destroy(obj);
            }
        }
    }
}
