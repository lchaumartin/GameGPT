using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public int platformSize = 5; // NxN platform size
    public float platformHeight = 1f; // Height of the platform
    public Material platformMaterial; // Material for the platform

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;
    private bool[,] holes; // Boolean array indicating the positions of the holes on the platform

    private void Start()
    {
        // Get the MeshFilter and MeshRenderer components from the game object
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        // Generate the platform mesh
        GeneratePlatform();
    }

    private void GeneratePlatform()
    {
        // Create a new mesh for the platform
        Mesh platformMesh = new Mesh();

        // Calculate the number of vertices and triangles in the mesh
        int numVertices = platformSize * platformSize * 4 * 2;
        int numTriangles = platformSize * platformSize * 4 * 3;

        // Create arrays to hold the vertex and triangle data for the mesh
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];
        Vector2[] UVs = new Vector2[numVertices]; 

        // Calculate the vertex positions for the platform
        int vertexIndex = 0;
        for (int z = 0; z < platformSize; z++)
        {
            for (int x = 0; x < platformSize; x++)
            {
                // Calculate the position of the current vertex
                float xPos = (x - platformSize / 2f) * 1f;
                float zPos = (z - platformSize / 2f) * 1f;
                float yPos = 0f;

                // Add the vertex to the vertex array
                vertices[vertexIndex] = new Vector3(xPos, yPos, zPos);
                vertices[vertexIndex + 1] = new Vector3(xPos + 1f, yPos, zPos);
                vertices[vertexIndex + 2] = new Vector3(xPos, yPos, zPos + 1f);
                vertices[vertexIndex + 3] = new Vector3(xPos + 1f, yPos, zPos + 1f);

                //bottom face
                vertices[vertexIndex + numVertices / 2] = new Vector3(xPos, yPos - 0.01f, zPos);
                vertices[vertexIndex + numVertices / 2 + 1] = new Vector3(xPos + 1f, yPos - 0.01f, zPos);
                vertices[vertexIndex + numVertices / 2 + 2] = new Vector3(xPos, yPos - 0.01f, zPos + 1f);
                vertices[vertexIndex + numVertices / 2 + 3] = new Vector3(xPos + 1f, yPos - 0.01f, zPos + 1f);

                // Setting up uvs
                UVs[vertexIndex] = new Vector2(x / (float)platformSize, z / (float)platformSize);
                UVs[vertexIndex + 1] = new Vector2((x + 1) / (float)platformSize, z / (float)platformSize);
                UVs[vertexIndex + 2] = new Vector2(x / (float)platformSize, (z + 1) / (float)platformSize);
                UVs[vertexIndex + 3] = new Vector2((x + 1) / (float)platformSize, (z + 1) / (float)platformSize);

                //bottom face
                UVs[vertexIndex + numVertices / 2] = new Vector2(x / (float)platformSize, z / (float)platformSize);
                UVs[vertexIndex + numVertices / 2 + 1] = new Vector2((x + 1) / (float)platformSize, z / (float)platformSize);
                UVs[vertexIndex + numVertices / 2 + 2] = new Vector2(x / (float)platformSize, (z + 1) / (float)platformSize);
                UVs[vertexIndex + numVertices / 2 + 3] = new Vector2((x + 1) / (float)platformSize, (z + 1) / (float)platformSize);

                // Increment the vertex index
                vertexIndex += 4;
            }
        }

        // Randomly position holes on the platform
        holes = new bool[platformSize, platformSize];
        int numHoles = Random.Range(1, platformSize * platformSize); // Choose a random number of holes between 1 and platformSize^2 - 1
        for (int i = 0; i < numHoles; i++)
        {
            int holeX = Random.Range(0, platformSize);
            int holeZ = Random.Range(0, platformSize);
            holes[holeX, holeZ] = true;
        }

        // Calculate the triangle indices for the platform
        int triangleIndex = 0;
        for (int z = 0; z < platformSize; z++)
        {
            for (int x = 0; x < platformSize; x++)
            {
                // Check if there is a hole at this position
                if (!holes[x, z])
                {
                    // Calculate the indices for the current quad
                    int tl = z * platformSize * 4 + x * 4;
                    int tr = tl + 1;
                    int bl = tl + 2;
                    int br = tl + 3;
                    // Add the triangles for the current quad
                    triangles[triangleIndex] = tl;
                    triangles[triangleIndex + 1] = bl;
                    triangles[triangleIndex + 2] = tr;
                    triangles[triangleIndex + 3] = tr;
                    triangles[triangleIndex + 4] = bl;
                    triangles[triangleIndex + 5] = br;

                    //bottom face
                    tl += numVertices / 2;
                    tr += numVertices / 2;
                    bl += numVertices / 2;
                    br += numVertices / 2;
                    triangles[triangleIndex + 6] = tl;
                    triangles[triangleIndex + 7] = tr;
                    triangles[triangleIndex + 8] = bl;
                    triangles[triangleIndex + 9] = tr;
                    triangles[triangleIndex + 10] = br;
                    triangles[triangleIndex + 11] = bl;

                    // Increment the triangle index
                    triangleIndex += 12;
                }
            }
        }

        // Assign the vertex and triangle data to the mesh
        platformMesh.vertices = vertices;
        platformMesh.triangles = triangles;
        platformMesh.uv = UVs;

        // Recalculate the mesh's normals to ensure lighting is correct
        platformMesh.RecalculateNormals();

        // Set the mesh for the MeshFilter component
        meshFilter.mesh = platformMesh;
        meshCollider.sharedMesh = platformMesh;

        // Set the material for the MeshRenderer component
        meshRenderer.material = platformMaterial;
    }
}