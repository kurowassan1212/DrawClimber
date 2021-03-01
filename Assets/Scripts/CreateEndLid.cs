using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class CreateEndLid : MonoBehaviour
{
    public Vector3 leftVertexOfFront;
    public Vector3 rightVertexOfFront;
    public Vector3 leftVertexOfBack;
    public Vector3 rightVertexOfBack;

    private MeshFilter meshFilter;
    private Mesh mesh;
    public bool onePlay, frontVerticesOK, backVerticesOK;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh = new Mesh();
        mesh.name = "CreateStartLid";
        onePlay = true;
        frontVerticesOK = backVerticesOK = false;
    }
    void Update()
    {
        if (frontVerticesOK == true && backVerticesOK == true)
        {
            if (onePlay == true)
            {
                createMesh();
                onePlay = false;
            }
        }
        thisEnabledFalse();
    }


    void createMesh()
    {
        Vector3[] vertices = new Vector3[4];  // 四角なので頂点数は1つのメッシュに付き4つ.
        int[] triangles = new int[6];     // 1つの四角メッシュには2つ三角メッシュが必要. 三角メッシュには3つの頂点インデックスが必要.

        // ----- 頂点座標の割り当て -----
        vertices[0] = leftVertexOfFront;
        vertices[1] = rightVertexOfFront;
        vertices[2] = rightVertexOfBack;
        vertices[3] = leftVertexOfBack;

        // ----- 頂点インデックスの割り当て -----
        //この頂点順番でやるよって感じ
        triangles[0] = 2;
        triangles[1] = 1;
        triangles[2] = 0;
        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
    //描いたらこのスクリプトを無効にする。コピーした時にMeshがくずれてしまうため
    void thisEnabledFalse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            this.enabled = false;
        }
    }

}


