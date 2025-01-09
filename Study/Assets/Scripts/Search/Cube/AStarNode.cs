using UnityEngine;

public class AStarNode : MonoBehaviour
{
    public Vector2Int gridPosition;
    public bool isObstacle = false;

    private void Start()
    {
        // 시각적으로 장애물을 표시
        if (isObstacle)
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
    }
}