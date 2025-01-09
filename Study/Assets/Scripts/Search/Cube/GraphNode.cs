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
        // ����� �̸��� ����
        gameObject.name = nodeName;
        rend = GetComponent<Renderer>();
        rend.material.color = Color.white; // �ʱ� ����
    }

    // ��� ���� ���� �޼���
    public void SetColor(Color color)
    {
        rend.material.color = color;
    }
}