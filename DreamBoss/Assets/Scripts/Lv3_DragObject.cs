using UnityEngine;
using UnityEngine.EventSystems;

public class Lv3_DragObject : DragObjectBase
{
    /// <summary>
    /// 四個試管座標
    /// </summary>
    private Lv3_TestTube[] testTubePositions;
    /// <summary>
    /// 是否進入試管中
    /// </summary>
    private bool inTest;
    /// <summary>
    /// 細胞的變形元件
    /// </summary>
    RectTransform rectCell;

    protected override void Awake()
    {
        base.Awake();

        rectCell = GetComponent<RectTransform>();
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
            RectTransform rectTestCube = testTubePositions[i].GetComponent<RectTransform>();

            // print("第幾隻試管：" + i);
            // print("與試管的距離：" + Vector2.Distance(rectCell.anchoredPosition, rectTestCube.anchoredPosition));

            if (!inTest && Vector2.Distance(rectCell.anchoredPosition, rectTestCube.anchoredPosition) < 50)
            {
                inTest = true;
                StartCoroutine(testTubePositions[i].SetCell(this));
            }
        }

        if (!inTest) rectCell.anchoredPosition = new Vector2(165, 250);          // 回到原點
    }
}
