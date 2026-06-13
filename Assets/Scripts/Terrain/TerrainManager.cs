using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [Header("Chunk Settings")]
    public int resolution = 32;
    public float chunkSize = 50f;
    public float noiseScale = 0.02f;
    public float baseHeight = 0f;
    public Material terrainMaterial;

    [Header("Streaming")]
    public Transform player;
    public int viewDistanceInChunks = 3;

    [Header("World Border")]
    public int worldBorderInChunks = 10; // chunks from origin, in each direction

    Dictionary<Vector2Int, TerrainChunk> activeChunks = new Dictionary<Vector2Int, TerrainChunk>();
    Vector2Int currentPlayerChunk;

    void Start()
    {
        UpdateChunks(true);
    }

    void Update()
    {
        Vector2Int playerChunk = WorldToChunkCoord(player.position);
        if (playerChunk != currentPlayerChunk)
        {
            currentPlayerChunk = playerChunk;
            UpdateChunks(false);
        }
    }

    Vector2Int WorldToChunkCoord(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / chunkSize),
            Mathf.FloorToInt(worldPos.z / chunkSize)
        );
    }

    void UpdateChunks(bool force)
    {
        currentPlayerChunk = WorldToChunkCoord(player.position);
        HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();

        for (int dx = -viewDistanceInChunks; dx <= viewDistanceInChunks; dx++)
        {
            for (int dz = -viewDistanceInChunks; dz <= viewDistanceInChunks; dz++)
            {
                Vector2Int coord = currentPlayerChunk + new Vector2Int(dx, dz);

                if (Mathf.Abs(coord.x) > worldBorderInChunks || Mathf.Abs(coord.y) > worldBorderInChunks)
                    continue;

                neededChunks.Add(coord);

                if (!activeChunks.ContainsKey(coord))
                    SpawnChunk(coord);
            }
        }

        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kvp in activeChunks)
        {
            if (!neededChunks.Contains(kvp.Key))
                toRemove.Add(kvp.Key);
        }

        foreach (var coord in toRemove)
        {
            Destroy(activeChunks[coord].gameObject);
            activeChunks.Remove(coord);
        }
    }

    void SpawnChunk(Vector2Int coord)
    {
        GameObject go = new GameObject($"Chunk_{coord.x}_{coord.y}");
        go.transform.parent = transform;
        go.transform.position = new Vector3(coord.x * chunkSize, 0f, coord.y * chunkSize);
        go.layer = LayerMask.NameToLayer("Terrain");

        TerrainChunk chunk = go.AddComponent<TerrainChunk>();
        Vector2 worldOffset = new Vector2(coord.x * chunkSize, coord.y * chunkSize);
        chunk.Generate(resolution, chunkSize, noiseScale, baseHeight, worldOffset, terrainMaterial);

        activeChunks[coord] = chunk;
    }
}