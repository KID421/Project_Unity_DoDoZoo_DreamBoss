using UnityEngine;
using UnityEngine.EventSystems;

public class Lv3_DragObject : DragObjectBase
{
    /// <summary>
    /// 四個試管座標
    /// </summary>
    private Lv3_TestTube[] testTubePositions;

    protected override void Awake()
    {
        base.Awake();

        testTubePositions = FindObjectsOfType<Lv3_TestTube>();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        // base.OnEndDrag(eventData);

        CheckInWhichTestTube();
    }

    /// <summary>
    /// 檢查在哪個試管裡面
    /// </summary>
    private void CheckInWhichTestTube()
    {
        for (int i = 0; i < testTubePositions.Length; i++)
        {
            RectTransform rectCell = GetComponent<RectTransform>();
            RectTransform rectTestCube = testTubePositions[i].GetComponent<RectTransform>();

            if (Vector2.Distance(rectCell.anchoredPosition, rectTestCube.anchoredPosition) < 50)
            {
                StartCoroutine(testTubePositions[i].SetCell(this));
            }
        }
    }
}
