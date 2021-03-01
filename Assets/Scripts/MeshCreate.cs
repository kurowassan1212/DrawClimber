using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreate : MonoBehaviour
{
    private MeshFilter meshFilter;
    public Material material;
    private MeshRenderer meshRenderer;

    void Start()
    {
        Vector3[] vertices = {
            new Vector3(-1f, -1f, 0),
            new Vector3(-1f,  1f, 0),
            new Vector3( 1f,  1f, 0),
            new Vector3( 1f, -1f, 0)
        };

        int[] triangles = { 0, 1, 2, 0, 2, 3 };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;
    }
}
