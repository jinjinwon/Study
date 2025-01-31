using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Terrain))]
public class RiversGenerator : MonoBehaviour
{
    [Header("River Settings")]
    public int riverCount = 3;
    public int riverLength = 300;
    public float riverDepth = 0.02f;

    [Header("Water Settings")]
    public GameObject waterPrefab; // 인스펙터에서 할당할 물 프리팹

    private Terrain terrain;
    private TerrainData terrainData;
    private float[,] heights;

    public void GenerateRivers(TerrainData data)
    {
        Debug.Log("Generating rivers...");

        terrainData = data;
        terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("Terrain component not found!");
            return;
        }

        heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int i = 0; i < riverCount; i++)
        {
            // 시작점 찾기 (높이가 높은 지점)
            int startX = Random.Range(0, terrainData.heightmapResolution);
            int startZ = Random.Range(0, terrainData.heightmapResolution);

            // 시작점의 높이가 평균 이하라면 다시 선택
            float averageHeight = 0f;
            foreach (var h in heights)
                averageHeight += h;
            averageHeight /= heights.Length;

            if (heights[startZ, startX] < averageHeight)
            {
                i--;
                continue;
            }

            int currentX = startX;
            int currentZ = startZ;
            List<Vector2Int> riverPath = new List<Vector2Int>();

            for (int j = 0; j < riverLength; j++)
            {
                if (currentX < 0 || currentX >= terrainData.heightmapResolution ||
                    currentZ < 0 || currentZ >= terrainData.heightmapResolution)
                    break;

                heights[currentZ, currentX] = Mathf.Clamp01(heights[currentZ, currentX] - riverDepth);

                riverPath.Add(new Vector2Int(currentX, currentZ));

                // 다음 지점 결정 (가장 낮은 인접 지점으로 이동)
                Vector2Int nextPoint = GetLowestNeighbor(currentX, currentZ);
                if (nextPoint.x == currentX && nextPoint.y == currentZ)
                    break; // 더 이상 이동할 수 없을 때 종료

                currentX = nextPoint.x;
                currentZ = nextPoint.y;
            }

            // 물 오브젝트 배치
            if (waterPrefab != null)
            {
                foreach (var point in riverPath)
                {
                    Vector3 riverPosition = new Vector3(
                        (float)point.x / terrainData.heightmapResolution * terrainData.size.x,
                        terrain.SampleHeight(new Vector3(
                            (float)point.x / terrainData.heightmapResolution * terrainData.size.x,
                            0,
                            (float)point.y / terrainData.heightmapResolution * terrainData.size.z)),
                        (float)point.y / terrainData.heightmapResolution * terrainData.size.z
                    );

                    // 물의 Y 위치를 지형 높이보다 약간 높게 설정하여 물이 지형 위에 나타나도록 함
                    riverPosition.y += 0.1f; // 작은 오프셋

                    Instantiate(waterPrefab, riverPosition, Quaternion.identity, this.transform);
                }
            }
            else
            {
                Debug.LogWarning("WaterPrefab is not assigned in RiversGenerator.");
            }
        }

        terrainData.SetHeights(0, 0, heights);
        Debug.Log("Rivers generated.");
    }

    Vector2Int GetLowestNeighbor(int x, int z)
    {
        float minHeight = heights[z, x];
        Vector2Int minPoint = new Vector2Int(x, z);

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dz = -1; dz <= 1; dz++)
            {
                if (dx == 0 && dz == 0)
                    continue;

                int neighborX = x + dx;
                int neighborZ = z + dz;

                if (neighborX >= 0 && neighborX < terrainData.heightmapResolution &&
                    neighborZ >= 0 && neighborZ < terrainData.heightmapResolution)
                {
                    if (heights[neighborZ, neighborX] < minHeight)
                    {
                        minHeight = heights[neighborZ, neighborX];
                        minPoint = new Vector2Int(neighborX, neighborZ);
                    }
                }
            }
        }

        return minPoint;
    }
}
