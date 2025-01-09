using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSVisualizer : MonoBehaviour
{
    [Header("BFS Settings")]
    public GraphNode startNode;       // Ž�� ���� ���
    public string targetName;         // ã���� �ϴ� ��� �̸�
    public float delay = 1f;          // �� �ܰ� ������ ���� �ð�

    private HashSet<GraphNode> visited; // �湮�� ��带 ����
    private Queue<GraphNode> queue;     // BFS ť

    private void Start()
    {
        if (startNode == null)
        {
            Debug.LogError("BFSVisualizer: Start Node�� �������� �ʾҽ��ϴ�.");
            return;
        }

        visited = new HashSet<GraphNode>();
        queue = new Queue<GraphNode>();
        queue.Enqueue(startNode);
        Debug.Log($"Enqueued start node: {startNode.nodeName}");
    }

    public void OnClickStart()
    {
        StartCoroutine(BFSCoroutine(targetName));
    }

    IEnumerator BFSCoroutine(string target)
    {
        while (queue.Count > 0)
        {
            GraphNode current = queue.Dequeue();
            Debug.Log($"Dequeued node: {current.nodeName}");

            if (visited.Contains(current))
            {
                Debug.Log($"Already visited node: {current.nodeName}");
                continue;
            }

            visited.Add(current);
            current.SetColor(Color.yellow);
            Debug.Log($"Visiting node: {current.nodeName}");

            if (current.nodeName.Equals(target))
            {
                current.SetColor(Color.green);
                Debug.Log($"Target node {target} found!");
                yield break;
            }

            yield return new WaitForSeconds(delay);

            foreach (var neighbor in current.neighbors)
            {
                if (neighbor != null && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    Debug.Log($"Enqueued neighbor node: {neighbor.nodeName}");
                    neighbor.SetColor(Color.blue); // ť�� �߰��� ��� ���� ���� (�ɼ�)
                }
            }

            // �湮 �Ϸ� �� ���� �������� ���� (�ɼ�)
            current.SetColor(Color.white);
        }

        Debug.Log($"Target {target} not found.");
    }
}
