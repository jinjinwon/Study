using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarVisualizer : MonoBehaviour
{
    public AStarNode startNode;
    public AStarNode goalNode;
    public float delay = 0.5f;

    private List<AStarNode> openSet;
    private HashSet<AStarNode> closedSet;
    private Dictionary<AStarNode, AStarNode> cameFrom;
    private Dictionary<AStarNode, int> gScore;
    private Dictionary<AStarNode, int> fScore;

    private void Start()
    {
        openSet = new List<AStarNode>();
        closedSet = new HashSet<AStarNode>();
        cameFrom = new Dictionary<AStarNode, AStarNode>();
        gScore = new Dictionary<AStarNode, int>();
        fScore = new Dictionary<AStarNode, int>();

        openSet.Add(startNode);
        gScore[startNode] = 0;
        fScore[startNode] = GetHeuristic(startNode, goalNode);
    }

    public void OnClickStart()
    {
        StartCoroutine(AStarCoroutine());
    }

    IEnumerator AStarCoroutine()
    {
        while (openSet.Count > 0)
        {
            // fScore가 가장 낮은 노드 선택
            AStarNode current = openSet[0];
            foreach (var node in openSet)
            {
                if (fScore.ContainsKey(node) && fScore[node] < fScore[current])
                    current = node;
            }

            // 현재 노드를 강조 표시
            current.GetComponent<Renderer>().material.color = Color.yellow;

            if (current == goalNode)
            {
                ReconstructPath(current);
                yield break;
            }

            openSet.Remove(current);
            closedSet.Add(current);
            current.GetComponent<Renderer>().material.color = Color.red;

            foreach (var neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || neighbor.isObstacle)
                    continue;

                int tentativeGScore = gScore[current] + 1;

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tentativeGScore >= (gScore.ContainsKey(neighbor) ? gScore[neighbor] : int.MaxValue))
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + GetHeuristic(neighbor, goalNode);

                // 이웃 노드를 강조 표시
                neighbor.GetComponent<Renderer>().material.color = Color.blue;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    void ReconstructPath(AStarNode current)
    {
        while (cameFrom.ContainsKey(current))
        {
            current.GetComponent<Renderer>().material.color = Color.green;
            current = cameFrom[current];
        }
        startNode.GetComponent<Renderer>().material.color = Color.green;
    }

    int GetHeuristic(AStarNode a, AStarNode b)
    {
        // 맨해튼 거리 사용
        return Mathf.Abs(a.gridPosition.x - b.gridPosition.x) + Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
    }

    List<AStarNode> GetNeighbors(AStarNode node)
    {
        List<AStarNode> neighbors = new List<AStarNode>();
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        foreach (var dir in directions)
        {
            Vector2Int neighborPos = node.gridPosition + dir;
            AStarNode neighbor = FindNodeAtPosition(neighborPos);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    AStarNode FindNodeAtPosition(Vector2Int pos)
    {
        foreach (var node in FindObjectsOfType<AStarNode>())
        {
            if (node.gridPosition.Equals(pos))
                return node;
        }
        return null;
    }
}