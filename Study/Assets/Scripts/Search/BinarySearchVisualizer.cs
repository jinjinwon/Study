using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySearchVisualizer : MonoBehaviour
{
    public List<GameObject> sortedObjects;  // 정렬된 리스트
    public string targetName;
    public float delay = 1f; 				// 각 단계 사이의 지연 시간

    private void Start()
    {
        // 데이터가 정렬되어 있는지 확인
        sortedObjects.Sort((a, b) => a.name.CompareTo(b.name));
    }

    public void OnClickStart()
    {
        StartCoroutine(BinarySearchCoroutine(targetName));
    }

    IEnumerator BinarySearchCoroutine(string target)
    {
        int left = 0;
        int right = sortedObjects.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            // 현재 중간 인덱스 오브젝트를 강조 표시
            sortedObjects[mid].GetComponent<Renderer>().material.color = Color.yellow;

            yield return new WaitForSeconds(delay);

            if (sortedObjects[mid].name.Equals(target))
            {
                sortedObjects[mid].GetComponent<Renderer>().material.color = Color.green;
                yield break;
            }
            else if (string.Compare(sortedObjects[mid].name, target) < 0)
            {
                // 중간 값보다 타겟이 클 경우
                sortedObjects[mid].GetComponent<Renderer>().material.color = Color.white;
                left = mid + 1;
            }
            else
            {
                // 중간 값보다 타겟이 작을 경우
                sortedObjects[mid].GetComponent<Renderer>().material.color = Color.white;
                right = mid - 1;
            }
        }
    }
}