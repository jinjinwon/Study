using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class SimpleOcclusionCulling : MonoBehaviour
{
    [Header("Occluder Settings")]
    // �������� �� ������Ʈ�� Layer
    public LayerMask occluderLayer;

    [Header("Occludee Settings")]
    // ������ ������Ʈ�� Render�� (Occludee�±װ� ���� ��� Render���� ���)
    public List<Renderer> occludees = new List<Renderer>();

    private Camera cam;

    // ī�޶� ���� 6�� ������ ��� �迭
    private Plane[] frustumPlanes;
    // ������ AABB�� 8���� �ڳ��� ��ǥ �����
    private Vector3[] bboxCorners = new Vector3[8];

    void OnEnable()
    {
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("Main Camera�� �����ϴ�.");
            return;
        }

        // occludees ����Ʈ�� ��������� �±׷� �ڵ� ����
        if (occludees.Count == 0)
        {
            var tagged = GameObject.FindGameObjectsWithTag("Occludee");

            foreach (var go in tagged)
            {
                if (go.TryGetComponent<Renderer>(out var r))
                    occludees.Add(r);
            }
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // �� �������� 6�� ���� ���
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
        Vector3 camPos = cam.transform.position;

        foreach (var rend in occludees)
        {
            if (rend == null) continue;

            // AABB�� �� �������ҿ� �� ���̶� ����� return
            if (!GeometryUtility.TestPlanesAABB(frustumPlanes, rend.bounds))
            {
                rend.enabled = false;
                continue;
            }

            // rend.bounds�� Min/Max ��ǥ�� �̿��Ͽ� 8���� �ڳ� ���
            GetBoundingBoxCorners(rend.bounds, bboxCorners);

            bool anyVisible = false;
            for (int i = 0; i < bboxCorners.Length; i++)
            {
                Vector3 dir = bboxCorners[i] - camPos;
                float dist = dir.magnitude;

                if (Physics.Raycast(camPos, dir.normalized, out RaycastHit hit, dist, occluderLayer))
                {
                    // �ڱ� �ڽ� Collider�� �¾����� ���� ó��
                    if (hit.collider.gameObject == rend.gameObject)
                    {
                        anyVisible = true;
                        break;
                    }

                    // �ٸ� ��ü�� ���������� ��� �˻�
                    continue;
                }
                else
                {
                    // �ƹ� �͵� �� ������ ����
                    anyVisible = true;
                    break;
                }
            }
            rend.enabled = anyVisible;
        }
    }

    // 8���� ������ ��ǥ�� ����Ͽ� Raycast�� ��ǥ �������� ���
    void GetBoundingBoxCorners(Bounds b, Vector3[] corners)
    {
        Vector3 min = b.min, max = b.max;
        corners[0] = new Vector3(min.x, min.y, min.z);
        corners[1] = new Vector3(max.x, min.y, min.z);
        corners[2] = new Vector3(min.x, max.y, min.z);
        corners[3] = new Vector3(max.x, max.y, min.z);
        corners[4] = new Vector3(min.x, min.y, max.z);
        corners[5] = new Vector3(max.x, min.y, max.z);
        corners[6] = new Vector3(min.x, max.y, max.z);
        corners[7] = new Vector3(max.x, max.y, max.z);
    }
}
