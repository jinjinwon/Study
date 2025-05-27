using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class SimpleOcclusionCulling : MonoBehaviour
{
    [Header("Occluder Settings")]
    // 가림막이 될 오브젝트의 Layer
    public LayerMask occluderLayer;

    [Header("Occludee Settings")]
    // 가려질 오브젝트의 Render들 (Occludee태그가 붙은 모든 Render들을 등록)
    public List<Renderer> occludees = new List<Renderer>();

    private Camera cam;

    // 카메라 뷰의 6면 정보를 담는 배열
    private Plane[] frustumPlanes;
    // 축정렬 AABB의 8개의 코너의 좌표 저장용
    private Vector3[] bboxCorners = new Vector3[8];

    void OnEnable()
    {
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("Main Camera가 없습니다.");
            return;
        }

        // occludees 리스트가 비어있으면 태그로 자동 수집
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

        // 뷰 프러스텀 6면 정보 얻기
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
        Vector3 camPos = cam.transform.position;

        foreach (var rend in occludees)
        {
            if (rend == null) continue;

            // AABB가 뷰 프러스텀에 한 면이라도 벗어나면 return
            if (!GeometryUtility.TestPlanesAABB(frustumPlanes, rend.bounds))
            {
                rend.enabled = false;
                continue;
            }

            // rend.bounds의 Min/Max 좌표를 이용하여 8개의 코너 계산
            GetBoundingBoxCorners(rend.bounds, bboxCorners);

            bool anyVisible = false;
            for (int i = 0; i < bboxCorners.Length; i++)
            {
                Vector3 dir = bboxCorners[i] - camPos;
                float dist = dir.magnitude;

                if (Physics.Raycast(camPos, dir.normalized, out RaycastHit hit, dist, occluderLayer))
                {
                    // 자기 자신 Collider에 맞았으면 보임 처리
                    if (hit.collider.gameObject == rend.gameObject)
                    {
                        anyVisible = true;
                        break;
                    }

                    // 다른 물체에 가려졌으니 계속 검사
                    continue;
                }
                else
                {
                    // 아무 것도 안 맞으면 보임
                    anyVisible = true;
                    break;
                }
            }
            rend.enabled = anyVisible;
        }
    }

    // 8개의 꼭짓점 좌표를 계산하여 Raycast의 목표 지점으로 사용
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
