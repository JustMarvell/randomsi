using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainChunk : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider;

    public void Generate(int resolution, float chunkSize, float noiseScale, float baseHeight, Vector2 worldOffset, Material material)
    {
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshCollider = GetComponent<MeshCollider>();

        int verts = resolution + 1;
        Vector3[] vertices = new Vector3[verts * verts];
        Vector2[] uvs = new Vector2[verts * verts];
        int[] triangles = new int[resolution * resolution * 6];

        BiomeType biome = BiomeMap.GetBiome(worldOffset);
        float heightScale = BiomeMap.GetHeightScale(biome);

        for (int z = 0; z < verts; z++)
        {
            for (int x = 0; x < verts; x++)
            {
                float worldX = worldOffset.x + (x / (float)resolution) * chunkSize;
                float worldZ = worldOffset.y + (z / (float)resolution) * chunkSize;

                float noise = Mathf.PerlinNoise(worldX * noiseScale, worldZ * noiseScale);
                float height = baseHeight - noise * heightScale;

                int i = z * verts + x;
                vertices[i] = new Vector3((x / (float)resolution) * chunkSize, height, (z / (float)resolution) * chunkSize);
                uvs[i] = new Vector2(x / (float)resolution, z / (float)resolution);
            }
        }

        int t = 0;
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = z * verts + x;
                triangles[t++] = i;
                triangles[t++] = i + verts;
                triangles[t++] = i + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + verts;
                triangles[t++] = i + verts + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;
        meshCollider.sharedMesh = mesh;
    }
}