using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GraphNode : MonoBehaviour
{
    [Header("Node Properties")]
    public string nodeName;
    public List<GraphNode> neighbors = new List<GraphNode>();

    private Renderer rend;

    private void Start()
    {
        // 노드의 이름을 설정
        gameObject.name = nodeName;
        rend = GetComponent<Renderer>();
        rend.material.color = Color.white; // 초기 색상
    }

    // 노드 색상 변경 메서드
    public void SetColor(Color color)
    {
        rend.material.color = color;
    }
}