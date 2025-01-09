using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSVisualizer : MonoBehaviour
{
    [Header("DFS Settings")]
    public GraphNode startNode;       // Ž�� ���� ���
    public string targetName;         // ã���� �ϴ� ��� �̸�
    public float delay = 1f;          // �� �ܰ� ������ ���� �ð�

    private HashSet<GraphNode> visited;
    private List<GraphNode> path = new List<GraphNode>();

    private void Start()
    {
        visited = new HashSet<GraphNode>();
    }

    public void OnClickStart()
    {
        StartCoroutine(DFSCoroutine(startNode, targetName));
    }

    IEnumerator DFSCoroutine(GraphNode current, string target)
    {
        if (current == null || visited.Contains(current))
            yield break;

        // ���� ��带 �湮
        visited.Add(current);
        current.SetColor(Color.yellow); // ���� �湮 ���� ��� ���� ����

        // Ž�� ��ο� �߰�
        path.Add(current);
        // ã���� �ϴ� ������� Ȯ��
        if (current.nodeName.Equals(target))
        {
            current.SetColor(Color.green); // ã�� ��� ���� ����
            yield break;
        }

        yield return new WaitForSeconds(delay);

        // �ڽ� ��带 ��������� �湮
        foreach (var neighbor in current.neighbors)
        {
            if (!visited.Contains(neighbor))
            {
                yield return StartCoroutine(DFSCoroutine(neighbor, target));
            }
        }

        // Ž���� ���� ���� ���� �������� ����
        current.SetColor(Color.white);
    }
}
