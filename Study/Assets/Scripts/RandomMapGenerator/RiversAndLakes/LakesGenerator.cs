using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class LakesGenerator : MonoBehaviour
{
    [Header("Lake Settings")]
    public int lakeCount = 5;
    public int lakeRadius = 50;
    public float lakeDepth = 0.05f;

    [Header("Water Settings")]
    public GameObject waterPrefab; // 인스펙터에서 할당할 물 프리팹

    private Terrain terrain;
    private TerrainData terrainData;
    private float[,] heights;

    public void GenerateLakes()
    {
        Debug.Log("Generating lakes...");

        terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("Terrain component not found!");
            return;
        }

        terrainData = terrain.terrainData;
        heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int i = 0; i < lakeCount; i++)
        {
            int centerX = Random.Range(lakeRadius, terrainData.heightmapResolution - lakeRadius - 1);
            int centerZ = Random.Range(lakeRadius, terrainData.heightmapResolution - lakeRadius - 1);

            for (int z = centerZ - lakeRadius; z <= centerZ + lakeRadius; z++)
            {
                for (int x = centerX - lakeRadius; x <= centerX + lakeRadius; x++)
                {
                    if (x >= 0 && x < terrainData.heightmapResolution &&
                        z >= 0 && z < terrainData.heightmapResolution)
                    {
                        float distance = Vector2.Distance(new Vector2(x, z), new Vector2(centerX, centerZ));
                        if (distance <= lakeRadius)
                        {
                            float depthAdjustment = lakeDepth * (1 - (distance / lakeRadius));
                            heights[z, x] = Mathf.Clamp01(heights[z, x] - depthAdjustment);
                        }
                    }
                }
            }

            // 물 오브젝트 배치
            if (waterPrefab != null)
            {
                Vector3 lakePosition = new Vector3(
                    (float)centerX / terrainData.heightmapResolution * terrainData.size.x,
                    terrain.SampleHeight(new Vector3(
                        (float)centerX / terrainData.heightmapResolution * terrainData.size.x,
                        0,
                        (float)centerZ / terrainData.heightmapResolution * terrainData.size.z)),
                    (float)centerZ / terrainData.heightmapResolution * terrainData.size.z
                );

                // 물의 Y 위치를 지형 높이보다 약간 높게 설정하여 물이 지형 위에 나타나도록 함
                lakePosition.y += 0.1f; // 작은 오프셋

                Instantiate(waterPrefab, lakePosition, Quaternion.identity, this.transform);
            }
            else
            {
                Debug.LogWarning("WaterPrefab is not assigned in LakesGenerator.");
            }
        }

        terrainData.SetHeights(0, 0, heights);
        Debug.Log("Lakes generated.");
    }
}
