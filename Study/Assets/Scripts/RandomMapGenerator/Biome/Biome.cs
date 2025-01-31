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
    public TerrainLayer terrainLayer; // ���� �ؽ�ó
    public GameObject[] vegetationPrefabs; // �ش� ���̿ȿ� ��ġ�� ������Ʈ��
}
