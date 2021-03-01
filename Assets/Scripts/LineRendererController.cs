using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class LineRendererController : MonoBehaviour
{
    /// <summary>
    /// マウスドラッグのスピード
    /// </summary>
    [SerializeField]
    protected float dragSpeed = 0.5f;
 
    /// <summary>
    /// マウスドラッグ中のフラグ
    /// </summary>
    protected bool isDrawing = false;

    protected bool isEnd = false;
 
    /// <summary>
    /// 描画用のラインレンダラー
    /// </summary>
    private LineRenderer currentLineRenderer;
 
    /// <summary>
    /// 描画する線の位置リスト
    /// </summary>
    private List<Vector3> rendererPositions = new List<Vector3>();
    
    private List<Vector3> meshPositions = new List<Vector3>();
 
    /// <summary>
    /// 基準とするゲームオブジェクト
    /// </summary>
    [SerializeField]
    protected GameObject originObject;
 
    /// <summary>
    /// 基準位置から現在ドラッグしている位置までのオフセット
    /// </summary>
    private Vector2 currentOffset = Vector2.zero;
 
    /// <summary>
    /// 描画を行うマウスのボタン
    /// </summary>
    private const int DRAW_BUTTON = 0;

    private Vector3 startPos;
    
    public LineRenderer line;

    public Material createMaterial;
    void Start()
    {
        currentLineRenderer = gameObject.GetComponent<LineRenderer>();
        currentLineRenderer.startWidth = 0.1f;
        currentLineRenderer.endWidth = 0.1f;
        startPos = originObject.transform.position;
    }
 
    void Update()
    {
        if (Input.GetMouseButtonDown(DRAW_BUTTON)) {
            isDrawing = true;
            rendererPositions.Clear();
        }
        if (Input.GetMouseButtonUp(DRAW_BUTTON)) {
            if (isDrawing)
            {
                isDrawing = false;
                isEnd = true;
            }
        }
 
        // マウス移動時の線を描画
        if (isDrawing) {
            // マウスドラッグに応じたオフセットの取得
            float currentOffset_x = currentOffset.x + Input.GetAxis("Mouse X") * dragSpeed;
            float currentOffset_y = currentOffset.y + Input.GetAxis("Mouse Y") * dragSpeed;
            currentOffset = new Vector2(currentOffset_x, currentOffset_y);
 
            // 描画先の座標を取得
            Vector3 position = new Vector3(currentOffset_x, currentOffset_y, 0) + originObject.transform.position;
 
            // ラインレンダラーに座標を設定し線を描画
            if (!rendererPositions.Contains(position)) {
                rendererPositions.Add(position);
                currentLineRenderer.positionCount = rendererPositions.Count;
                currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, position);
                startPos = position;
            }
        }
        else
        {
            if (isEnd)
            {
                CreateMeshes(currentLineRenderer);
                isEnd = false;
            }
        }
    }
    
    // Use this for initialization
    public void CreateMeshes(LineRenderer lines)
    {
        meshPositions.Clear();
        line = lines;
        GameObject caret = null;
        caret = new GameObject("Lines");
 
        Vector3 left, right; // A position to the left of the current line
 
        // For all but the last point
        for (var i = 0; i < line.positionCount - 1; i++)
        {
            caret.transform.position = line.GetPosition(i);
            caret.transform.LookAt(line.GetPosition(i + 1));
            right = caret.transform.position + transform.right * line.startWidth / 2;
            left = caret.transform.position - transform.right * line.startWidth / 2;
            meshPositions.Add(left);
            meshPositions.Add(right);
        }
 
        // Last point looks backwards and reverses
        caret.transform.position = line.GetPosition(line.positionCount - 1);
        caret.transform.LookAt(line.GetPosition(line.positionCount - 2));
        right = caret.transform.position + transform.right * line.startWidth / 2;
        left = caret.transform.position - transform.right * line.startWidth / 2;
        meshPositions.Add(left);
        meshPositions.Add(right);
        Destroy(caret);
        MeshFilter meshFilter = caret.AddComponent(typeof(MeshFilter)) as MeshFilter;
        
        Mesh mesh = DrawMesh(meshFilter);
        MeshRenderer meshRenderer = caret.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        meshRenderer.material = createMaterial;
        
        meshFilter.sharedMesh = mesh;
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            BoxCollider boxCollider = caret.AddComponent(typeof(BoxCollider)) as BoxCollider;
            boxCollider.center = mesh.vertices[i];
            boxCollider.size = new Vector3(0.01f,0.01f,1f);
        }

        Rigidbody rb = caret.AddComponent(typeof(Rigidbody)) as Rigidbody;
        rb.mass = 0.5f;

        GameObject createObj = Instantiate(caret);
        createObj.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    

    private Mesh DrawMesh(MeshFilter meshFilter)
    {
        Vector3[] verticies = new Vector3[rendererPositions.Count];

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i] = rendererPositions[i];
        }
 
        int[] triangles = new int[((rendererPositions.Count / 2) - 1) * 6];

        
        //Works on linear patterns tn = bn+c
        int position = 6;
        for (int i = 0; i < (triangles.Length / 6); i++)
        {
            triangles[i * position] = 2 * i;
            triangles[i * position + 3] = 2 * i;
 
            triangles[i * position + 1] = 2 * i + 3;
            triangles[i * position + 4] = (2 * i + 3) - 1;
 
            triangles[i * position + 2] = 2 * i + 1;
            triangles[i * position + 5] = (2 * i + 1) + 2;
        }
 
 
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}