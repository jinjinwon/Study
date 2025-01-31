using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    public GameObject[] grassPrefabs;
    public int grassCount = 1000;
    public float grassHeightOffset = 0.05f;

    private Terrain terrain;
    private TerrainData terrainData;
    private float[,] heights;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        PlaceGrass();
    }

    void PlaceGrass()
    {
        if (grassPrefabs.Length == 0)
        {
            Debug.LogWarning("No grass prefabs assigned!");
            return;
        }

        Vector3 terrainPos = terrain.transform.position;

        for (int i = 0; i < grassCount; i++)
        {
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;

            Vector3 position = new Vector3(x, y + grassHeightOffset, z) + terrainPos;

            GameObject grass = Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Length)], position, Quaternion.identity);
            grass.transform.parent = this.transform;
        }
    }
}
