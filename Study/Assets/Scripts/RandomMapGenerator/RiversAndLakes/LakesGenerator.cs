using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class LakesGenerator : MonoBehaviour
{
    [Header("Lake Settings")]
    public int lakeCount = 5;
    public int lakeRadius = 50;
    public float lakeDepth = 0.05f;

    [Header("Water Settings")]
    public GameObject waterPrefab; // �ν����Ϳ��� �Ҵ��� �� ������

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

            // �� ������Ʈ ��ġ
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

                // ���� Y ��ġ�� ���� ���̺��� �ణ ���� �����Ͽ� ���� ���� ���� ��Ÿ������ ��
                lakePosition.y += 0.1f; // ���� ������

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
