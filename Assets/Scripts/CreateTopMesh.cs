using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//オブジェクトの座標000にしとく
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class CreateTopMesh : MonoBehaviour
{

    public Vector3[] leftVertexOfFront;
    public Vector3[] leftVertexOfBack;
    private MeshFilter meshFilter;
    private Mesh mesh;



    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh = new Mesh();
        mesh.name = "TopCubeMesh";
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
        //leftVertexOfFrontに二つ以上頂点の座標が入っている場合
        if (leftVertexOfFront.Length > 1)
        {

            int meshCount = leftVertexOfFront.Length - 1;                   // 四角メッシュ生成数はセクション - 1.

            Vector3[] vertices = new Vector3[(meshCount) * 4];  // 四角なので頂点数は1つのメッシュに付き4つ.
            int[] triangles = new int[(meshCount) * 2 * 3];     // 1つの四角メッシュには2つ三角メッシュが必要. 三角メッシュには3つの頂点インデックスが必要.

            // ----- 頂点座標の割り当て -----
            for (int i = 0; i < meshCount; i++)
            {
                vertices[i * 4 + 0] = leftVertexOfFront[i];
                vertices[i * 4 + 1] = leftVertexOfBack[i];
                vertices[i * 4 + 2] = leftVertexOfFront[i + 1];
                vertices[i * 4 + 3] = leftVertexOfBack[i + 1];
            }

            // ----- 頂点インデックスの割り当て -----
            int positionIndex = 0;

            for (int i = 0; i < meshCount; i++)
            {
                //この頂点順番でやるよって感じ
                triangles[positionIndex++] = (i * 4) + 2;
                triangles[positionIndex++] = (i * 4) + 0;
                triangles[positionIndex++] = (i * 4) + 1;

                triangles[positionIndex++] = (i * 4) + 1;
                triangles[positionIndex++] = (i * 4) + 3;
                triangles[positionIndex++] = (i * 4) + 2;
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

