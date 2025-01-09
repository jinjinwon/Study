using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearSearchVisualizer : MonoBehaviour
{
    public List<GameObject> objectsToSearch;
    public string targetName;
    public float delay = 1f; 					// �� �ܰ� ������ ���� �ð�

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
            // ���� �˻� ���� ������Ʈ�� ����
            objectsToSearch[i].GetComponent<Renderer>().material.color = Color.yellow;

            if (objectsToSearch[i].name.Equals(target))
            {
                Debug.Log($"{target} found at index {i}");
                objectsToSearch[i].GetComponent<Renderer>().material.color = Color.green;
                yield break;
            }
            else
            {
                // �˻� ���� �� ���� �������� ����
                objectsToSearch[i].GetComponent<Renderer>().material.color = Color.white;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}