using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class CreateMeshByMouseBack : MonoBehaviour
{
    public CreateTopMesh createTopMesh;
    public CreateBottomMesh createBottomMesh;
    public CreateStartLid createStartLid;
    public CreateEndLid createEndLid;
    private List<Vector3> centerPoints;
    [SerializeField] float appendDistance = 0.5f;
    private float appendSqrDistance;

    //構造体 自分で型と要素数を決められる、要素ごとの型が違う配列。つまり違種類の物をまとめていれれる大きな箱
    private struct section
    {
        public Vector3 direction;   // 方向ベクトル.
        public Vector3 left;        // セクションの左端.
        public Vector3 right;       // セクションの右側.
    }
    private section[] necessaryInfo;
    [SerializeField] float width = 1;
    public Vector3[] vertices;

    public int[] triangles;
    private MeshFilter meshFilter;
    private Mesh mesh;
    private bool onePlay;
    public float okuyuki;
    public string myNumber;

    void Awake()
    {
        appendSqrDistance = Mathf.Pow(appendDistance, 2);
    }
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh = new Mesh();
        mesh.name = "CubeMesh";
        createTopMesh = GameObject.FindGameObjectWithTag("TopMesh").GetComponent<CreateTopMesh>();
        createBottomMesh = GameObject.FindGameObjectWithTag("BottomMesh").GetComponent<CreateBottomMesh>();
        createStartLid = GameObject.FindGameObjectWithTag("StartLid").GetComponent<CreateStartLid>();
        createEndLid = GameObject.FindGameObjectWithTag("EndLid").GetComponent<CreateEndLid>();
        onePlay = true;
    }

    void OnValidate()
    {
        appendSqrDistance = Mathf.Pow(appendDistance, 2);
    }

    void Update()
    {
        //手前のMeshを作る
        setCenterPoints();
        setDirectionAndSidePoint();
        createMesh();
        thisEnabledFalse();
    }

    /// マウス入力によってcenterPointsを設定する.
    void setCenterPoints()
    {

        // マウス押下中のみ処理を行う.
        if (Input.GetMouseButton(0))
        {

            // マウスの位置をスクリーン座標からワールド座標に変換.
            var screenMousePos = Input.mousePosition;
            screenMousePos.z = Camera.main.transform.position.z * -1f;
            Vector3 currentMousePoint = Camera.main.ScreenToWorldPoint(screenMousePos);

            //centerPointsがまだない場合はマウスの座標をセットする
            if (centerPoints == null)
            {
                centerPoints = new List<Vector3>();
                centerPoints.Add(new Vector3(currentMousePoint.x, currentMousePoint.y, currentMousePoint.z + okuyuki));
            }

            //今のマウスの現在地と前のcenterPointsとの距離を測る。
            var distance = (new Vector2(currentMousePoint.x, currentMousePoint.y) - new Vector2(centerPoints[centerPoints.Count - 1].x, centerPoints[centerPoints.Count - 1].y));

            //このif文は距離によって無駄な描画を減らすおまじない的なやつ
            if (distance.sqrMagnitude >= appendSqrDistance)
            {
                centerPoints.Add(new Vector3(currentMousePoint.x, currentMousePoint.y, currentMousePoint.z + okuyuki));
            }

        }

    }


    //前後のポイントかポイント自身を用いて傾く方向ベクトルの計算を行う
    // 次に、方向ベクトルに垂直に幅を足して両端を求める。
    void setDirectionAndSidePoint()
    {

        // 2つ以上セクションを用意できない状態の場合処理を抜ける.
        if (centerPoints == null || centerPoints.Count <= 1) return;

        necessaryInfo = new section[centerPoints.Count];
        //createTopMeshのleftPointBackも宣言する
        createTopMesh.leftVertexOfBack = new Vector3[centerPoints.Count];
        createBottomMesh.rightVertexOfBack = new Vector3[centerPoints.Count];

        for (int i = 0; i < centerPoints.Count; i++)
        {
            // ----- 方向ベクトルの計算 -----
            if (i == 0)
            {
                // 始点の場合.
                necessaryInfo[i].direction = centerPoints[i + 1] - centerPoints[i];
            }
            else if (i == centerPoints.Count - 1)
            {
                // 終点の場合.
                necessaryInfo[i].direction = centerPoints[i] - centerPoints[i - 1];
            }
            else
            {
                // 途中の場合.
                necessaryInfo[i].direction = centerPoints[i + 1] - centerPoints[i - 1];
            }

            //Normalizeはmagnitude を 1 としたベクトルを作成します
            //正規化されたときベクトルは同じ方向は維持したままで長さが 1.0 のものが作成されます。
            necessaryInfo[i].direction.Normalize();

            // ----- 方向ベクトルに直交するベクトルの計算 -----
            Vector2 side = Quaternion.AngleAxis(90f, -Vector3.forward) * necessaryInfo[i].direction;
            side.Normalize();




            //z方向にずらして代入
            necessaryInfo[i].left = new Vector2(centerPoints[i].x, centerPoints[i].y) - side * width / 2f;
            necessaryInfo[i].right = new Vector2(centerPoints[i].x, centerPoints[i].y) + side * width / 2f;
            necessaryInfo[i].left.z = centerPoints[i].z;
            necessaryInfo[i].right.z = centerPoints[i].z;
            //createTopMeshにも割り当てる。
            createTopMesh.leftVertexOfBack[i] = necessaryInfo[i].left;
            createBottomMesh.rightVertexOfBack[i] = necessaryInfo[i].right;
            //createStartMeshにも二つの座標を割り当てるただし最初のいちどのみ
            if (onePlay == true)
            {
                createStartLid.leftVertexOfBack = necessaryInfo[i].left;
                createStartLid.rightVertexOfBack = necessaryInfo[i].right;
                createStartLid.backVerticesOK = true;
                onePlay = false;
            }

            //CreateEndMeshにクリックを離したときあてはめる。
            if (Input.GetMouseButtonUp(0))
            {
                createEndLid.leftVertexOfBack = necessaryInfo[i].left;
                createEndLid.rightVertexOfBack = necessaryInfo[i].right;
                createEndLid.backVerticesOK = true;
            }
        }
    }


    //Gizmo（頂点の点）を表示する
    void OnDrawGizmos()
    {
        if (necessaryInfo == null) return;

        Gizmos.color = Color.black;
        for (int i = 0; i < necessaryInfo.Length; i++)
        {
            Gizmos.DrawSphere(centerPoints[i], 0.1f);
        }

        Gizmos.color = Color.blue;
        for (int i = 0; i < necessaryInfo.Length; i++)
        {
            Gizmos.DrawSphere(necessaryInfo[i].left, 0.1f);
            Gizmos.DrawSphere(necessaryInfo[i].right, 0.1f);
        }
    }

    void createMesh()
    {
        if (centerPoints == null || centerPoints.Count <= 1) return;



        int meshCount = centerPoints.Count - 1;   // 四角メッシュ生成数はセクション - 1.

        vertices = new Vector3[(meshCount) * 4];  // 四角なので頂点数は1つのメッシュに付き4つ.
        triangles = new int[(meshCount) * 2 * 3];     // 1つの四角メッシュには2つ三角メッシュが必要. 三角メッシュには3つの頂点インデックスが必要.

        // ----- 頂点座標の割り当て -----
        for (int i = 0; i < meshCount; i++)
        {
            vertices[i * 4 + 0] = necessaryInfo[i].left;
            vertices[i * 4 + 1] = necessaryInfo[i].right;
            vertices[i * 4 + 2] = necessaryInfo[i + 1].left;
            vertices[i * 4 + 3] = necessaryInfo[i + 1].right;
        }

        // ----- 頂点インデックスの割り当て -----
        int positionIndex = 0;

        for (int i = 0; i < meshCount; i++)
        {
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
    //描いたらこのスクリプトを無効にする。コピーした時にMeshがくずれてしまうため
    void thisEnabledFalse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            this.enabled = false;
        }
    }
}