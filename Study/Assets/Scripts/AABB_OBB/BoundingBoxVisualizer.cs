using UnityEngine;

public class BoundingBoxVisualizer : MonoBehaviour
{
    [SerializeField] private Collider col;
    [SerializeField] private BoxCollider box;

    void OnDrawGizmos()
    {
        if (col != null)
        {
            Gizmos.color = Color.blue;
            Bounds aabb = col.bounds;
            Gizmos.DrawWireCube(aabb.center, aabb.size);
        }

        if (box != null)
        {
            Gizmos.color = Color.red;
            Vector3[] corners = GetBoxColliderCorners(box);

            // 12개의 엣지를 그려서 OBB를 시각화

            // 바닥 면 (0, 1, 2, 3)
            DrawLine(corners[0], corners[1]);
            DrawLine(corners[1], corners[2]);
            DrawLine(corners[2], corners[3]);
            DrawLine(corners[3], corners[0]);

            // 윗면 (4, 5, 6, 7)
            DrawLine(corners[4], corners[5]);
            DrawLine(corners[5], corners[6]);
            DrawLine(corners[6], corners[7]);
            DrawLine(corners[7], corners[4]);

            // 수직 엣지
            DrawLine(corners[0], corners[4]);
            DrawLine(corners[1], corners[5]);
            DrawLine(corners[2], corners[6]);
            DrawLine(corners[3], corners[7]);
        }
    }

    // BoxCollider의 로컬 코너를 월드 공간으로 변환하여 OBB의 8개 코너를 반환합니다.
    Vector3[] GetBoxColliderCorners(BoxCollider boxCollider)
    {
        Vector3[] corners = new Vector3[8];

        // BoxCollider의 로컬 중심과 크기를 가져옴
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size;
        Vector3 half = size * 0.5f;

        // 로컬 공간에서 8개의 코너 계산
        corners[0] = center + new Vector3(-half.x, -half.y, -half.z);
        corners[1] = center + new Vector3(half.x, -half.y, -half.z);
        corners[2] = center + new Vector3(half.x, -half.y, half.z);
        corners[3] = center + new Vector3(-half.x, -half.y, half.z);
        corners[4] = center + new Vector3(-half.x, half.y, -half.z);
        corners[5] = center + new Vector3(half.x, half.y, -half.z);
        corners[6] = center + new Vector3(half.x, half.y, half.z);
        corners[7] = center + new Vector3(-half.x, half.y, half.z);

        // 각 코너를 GameObject의 월드 좌표로 변환
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = boxCollider.transform.TransformPoint(corners[i]);
        }
        return corners;
    }

    // 두 점 사이의 선을 그립니다.
    void DrawLine(Vector3 start, Vector3 end)
    {
        Gizmos.DrawLine(start, end);
    }
}
