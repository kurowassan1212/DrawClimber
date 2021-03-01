using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FreeHand : MonoBehaviour
{
    public GameObject lineRendererOb;

    public Material lineMaterial;


    public Color lineColor;
    public LineRenderer lineRenderer;


    [Range(0, 10)] public float lineWidth;


    void Awake()
    {

    }

    void Update()
    {

        // ボタンが押された時に線オブジェクトの追加を行う
        if (Input.GetMouseButtonDown(0))
        {
            if (lineRendererOb != null)
            {
                Destroy(lineRendererOb);
            }
            this.AddLineObject();
        }

        // ボタンが押されている時、LineRendererに位置データの設定を指定していく
        if (Input.GetMouseButton(0))
        {
            this.AddPositionDataToLineRendererList();
        }
    }

    /// 線オブジェクトの追加を行うメソッド
    private void AddLineObject()
    {

        // 追加するオブジェクトをインスタンス
        lineRendererOb = new GameObject();

        // オブジェクトにLineRendererを取り付ける
        lineRendererOb.AddComponent<LineRenderer>();
        lineRenderer = lineRendererOb.GetComponent<LineRenderer>();
        // 線と線をつなぐ点の数を0に初期化
        lineRenderer.positionCount = 0;

        // マテリアルを初期化
        lineRenderer.material = this.lineMaterial;

        // 線の色を初期化
        lineRenderer.material.color = this.lineColor;

        // 線の太さを初期化
        lineRenderer.startWidth = this.lineWidth;
        lineRenderer.endWidth = this.lineWidth;
    }

    /// 描く線のコンポーネントに位置情報を登録していく
    private void AddPositionDataToLineRendererList()
    {

        // 座標の変換を行いマウス位置を取得
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1.0f);
        var mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // 線と線をつなぐ点の数を更新
        lineRenderer.positionCount += 1;

        // 描く線のコンポーネントリストを更新
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePosition);
    }
}