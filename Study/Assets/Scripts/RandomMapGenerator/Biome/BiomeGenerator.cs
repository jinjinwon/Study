using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    public Biome[] biomes;
    public float[,] moistureMap; // 습도 맵 (TerrainGenerator에서 생성)

    public BiomeType GetBiome(float height, float moisture)
    {
        foreach (Biome biome in biomes)
        {
            Debug.Log($"height : {height} || moisture : {moisture}");

            if (height >= biome.minHeight && height <= biome.maxHeight &&
                moisture >= biome.minMoisture && moisture <= biome.maxMoisture)
            {
                return biome.type;
            }
        }
        return BiomeType.Forest; // 기본값
    }
}
