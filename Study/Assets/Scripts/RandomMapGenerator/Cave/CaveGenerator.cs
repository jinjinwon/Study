using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public int caveWidth = 1024;
    public int caveHeight = 1024;
    public int initialFillProbability = 45; // 0-100
    public int smoothingIterations = 5;

    private int[,] caveMap;

    public void GenerateCave()
    {
        caveMap = new int[caveWidth, caveHeight];

        // 초기 채우기
        for (int x = 0; x < caveWidth; x++)
        {
            for (int y = 0; y < caveHeight; y++)
            {
                if (x == 0 || x == caveWidth - 1 || y == 0 || y == caveHeight - 1)
                    caveMap[x, y] = 1; // 벽
                else
                    caveMap[x, y] = (Random.Range(0, 100) < initialFillProbability) ? 1 : 0;
            }
        }

        // 규칙 적용
        for (int i = 0; i < smoothingIterations; i++)
        {
            caveMap = SmoothMap(caveMap);
        }
    }

    int[,] SmoothMap(int[,] map)
    {
        int[,] newMap = new int[caveWidth, caveHeight];

        for (int x = 0; x < caveWidth; x++)
        {
            for (int y = 0; y < caveHeight; y++)
            {
                int wallCount = GetSurroundingWallCount(map, x, y);

                if (map[x, y] == 1)
                {
                    newMap[x, y] = (wallCount >= 4) ? 1 : 0;
                }
                else
                {
                    newMap[x, y] = (wallCount >= 5) ? 1 : 0;
                }
            }
        }

        return newMap;
    }

    int GetSurroundingWallCount(int[,] map, int x, int y)
    {
        int count = 0;
        for (int neighborX = x - 1; neighborX <= x + 1; neighborX++)
        {
            for (int neighborY = y - 1; neighborY <= y + 1; neighborY++)
            {
                if (neighborX >= 0 && neighborX < caveWidth && neighborY >= 0 && neighborY < caveHeight)
                {
                    if (neighborX != x || neighborY != y)
                    {
                        count += map[neighborX, neighborY];
                    }
                }
                else
                {
                    count += 1; // 경계는 벽으로 간주
                }
            }
        }
        return count;
    }

    public void ApplyCaveToTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int x = 0; x < caveWidth && x < heights.GetLength(0); x++)
        {
            for (int y = 0; y < caveHeight && y < heights.GetLength(1); y++)
            {
                if (caveMap[x, y] == 0)
                {
                    heights[x, y] = 0f; // 지형을 낮춤 (동굴 생성)
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}
