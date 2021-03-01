using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class CreateMesh : MonoBehaviour
{
    public List<Vector3> centerPoints;//DrawLineからボタン離した時に代入される。
    public List<Vector3> frontTopVertices;
    public List<Vector3> frontBottomVertices;
    public List<Vector3> backTopVertices;
    public List<Vector3> backBottomVertices;

    public List<Vector3> verticesList;
    public List<int> trianglesList;

    private Vector3 direction;

    public float width, depth, createTime;

    private bool createVer;
    public Vector3[] verticeS;
    public int[] triangleS;
    private MeshFilter meshFilter;
    private Mesh mesh;

    public MeshFilter[] copyMeshFilters;
    private Mesh[] copyMeshes;

    private MeshCollider meshCollider;

    public GameObject meshParentPrefab, meshParentOb, playerOb;
    public GameObject[] copyOb;
    public PhysicMaterial material;

    public bool createMeshGradually;

    public int loopCount, trianglesNum;






    void Start()
    {
        centerPoints = new List<Vector3>();
        frontTopVertices = new List<Vector3>();
        frontBottomVertices = new List<Vector3>();
        backTopVertices = new List<Vector3>();
        backBottomVertices = new List<Vector3>();
        createVer = false;

        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh = new Mesh();
        /*  mesh.name = "DrawMesh"; */

        meshCollider = GetComponent<MeshCollider>();

        if (width == 0)
        {
            width = 0.5f;
        }
        if (depth == 0)
        {
            depth = 0.3f;
        }
        playerOb = GameObject.FindGameObjectWithTag("CylinderPlayer");
        //meshの位置がずれるため
        transform.position = new Vector3(0, 0, 0);
        createMeshGradually = false;
        loopCount = 0;
        trianglesNum = 0;
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();
        copyOb = new GameObject[3];
    }

    void Update()
    {
        GraduallyCreateMesh();
    }

    public void Create()
    {
        CreateVertices();
        MoveMesh();
        createMeshGradually = true;
    }

    public void CreateVertices()
    {
        //Drawlineによって代入されたcentorpointより進行方向から垂直に距離をとりfrontTopVerticesとfrontBottomVerticesを用意する

        // 2つ以上centerPointsを用意できない状態の場合処理を抜ける.
        if (centerPoints == null || centerPoints.Count <= 1) return;

        // ----- 方向ベクトルの計算 -----
        for (int i = 0; i < centerPoints.Count; i++)
        {
            // ----- 方向ベクトルの計算 -----
            if (i == 0)
            {
                // 始点の場合.
                direction = centerPoints[i + 1] - centerPoints[i];
            }
            else if (i == centerPoints.Count - 1)
            {
                // 終点の場合.
                direction = centerPoints[i] - centerPoints[i - 1];
            }
            else
            {
                // 途中の場合.
                direction = centerPoints[i + 1] - centerPoints[i - 1];
            }

            //Normalizeはmagnitude を 1 としたベクトルを作成します
            //正規化されたときベクトルは同じ方向は維持したままで長さが 1.0 のものが作成されます。
            direction.Normalize();

            // ----- 方向ベクトルに垂直になるベクトルの計算 -----
            Vector2 side = Quaternion.AngleAxis(90f, -Vector3.forward) * direction;
            side.Normalize();

            //手前の頂点
            Vector3 vertexPos = new Vector2(centerPoints[i].x, centerPoints[i].y) - side * width / 2f;
            vertexPos = new Vector3(vertexPos.x, vertexPos.y, 0);
            frontTopVertices.Add(vertexPos);
            Vector3 vertexPos2 = new Vector2(centerPoints[i].x, centerPoints[i].y) + side * width / 2f;
            vertexPos2 = new Vector3(vertexPos2.x, vertexPos2.y, 0);
            frontBottomVertices.Add(vertexPos2);

            //奥の頂点
            Vector3 vertexPos3 = new Vector2(centerPoints[i].x, centerPoints[i].y) - side * width / 2f;
            vertexPos3 = new Vector3(vertexPos3.x, vertexPos3.y, depth);
            backTopVertices.Add(vertexPos3);
            Vector3 vertexPos4 = new Vector2(centerPoints[i].x, centerPoints[i].y) + side * width / 2f;
            vertexPos4 = new Vector3(vertexPos4.x, vertexPos4.y, depth);
            backBottomVertices.Add(vertexPos4);
        }

        createVer = true;

    }
    private void GraduallyCreateMesh()
    {
        createTime += Time.deltaTime;
        if (createMeshGradually == false && createTime < 35f) return;
        //centerPoint-1回分繰り返す
        if (loopCount < centerPoints.Count)
        {
            verticesList.Clear();
            trianglesList.Clear();

            //最初の蓋の面を作る
            if (loopCount == 0)
            {
                verticeS = new Vector3[0];
                triangleS = new int[0];

                //頂点座標の割り当て
                verticesList.Add(frontTopVertices[loopCount]);
                verticesList.Add(frontBottomVertices[loopCount]);
                verticesList.Add(backTopVertices[loopCount]);
                verticesList.Add(backBottomVertices[loopCount]);
                verticeS = verticesList.ToArray();

                //頂点の順番の番号の割り当て
                trianglesList.Add(2);
                trianglesList.Add(0);
                trianglesList.Add(1);
                trianglesList.Add(2);
                trianglesList.Add(1);
                trianglesList.Add(3);
                triangleS = trianglesList.ToArray();
            }

            //側面の4つの面の作成

            //頂点座標の追加
            verticesList.AddRange(verticeS);
            verticesList.Add(frontTopVertices[loopCount]);
            verticesList.Add(frontBottomVertices[loopCount]);
            verticesList.Add(backTopVertices[loopCount]);
            verticesList.Add(backBottomVertices[loopCount]);
            verticeS = verticesList.ToArray();

            //頂点番号の追加
            trianglesList.AddRange(triangleS);

            //front
            trianglesList.Add((loopCount * 4) + 1);
            trianglesList.Add((loopCount * 4) + 0);
            trianglesList.Add((loopCount * 4) + 4);
            trianglesList.Add((loopCount * 4) + 1);
            trianglesList.Add((loopCount * 4) + 4);
            trianglesList.Add((loopCount * 4) + 5);

            //top
            trianglesList.Add((loopCount * 4) + 0);
            trianglesList.Add((loopCount * 4) + 2);
            trianglesList.Add((loopCount * 4) + 4);
            trianglesList.Add((loopCount * 4) + 2);
            trianglesList.Add((loopCount * 4) + 6);
            trianglesList.Add((loopCount * 4) + 4);

            //back
            trianglesList.Add((loopCount * 4) + 2);
            trianglesList.Add((loopCount * 4) + 3);
            trianglesList.Add((loopCount * 4) + 6);
            trianglesList.Add((loopCount * 4) + 3);
            trianglesList.Add((loopCount * 4) + 7);
            trianglesList.Add((loopCount * 4) + 6);

            //bottom
            trianglesList.Add((loopCount * 4) + 1);
            trianglesList.Add((loopCount * 4) + 5);
            trianglesList.Add((loopCount * 4) + 3);
            trianglesList.Add((loopCount * 4) + 5);
            trianglesList.Add((loopCount * 4) + 7);
            trianglesList.Add((loopCount * 4) + 3);

            //最後の面作成
            if (loopCount == centerPoints.Count - 1)
            {
                trianglesList.Add((centerPoints.Count * 4) + 1);
                trianglesList.Add((centerPoints.Count * 4) + 0);
                trianglesList.Add((centerPoints.Count * 4) + 2);
                trianglesList.Add((centerPoints.Count * 4) + 1);
                trianglesList.Add((centerPoints.Count * 4) + 2);
                trianglesList.Add((centerPoints.Count * 4) + 3);
            }

            triangleS = trianglesList.ToArray();

            mesh.vertices = verticeS;
            mesh.triangles = triangleS;
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;
            loopCount++;
            createTime = 0;

            //既存のコピーを破壊して、createmeshを無効にしたコピーを生成し続ける。
            for (int i = 0; i < 3; i++)
            {
                if (copyOb[i] != null)
                {
                    Destroy(copyOb[i]);
                }
                copyOb[i] = Instantiate(this.gameObject.transform.parent.gameObject);
                copyOb[i].transform.GetChild(0).GetComponent<CreateMesh>().enabled = false;
                copyOb[i].transform.parent = playerOb.transform;
                switch (i)
                {
                    case 0://手前視認不可
                        copyOb[i].transform.localScale = new Vector3(1f, 1f, 1f);
                        copyOb[i].transform.localPosition = new Vector3(0, -0.82f, 0f);
                        copyOb[i].transform.localEulerAngles = new Vector3(-90, 180, 0);
                        copyOb[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        break;
                    case 1://奥視認不可
                        copyOb[i].transform.localScale = new Vector3(1f, 1f, 1f);
                        copyOb[i].transform.localPosition = new Vector3(0, 0.73f, 0f);
                        copyOb[i].transform.localEulerAngles = new Vector3(-90, 0, 0);
                        copyOb[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        break;
                    case 2://奥視認可能
                        copyOb[i].transform.localScale = new Vector3(1f, 1f, 1f);
                        copyOb[i].transform.localPosition = new Vector3(0, 0.73f, 0f);
                        copyOb[i].transform.localEulerAngles = new Vector3(-90, 180, 0);
                        break;
                }
            }
        }
    }


    private void MoveMesh()
    {
        //親オブジェクトを最初のcenterpointの位置に生成してその子オブジェクトになる
        meshParentOb = Instantiate(meshParentPrefab, centerPoints[0], Quaternion.identity);
        transform.parent = meshParentOb.transform;
        meshParentOb.transform.parent = playerOb.transform;
        meshParentOb.transform.localScale = new Vector3(1f, 1f, 1f);
        meshParentOb.transform.localPosition = new Vector3(0, -0.82f, 0f);
        meshParentOb.transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

    /*     private void CopyAndMove()
        {
            copyMesh = new GameObject[3];
            copyMeshFilters = new MeshFilter[3];
            copyMeshes = new Mesh[3];
            for (int i = 0; i < 3; i++)
            {
                copyMesh[i] = Instantiate(this.gameObject.transform.parent.gameObject);
                copyMesh[i].transform.parent = playerOb.transform;
                copyMesh[i].transform.GetChild(0).GetComponent<CreateMesh>().enabled = false;
                copyMeshFilters[i] = copyMesh[i].transform.GetChild(0).gameObject.GetComponent<MeshFilter>();
                copyMeshes[i] = copyMeshFilters[i].mesh;

                switch (i)
                {
                    case 0://手前視認不可
                       copyOb[0].transform.localScale = new Vector3(1f, 1f, 1f);
                       copyOb[0].transform.localPosition = new Vector3(0, -0.82f, 0f);
                       copyOb[0].transform.localEulerAngles = new Vector3(-90, 180, 0);

                        break;
                    case 1://奥視認不可
                        copyOb[0].transform.localScale = new Vector3(1f, 1f, 1f);
                        copyOb[0].transform.localPosition = new Vector3(0, 0.73f, 0f);
                       copyOb[0].transform.localEulerAngles = new Vector3(-90, 0, 0);
                        break;
                    case 2://奥視認可能
                        copyOb[0].transform.localScale = new Vector3(1f, 1f, 1f);
                       copyOb[0].transform.localPosition = new Vector3(0, 0.73f, 0f);
                       copyOb[0].transform.localEulerAngles = new Vector3(-90, 180, 0);
                        break;
                }
            }
        } */

    private void OnDrawGizmos()
    {
        if (createVer == true)
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < centerPoints.Count; i++)
            {
                Gizmos.DrawSphere(centerPoints[i], 0.1f);
            }

            Gizmos.color = Color.blue;
            for (int i = 0; i < centerPoints.Count; i++)
            {
                Gizmos.DrawSphere(frontTopVertices[i], 0.1f);
                Gizmos.DrawSphere(frontBottomVertices[i], 0.1f);
                Gizmos.DrawSphere(backTopVertices[i], 0.1f);
                Gizmos.DrawSphere(backBottomVertices[i], 0.1f);
            }
        }
    }

}

