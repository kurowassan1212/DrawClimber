using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private int positionCount;
    private Camera mainCamera;
    public CreateMesh createMesh;
    private List<Vector3> centerPoints;
    private float appendSqrDistance;
    [SerializeField] float appendDistance = 0.5f;

    public GameObject createMeshPrefab, createMeshOb;

    void Start()
    {
        appendSqrDistance = Mathf.Pow(appendDistance, 2);
        lineRenderer = GetComponent<LineRenderer>();
        // ラインの座標指定を、このラインオブジェクトのローカル座標系を基準にするよう設定を変更
        // この状態でラインオブジェクトを移動・回転させると、描かれたラインもワールド空間に取り残されることなく、一緒に移動・回転
        lineRenderer.useWorldSpace = false;
        positionCount = 0;
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // このラインオブジェクトを、位置はカメラ前方10m、回転はカメラと同じになるようキープさせる
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 10;
        transform.rotation = mainCamera.transform.rotation;

        //ボタンを押すと、CreateMeshオブジェクトを生成する。
        if (Input.GetMouseButtonDown(0))
        {
            if (createMeshOb != null)
            {
                Destroy(createMeshOb.transform.parent.gameObject);
                Destroy(createMesh.copyOb[0]);
                Destroy(createMesh.copyOb[1]);
                Destroy(createMesh.copyOb[2]);
                centerPoints.Clear();
                centerPoints = null;
                lineRenderer.enabled = true;
            }
            createMeshOb = Instantiate(createMeshPrefab);
            createMesh = createMeshOb.GetComponent<CreateMesh>();
        }

        if (Input.GetMouseButton(0))
        {
            // 座標指定の設定をローカル座標系にしたため、与える座標にも手を加える
            Vector3 currentMousePoint = Input.mousePosition;
            currentMousePoint.z = 10.0f;

            // マウススクリーン座標をワールド座標に直す
            currentMousePoint = mainCamera.ScreenToWorldPoint(currentMousePoint);

            // さらにそれをローカル座標に直す。
            currentMousePoint = transform.InverseTransformPoint(currentMousePoint);

            // 得られたローカル座標をラインレンダラーに追加する
            positionCount++;
            lineRenderer.positionCount = positionCount;
            lineRenderer.SetPosition(positionCount - 1, currentMousePoint);

            //座標を渡す
            //centerPointsがまだない場合はマウスの座標をセットする
            if (centerPoints == null)
            {
                centerPoints = new List<Vector3>();
                centerPoints.Add(new Vector3(currentMousePoint.x, currentMousePoint.y, 0));
            }

            //今のマウスの現在地と前のcenterPointsとの距離を測る。
            /* var distance = (new Vector2(currentMousePoint.x, currentMousePoint.y) - new Vector2(centerPoints[centerPoints.Count - 1].x, centerPoints[centerPoints.Count - 1].y)); */

            //距離が一定の距離離れている場合z座標を0にして渡す。
            //このif文は距離によって無駄な描画を減らすおまじない的なやつ
            /* if (distance.sqrMagnitude >= appendSqrDistance)
            { */
            centerPoints.Add(new Vector3(currentMousePoint.x, currentMousePoint.y, 0));
            /* } */

        }

        if ((Input.GetMouseButtonUp(0)))
        {
            //createmeshにcenterpointsを渡す
            createMesh.centerPoints = this.centerPoints;
            createMesh.Create();
            //線をリセットする
            positionCount = 0;
            lineRenderer.enabled = false;
        }

    }

}

