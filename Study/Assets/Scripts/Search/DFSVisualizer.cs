using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSVisualizer : MonoBehaviour
{
    [Header("DFS Settings")]
    public GraphNode startNode;       // 탐색 시작 노드
    public string targetName;         // 찾고자 하는 노드 이름
    public float delay = 1f;          // 각 단계 사이의 지연 시간

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

        // 현재 노드를 방문
        visited.Add(current);
        current.SetColor(Color.yellow); // 현재 방문 중인 노드 색상 변경

        // 탐색 경로에 추가
        path.Add(current);
        // 찾고자 하는 노드인지 확인
        if (current.nodeName.Equals(target))
        {
            current.SetColor(Color.green); // 찾은 노드 색상 변경
            yield break;
        }

        yield return new WaitForSeconds(delay);

        // 자식 노드를 재귀적으로 방문
        foreach (var neighbor in current.neighbors)
        {
            if (!visited.Contains(neighbor))
            {
                yield return StartCoroutine(DFSCoroutine(neighbor, target));
            }
        }

        // 탐색이 끝난 노드는 원래 색상으로 복귀
        current.SetColor(Color.white);
    }
}
