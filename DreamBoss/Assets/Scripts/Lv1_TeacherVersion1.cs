using UnityEngine;

public class Lv1_TeacherVersion1 : LevelBase
{
    [Header("繪製判斷距離"), Range(0.5f, 5)]
    public float drawDistance = 1.3f;

    /// <summary>
    /// 題目線條：放所有製作完成的題目線條
    /// </summary>
    public TeacherQuestion[] lineQuestion;
    /// <summary>
    /// 線條渲染：玩家繪製產生的線條
    /// </summary>
    private LineRenderer[] lines = new LineRenderer[2];
    /// <summary>
    /// 線條編號：玩家繪製產生的線條編號
    /// </summary>
    private int indexLine;
    /// <summary>
    /// 步驟的編號
    /// </summary>
    private int indexStep = 0;
    /// <summary>
    /// 目前的線條編號 - 每一線段從 0 - 結束用
    /// </summary>
    private int indexCurrent;
    /// <summary>
    /// 是否結束
    /// </summary>
    private bool finish;

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
        for (int i = 0; i < 2; i++) lines[i] = GameObject.Find("線段 " + i).GetComponent<LineRenderer>();

        LineRenderer q = lineQuestion[0].lineSteps[0];

        // 取得題目的第一個點
        Vector3 posLineQuestion = q.GetPosition(indexLine);
        lines[0].positionCount = 1;
        lines[0].SetPosition(indexLine, posLineQuestion);
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
        // 已經完成
        if (finish) return;
        if (indexStep == lineQuestion[0].lineSteps.Length - 1 && indexCurrent == lineQuestion[0].lineSteps[indexStep].positionCount)
        {
            finish = true;
            StartCoroutine(Pass());
        }

        // 如果線條編號超過 99 就跳出，※ 後續要改為根據每個題目的數量
        //if (indexLine > lineQuestion[0].lineSteps[indexStep].positionCount) return;
        if (indexStep == 0 && indexLine == lineQuestion[0].lineSteps[indexStep].positionCount && indexStep < lineQuestion[0].lineSteps.Length)
        {
            indexStep++;
        }

        // 如果按住左鍵
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // 取得滑鼠位置
            Vector3 posMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 posMouseWorld = Camera.main.ScreenToWorldPoint(posMouse);

            // 取得題目位置
            LineRenderer q = lineQuestion[0].lineSteps[indexStep];

            // 減去前一個步驟的數量
            if (indexStep == 1) indexCurrent = indexLine - lineQuestion[0].lineSteps[indexStep - 1].positionCount;
            else if (indexCurrent < q.positionCount) indexCurrent = indexLine;

            if (indexCurrent == lineQuestion[0].lineSteps[indexStep].positionCount) return;                                 // 避免 編號 跑到 最後一個 導致錯誤
            Vector3 posLineQuestion = q.GetPosition(indexCurrent);                                                          // 取得題目的每個當前位置
            
            float dis = Vector3.Distance(posMouseWorld, posLineQuestion);                                                   // 判斷 滑鼠 與 題目 距離

            //print("題目：" + posLineQuestion);
            //print("滑鼠：" + posMouseWorld);
            //print("距離：" + dis);

            if (dis < drawDistance)                                                                                         // 如果距離 小於 判斷距離 就畫出線條
            {
                lines[indexStep].positionCount = (indexCurrent + 1);
                lines[indexStep].SetPosition(indexCurrent, posLineQuestion);
                indexLine++;
            }
        }
    }
}

/// <summary>
/// 教師題目
/// </summary>
[System.Serializable]
public struct TeacherQuestion
{
    [Header("線條步驟")]
    public LineRenderer[] lineSteps;
}