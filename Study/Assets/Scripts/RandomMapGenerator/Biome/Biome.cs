using UnityEngine;

public enum BiomeType { Forest, Desert, Mountain, Snow }

[System.Serializable]
public class Biome
{
    public BiomeType type;
    public float minHeight;
    public float maxHeight;
    public float minMoisture;
    public float maxMoisture;
    public TerrainLayer terrainLayer; // 지형 텍스처
    public GameObject[] vegetationPrefabs; // 해당 바이옴에 배치할 오브젝트들
}
