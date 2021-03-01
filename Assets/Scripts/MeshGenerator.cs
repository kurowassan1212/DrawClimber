using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    public MeshCollider meshCollider;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        CreateMesh();
    }

   

    private void CreateMesh()
    {
        Vector3[] vertices = {
            new Vector3 (0, 0, 0),//front左下
            new Vector3 (0, 1, 0),//front左上
            new Vector3 (1, 1, 0),//front右上
            new Vector3 (1, 0, 0),//front右下
            
            new Vector3 (0, 0, 1),//back左下
            new Vector3 (0, 1, 1),//back左上
            new Vector3 (1, 1, 1),//back右上
            new Vector3 (1, 0, 1),//back右下
        };

        int[] triangles = {
            1, 4, 5, //startLid
            1, 0, 4,
            0, 1, 2, //front
		 	0, 2, 3,
            1, 5, 2, //top
			5, 6, 2,
            6, 5, 4, //back
			6, 4, 7,
            0, 3, 4, //bottom
			4, 3, 7,
            3, 2, 6, //endLid
            3, 6, 7,
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;

        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
    }
}