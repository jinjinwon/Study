using UnityEngine;

public class AStarNode : MonoBehaviour
{
    public Vector2Int gridPosition;
    public bool isObstacle = false;

    private void Start()
    {
        // �ð������� ��ֹ��� ǥ��
        if (isObstacle)
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
    }
}