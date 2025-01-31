using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    public GameObject[] rockPrefabs;
    public int rockCount = 500;
    public float rockHeightOffset = 0.1f;

    private Terrain terrain;
    private TerrainData terrainData;
    private float[,] heights;
    private float[,] moistureMap;
    private BiomeGenerator biomeGenerator;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        biomeGenerator = FindObjectOfType<BiomeGenerator>();
        if (biomeGenerator == null)
        {
            Debug.LogWarning("BiomeGenerator not found!");
            return;
        }

        moistureMap = biomeGenerator.moistureMap;

        PlaceRocks();
    }

    void PlaceRocks()
    {
        if (rockPrefabs.Length == 0)
        {
            Debug.LogWarning("No rock prefabs assigned!");
            return;
        }

        Vector3 terrainPos = terrain.transform.position;

        for (int i = 0; i < rockCount; i++)
        {
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;
            float heightNormalized = y / terrainData.size.y;
            float moisture = moistureMap[Mathf.FloorToInt(x), Mathf.FloorToInt(z)];

            BiomeType biomeType = biomeGenerator.GetBiome(heightNormalized, moisture);
            Biome currentBiome = biomeGenerator.biomes[System.Array.IndexOf(biomeGenerator.biomes, biomeType)];

            // 특정 바이옴에서만 바위 배치 (예: Mountain, Desert)
            if (currentBiome.type == BiomeType.Mountain || currentBiome.type == BiomeType.Desert)
            {
                Vector3 position = new Vector3(x, y + rockHeightOffset, z) + terrainPos;
                GameObject rock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)], position, Quaternion.identity);
                rock.transform.parent = this.transform;
            }
        }
    }
}
