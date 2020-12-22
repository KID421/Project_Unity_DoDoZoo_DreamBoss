using UnityEngine;

public class Lv1_TeacherVersion1 : LevelBase
{
    [Header("繪製判斷距離"), Range(0.5f, 5)]
    public float drawDistance = 1.3f;

    /// <summary>
    /// 題目線條：放所有製作完成的題目線條
    /// </summary>
    public LineRenderer lineQuestion;

    /// <summary>
    /// 線條渲染：玩家繪製產生的線條
    /// </summary>
    private LineRenderer line;
    /// <summary>
    /// 線條編號：玩家繪製產生的線條編號
    /// </summary>
    private int indexLine;

    protected override void Awake()
    {
        base.Awake();

        InitializeLine();
    }

    /// <summary>
    /// 初始化線條
    /// </summary>
    private void InitializeLine()
    {
        line = GetComponent<LineRenderer>();

        // 取得題目的第一個點
        Vector3 posLineQuestion = lineQuestion.GetPosition(indexLine);
        line.positionCount = 1;
        line.SetPosition(indexLine, posLineQuestion);
        // 編號遞增
        indexLine++;
    }

    protected override void Update()
    {
        base.Update();

        CheckMousePoisition();
    }

    /// <summary>
    /// 檢查滑鼠觸碰點擊位置
    /// 如果很靠近題目的線條位置就產生線條
    /// </summary>
    private void CheckMousePoisition()
    {
        // 如果線條編號超過 99 就跳出，※ 後續要改為根據每個題目的數量
        if (indexLine > 99) return;

        // 如果按住左鍵
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // 取得滑鼠位置
            Vector3 posMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 posMouseWorld = Camera.main.ScreenToWorldPoint(posMouse);
            // 取得題目位置
            Vector3 posLineQuestion = lineQuestion.GetPosition(indexLine);
            // 判斷 滑鼠 與 題目 距離
            float dis = Vector3.Distance(posMouseWorld, posLineQuestion);

            //print("題目：" + posLineQuestion);
            //print("滑鼠：" + posMouseWorld);
            //print("距離：" + dis);
            
            // 如果距離 小於 判斷距離 就畫出線條
            if (dis < drawDistance)
            {
                line.positionCount = (indexLine + 1);
                line.SetPosition(indexLine, posLineQuestion);
                indexLine++;
            }
        }
    }
}
