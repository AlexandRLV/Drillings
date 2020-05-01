using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireframeUVGenerator : MonoBehaviour
{
    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        
        Mesh mesh = new Mesh();

        Mesh oldMesh = meshFilter.mesh;
        mesh.vertices = oldMesh.vertices;
        mesh.triangles = oldMesh.triangles;
        mesh.uv = new Vector2[oldMesh.uv.Length];

        for (int i = 0; i < oldMesh.vertices.Length - 2; i += 3)
        {
            mesh.uv[0] = new Vector2(0, 1);
            mesh.uv[1] = new Vector2(1, 1);
            mesh.uv[2] = new Vector2(1, 0);
        }
        
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}