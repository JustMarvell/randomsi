using UnityEngine;

public enum BiomeType
{
    OpenWater,
    CoralReef,
    KelpForest,
    Trench
}

// Returns a biome for a world position. Currently single biome, expand later with noise-based blending.
public static class BiomeMap
{
    public static BiomeType GetBiome(Vector2 worldPos)
    {
        return BiomeType.OpenWater;
    }

    // Per-biome height multiplier, used by TerrainChunk
    public static float GetHeightScale(BiomeType biome)
    {
        switch (biome)
        {
            case BiomeType.Trench: return 25f;
            case BiomeType.CoralReef: return 6f;
            case BiomeType.KelpForest: return 8f;
            default: return 12f;
        }
    }
}