using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//オブジェクトの座標000にしとく
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class CreateBottomMesh : MonoBehaviour
{

    public Vector3[] rightVertexOfFront;
    public Vector3[] rightVertexOfBack;
    private MeshFilter meshFilter;
    private Mesh mesh;



    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh = new Mesh();
        mesh.name = "BottomCubeMesh";
    }

    // Update is called once per frame
    void LateUpdate()
    {
        createSideMesh();
        thisEnabledFalse();
    }
    //すでに
    void createSideMesh()
    {
        //rightVertexOfFrontに二つ以上頂点の座標が入っている場合
        if (rightVertexOfFront.Length > 1)
        {

            int meshCount = rightVertexOfFront.Length - 1;                   // 四角メッシュ生成数はセクション - 1.

            Vector3[] vertices = new Vector3[(meshCount) * 4];  // 四角なので頂点数は1つのメッシュに付き4つ.
            int[] triangles = new int[(meshCount) * 2 * 3];     // 1つの四角メッシュには2つ三角メッシュが必要. 三角メッシュには3つの頂点インデックスが必要.

            // ----- 頂点座標の割り当て -----
            for (int i = 0; i < meshCount; i++)
            {
                vertices[i * 4 + 0] = rightVertexOfFront[i];
                vertices[i * 4 + 1] = rightVertexOfBack[i];
                vertices[i * 4 + 2] = rightVertexOfFront[i + 1];
                vertices[i * 4 + 3] = rightVertexOfBack[i + 1];
            }

            // ----- 頂点インデックスの割り当て -----
            int positionIndex = 0;

            for (int i = 0; i < meshCount; i++)
            {
                //この頂点順番でやるよって感じ
                triangles[positionIndex++] = (i * 4) + 1;
                triangles[positionIndex++] = (i * 4) + 0;
                triangles[positionIndex++] = (i * 4) + 2;

                triangles[positionIndex++] = (i * 4) + 2;
                triangles[positionIndex++] = (i * 4) + 3;
                triangles[positionIndex++] = (i * 4) + 1;
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }
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
