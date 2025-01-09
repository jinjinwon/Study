using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSVisualizer : MonoBehaviour
{
    [Header("BFS Settings")]
    public GraphNode startNode;       // 탐색 시작 노드
    public string targetName;         // 찾고자 하는 노드 이름
    public float delay = 1f;          // 각 단계 사이의 지연 시간

    private HashSet<GraphNode> visited; // 방문한 노드를 추적
    private Queue<GraphNode> queue;     // BFS 큐

    private void Start()
    {
        if (startNode == null)
        {
            Debug.LogError("BFSVisualizer: Start Node가 설정되지 않았습니다.");
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
                    neighbor.SetColor(Color.blue); // 큐에 추가된 노드 색상 변경 (옵션)
                }
            }

            // 방문 완료 후 원래 색상으로 복귀 (옵션)
            current.SetColor(Color.white);
        }

        Debug.Log($"Target {target} not found.");
    }
}
