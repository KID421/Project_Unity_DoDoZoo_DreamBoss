/* -- KID 2020.12.22 --
 * 參考資料：https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
 * 
 */

using UnityEngine;

[ExecuteInEditMode]
public class LineRendererBezier : MonoBehaviour
{
    [Header("數量")]
    public int count;

    private LineRenderer lineRenderer;

    public Transform p0;
    public Transform p1;
    public Transform p2;
    public Transform p3;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        DrawQuadraticBezierCurve(p0.position, p1.position, p2.position, p3.position);
    }

    /// <summary>
    /// 繪製二次的貝茲曲線
    /// </summary>
    /// <param name="point0"></param>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <param name="point3"></param>
    private void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        lineRenderer.positionCount = count;

        float t = 0f;

        Vector3 b = new Vector3(0, 0, 0);

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            b = (1 - t) * (1 - t) * (1 - t) * point0 + 3 * (1 - t) * (1 - t) * t * point1 + 3 * (1 - t) * t * t * point2 + t * t * t * point3;
            lineRenderer.SetPosition(i, b);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }
}
