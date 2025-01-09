using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashSearchManager : MonoBehaviour
{
    public List<GameObject> objectsToHash;
    public string targetName;
    public float delay = 1f; 			// 시각화 지연 시간

    private Dictionary<string, GameObject> objectDictionary;

    private void Start()
    {
        // 해시 테이블 생성
        objectDictionary = new Dictionary<string, GameObject>();
        foreach (var obj in objectsToHash)
        {
            if (!objectDictionary.ContainsKey(obj.name))
            {
                objectDictionary.Add(obj.name, obj);
            }
        }
    }

    public void OnClickStart()
    {
        StartCoroutine(HashSearchCoroutine(targetName));
    }

    IEnumerator HashSearchCoroutine(string target)
    {
        yield return new WaitForSeconds(delay);

        if (objectDictionary.TryGetValue(target, out GameObject foundObject))
        {
            foundObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}