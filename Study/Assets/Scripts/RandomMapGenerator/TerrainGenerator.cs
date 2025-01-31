using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Terrain))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain Settings")]
    public int width = 512;                             // 지형의 너비
    public int depth = 512;                             // 지형의 깊이
    public int height = 100;                            // 지형의 최대 높이
    public float scale = 20f;                           // 노이즈 스케일

    [Header("Noise Settings")]
    public int octaves = 4;                             // 노이즈 층 수
    [Range(0, 1)]
    public float persistence = 0.5f;                    // 진폭 감소율
    public float lacunarity = 2f;                       // 주파수 증가율

    [Header("Moisture Settings")]
    public float moistureScale = 30f;                   // 습도 맵 스케일
    public Vector2 moistureOffset;                      // 습도 맵 오프셋

    [Header("Biome Settings")]
    public BiomeGenerator biomeGenerator;

    [Header("Object Settings")]
    public GameObject[] treePrefabs;
    public int treeCount = 1000;

    public GameObject[] rockPrefabs;
    public int rockCount = 500;
    public float rockHeightOffset = 0.1f;

    public GameObject[] grassPrefabs;
    public int grassCount = 1000;
    public float grassHeightOffset = 0.05f;

    [Header("Cave Settings ------- 사용 안해용")]
    public int caveWidth = 512;
    public int caveHeight = 512;
    public int initialFillProbability = 45;
    public int smoothingIterations = 5;

    [Header("River Settings ------- 사용 안해용")]
    public int riverCount = 3;
    public int riverLength = 300;
    public float riverDepth = 0.02f;

    [Header("Lake Settings ------- 사용 안해용")]
    public int lakeCount = 5;
    public int lakeRadius = 50;
    public float lakeDepth = 0.05f;

    [Header("Water Settings ------- 사용 안해용")]
    public GameObject waterPrefab; // 인스펙터에서 할당할 물 프리팹

    private Vector2 offset;

    [SerializeField] private Terrain terrain;
    private TerrainData terrainData;
    private float[,] heights;
    private float[,] moistureMap;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        offset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));
        moistureOffset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));

        GenerateTerrain();
        GenerateMoistureMap();
        ApplyBiomes();

        //GenerateCaves();  // 오류가 있음..
        //GenerateRivers(); // 자연스럽지가 않음..
        //GenerateLakes();  // 자연스럽지가 않음..
        PlaceObjects();
    }

    public void GenerateTerrain()
    {
        terrainData = terrain.terrainData;
        terrainData.heightmapResolution = Mathf.Max(width, depth) + 1; // 높이맵 해상도는 가장 큰 축 기준
        terrainData.size = new Vector3(width, height, depth);
        heights = GenerateHeights();
        terrainData.SetHeights(0, 0, heights);
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[depth, width]; // [z, x] 인덱싱
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x + offset.x) / scale * frequency;
                    float sampleZ = (z + offset.y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                heights[z, x] = Mathf.Clamp01(noiseHeight * 0.5f + 0.5f);
            }
        }
        return heights;
    }

    float[,] GenerateMoistureMap()
    {
        moistureMap = new float[depth, width];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = (x + moistureOffset.x) / moistureScale;
                float sampleZ = (z + moistureOffset.y) / moistureScale;
                moistureMap[z, x] = Mathf.PerlinNoise(sampleX, sampleZ);
            }
        }
        return moistureMap;
    }

    void ApplyBiomes()
    {
        if (biomeGenerator == null || biomeGenerator.biomes.Length == 0)
        {
            return;
        }

        // 바이옴에 따른 텍스처 적용
        ApplyBiomeTextures();
    }

    void ApplyBiomeTextures()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        // 각 바이옴의 텍스처 레이어 추가
        foreach (Biome biome in biomeGenerator.biomes)
        {
            if (biome.terrainLayer != null && !terrainData.terrainLayers.Contains(biome.terrainLayer))
            {
                terrainData.terrainLayers = terrainData.terrainLayers.Concat(new TerrainLayer[] { biome.terrainLayer }).ToArray();
            }
        }

        // 텍스처 매핑
        float[,,] alphaMaps = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.terrainLayers.Length];

        for (int z = 0; z < terrainData.alphamapHeight; z++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // 높이맵과 모이스처 맵의 좌표 변환
                int heightmapX = Mathf.RoundToInt((float)x / terrainData.alphamapWidth * width);
                int heightmapZ = Mathf.RoundToInt((float)z / terrainData.alphamapHeight * depth);
                heightmapX = Mathf.Clamp(heightmapX, 0, width - 1);
                heightmapZ = Mathf.Clamp(heightmapZ, 0, depth - 1);

                float heightNormalized = heights[heightmapZ, heightmapX];
                float moisture = moistureMap[heightmapZ, heightmapX];
                BiomeType biomeType = biomeGenerator.GetBiome(heightNormalized, moisture);
                Biome currentBiome = GetBiomeByType(biomeType);

                for (int i = 0; i < terrainData.terrainLayers.Length; i++)
                {
                    if (terrainData.terrainLayers[i] == currentBiome.terrainLayer)
                    {
                        alphaMaps[z, x, i] = 1f;
                    }
                    else
                    {
                        alphaMaps[z, x, i] = 0f;
                    }
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMaps);
    }

    void GenerateCaves()
    {
        CaveGenerator caveGenerator = gameObject.AddComponent<CaveGenerator>();
        caveGenerator.caveWidth = caveWidth;
        caveGenerator.caveHeight = caveHeight;
        caveGenerator.initialFillProbability = initialFillProbability;
        caveGenerator.smoothingIterations = smoothingIterations;
        caveGenerator.GenerateCave();
        caveGenerator.ApplyCaveToTerrain();
    }

    void GenerateRivers()
    {
        RiversGenerator riversGenerator = gameObject.AddComponent<RiversGenerator>();
        riversGenerator.riverCount = riverCount;
        riversGenerator.riverLength = riverLength;
        riversGenerator.riverDepth = riverDepth;
        riversGenerator.waterPrefab = waterPrefab; 
        riversGenerator.GenerateRivers(terrainData);
    }

    void GenerateLakes()
    {
        LakesGenerator lakesGenerator = gameObject.AddComponent<LakesGenerator>();
        lakesGenerator.lakeCount = lakeCount;
        lakesGenerator.lakeRadius = lakeRadius;
        lakesGenerator.lakeDepth = lakeDepth;
        lakesGenerator.waterPrefab = waterPrefab; 
        lakesGenerator.GenerateLakes();
    }

    void PlaceObjects()
    {
        PlaceTrees();
        PlaceRocks();
        PlaceGrass();
    }

    void PlaceTrees()
    {
        if (biomeGenerator == null || biomeGenerator.biomes.Length == 0)
        {
            return;
        }

        Vector3 terrainPos = terrain.transform.position;

        for (int i = 0; i < treeCount; i++)
        {
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);

            // 월드 좌표를 높이맵 좌표로 변환
            int heightmapX = Mathf.RoundToInt((x / terrainData.size.x) * width);
            int heightmapZ = Mathf.RoundToInt((z / terrainData.size.z) * depth);
            heightmapX = Mathf.Clamp(heightmapX, 0, width - 1);
            heightmapZ = Mathf.Clamp(heightmapZ, 0, depth - 1);

            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;
            float heightNormalized = heights[heightmapZ, heightmapX];
            float moisture = moistureMap[heightmapZ, heightmapX];

            BiomeType biomeType = biomeGenerator.GetBiome(heightNormalized, moisture);
            Biome currentBiome = GetBiomeByType(biomeType);

            Vector3 position = new Vector3(x, y, z) + terrainPos;

            if (currentBiome.vegetationPrefabs.Length > 0)
            {
                GameObject vegetation = Instantiate(
                    currentBiome.vegetationPrefabs[Random.Range(0, currentBiome.vegetationPrefabs.Length)],
                    position,
                    Quaternion.identity
                );
                vegetation.transform.parent = this.transform;
            }
        }
    }

    void PlaceRocks()
    {
        if (rockPrefabs.Length == 0)
        {
            return;
        }

        Vector3 terrainPos = terrain.transform.position;

        for (int i = 0; i < rockCount; i++)
        {
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);

            // 월드 좌표를 높이맵 좌표로 변환
            int heightmapX = Mathf.RoundToInt((x / terrainData.size.x) * width);
            int heightmapZ = Mathf.RoundToInt((z / terrainData.size.z) * depth);
            heightmapX = Mathf.Clamp(heightmapX, 0, width - 1);
            heightmapZ = Mathf.Clamp(heightmapZ, 0, depth - 1);

            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;

            Vector3 position = new Vector3(x, y + rockHeightOffset, z) + terrainPos;

            GameObject rock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)], position, Quaternion.identity);
            rock.transform.parent = this.transform;
        }
    }

    void PlaceGrass()
    {
        if (grassPrefabs.Length == 0)
        {
            return;
        }

        Vector3 terrainPos = terrain.transform.position;

        for (int i = 0; i < grassCount; i++)
        {
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);

            // 월드 좌표를 높이맵 좌표로 변환
            int heightmapX = Mathf.RoundToInt((x / terrainData.size.x) * width);
            int heightmapZ = Mathf.RoundToInt((z / terrainData.size.z) * depth);
            heightmapX = Mathf.Clamp(heightmapX, 0, width - 1);
            heightmapZ = Mathf.Clamp(heightmapZ, 0, depth - 1);

            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;

            Vector3 position = new Vector3(x, y + grassHeightOffset, z) + terrainPos;

            GameObject grass = Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Length)], position, Quaternion.identity);
            grass.transform.parent = this.transform;
        }
    }

    Biome GetBiomeByType(BiomeType type)
    {
        foreach (Biome biome in biomeGenerator.biomes)
        {
            if (biome.type == type)
                return biome;
        }
        return biomeGenerator.biomes[0]; // 기본값
    }

#if UNITY_EDITOR
    //public void OnValidate()
    //{
    //    if (terrain == null)
    //    {
    //        terrain = GetComponent<Terrain>();
    //    }

    //    GenerateTerrain();
    //    GenerateMoistureMap();
    //    ApplyBiomes();

    //}
#endif
}
