using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearSearchVisualizer : MonoBehaviour
{
    public List<GameObject> objectsToSearch;
    public string targetName;
    public float delay = 1f; 					// 각 단계 사이의 지연 시간

    private void Start()
    {
    }

    public void OnClickStart()
    {
        StartCoroutine(LinearSearchCoroutine(targetName));
    }

    IEnumerator LinearSearchCoroutine(string target)
    {
        for (int i = 0; i < objectsToSearch.Count; i++)
        {
            // 현재 검색 중인 오브젝트를 강조
            objectsToSearch[i].GetComponent<Renderer>().material.color = Color.yellow;

            if (objectsToSearch[i].name.Equals(target))
            {
                Debug.Log($"{target} found at index {i}");
                objectsToSearch[i].GetComponent<Renderer>().material.color = Color.green;
                yield break;
            }
            else
            {
                // 검색 실패 시 원래 색상으로 복귀
                objectsToSearch[i].GetComponent<Renderer>().material.color = Color.white;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}