using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySearchVisualizer : MonoBehaviour
{
    public List<GameObject> sortedObjects;  // ���ĵ� ����Ʈ
    public string targetName;
    public float delay = 1f; 				// �� �ܰ� ������ ���� �ð�

    private void Start()
    {
        // �����Ͱ� ���ĵǾ� �ִ��� Ȯ��
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
            // ���� �߰� �ε��� ������Ʈ�� ���� ǥ��
            sortedObjects[mid].GetComponent<Renderer>().material.color = Color.yellow;

            yield return new WaitForSeconds(delay);

            if (sortedObjects[mid].name.Equals(target))
            {
                sortedObjects[mid].GetComponent<Renderer>().material.color = Color.green;
                yield break;
            }
            else if (string.Compare(sortedObjects[mid].name, target) < 0)
            {
                // �߰� ������ Ÿ���� Ŭ ���
                sortedObjects[mid].GetComponent<Renderer>().material.color = Color.white;
                left = mid + 1;
            }
            else
            {
                // �߰� ������ Ÿ���� ���� ���
                sortedObjects[mid].GetComponent<Renderer>().material.color = Color.white;
                right = mid - 1;
            }
        }
    }
}