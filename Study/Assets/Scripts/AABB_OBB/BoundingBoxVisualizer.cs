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

            // 12���� ������ �׷��� OBB�� �ð�ȭ

            // �ٴ� �� (0, 1, 2, 3)
            DrawLine(corners[0], corners[1]);
            DrawLine(corners[1], corners[2]);
            DrawLine(corners[2], corners[3]);
            DrawLine(corners[3], corners[0]);

            // ���� (4, 5, 6, 7)
            DrawLine(corners[4], corners[5]);
            DrawLine(corners[5], corners[6]);
            DrawLine(corners[6], corners[7]);
            DrawLine(corners[7], corners[4]);

            // ���� ����
            DrawLine(corners[0], corners[4]);
            DrawLine(corners[1], corners[5]);
            DrawLine(corners[2], corners[6]);
            DrawLine(corners[3], corners[7]);
        }
    }

    // BoxCollider�� ���� �ڳʸ� ���� �������� ��ȯ�Ͽ� OBB�� 8�� �ڳʸ� ��ȯ�մϴ�.
    Vector3[] GetBoxColliderCorners(BoxCollider boxCollider)
    {
        Vector3[] corners = new Vector3[8];

        // BoxCollider�� ���� �߽ɰ� ũ�⸦ ������
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size;
        Vector3 half = size * 0.5f;

        // ���� �������� 8���� �ڳ� ���
        corners[0] = center + new Vector3(-half.x, -half.y, -half.z);
        corners[1] = center + new Vector3(half.x, -half.y, -half.z);
        corners[2] = center + new Vector3(half.x, -half.y, half.z);
        corners[3] = center + new Vector3(-half.x, -half.y, half.z);
        corners[4] = center + new Vector3(-half.x, half.y, -half.z);
        corners[5] = center + new Vector3(half.x, half.y, -half.z);
        corners[6] = center + new Vector3(half.x, half.y, half.z);
        corners[7] = center + new Vector3(-half.x, half.y, half.z);

        // �� �ڳʸ� GameObject�� ���� ��ǥ�� ��ȯ
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = boxCollider.transform.TransformPoint(corners[i]);
        }
        return corners;
    }

    // �� �� ������ ���� �׸��ϴ�.
    void DrawLine(Vector3 start, Vector3 end)
    {
        Gizmos.DrawLine(start, end);
    }
}
