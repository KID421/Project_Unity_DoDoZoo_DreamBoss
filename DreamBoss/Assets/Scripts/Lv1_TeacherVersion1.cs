using UnityEngine;
using UnityEngine.UI;

public class Lv1_TeacherVersion1 : LevelBase
{
    [Header("繪製判斷距離"), Range(0.5f, 5)]
    public float drawDistance = 1.3f;
    [Header("題目編號")]
    public int indexQuestion;
    [Header("題目：數字 0 - 10")]
    public Sprite[] sprQuestions;
    /// <summary>
    /// 題目線條：放所有製作完成的題目線條
    /// </summary>
    [Header("題目線條：有 Line Renderer 的物件")]
    public TeacherQuestion[] lineQuestion;
    [Header("題目物件，父物件，包含提示物件")]
    public GameObject[] lineObject;

    /// <summary>
    /// 題目圖片
    /// </summary>
    private Image imgQuestion;
    /// <summary>
    /// 線條渲染：玩家繪製產生的線條
    /// </summary>
    private LineRenderer[] lines = new LineRenderer[3];
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
    /// 隨奇蹄目
    /// </summary>
    private void InitializeLine()
    {
        // 隨機題目
        imgQuestion = GameObject.Find("題目圖片").GetComponent<Image>();
        indexQuestion = Random.Range(0, lineQuestion.Length);

        // 測試
        indexQuestion = 1;

        imgQuestion.sprite = sprQuestions[indexQuestion];
        // 顯示提示物件
        lineObject[indexQuestion].SetActive(true);

        for (int i = 0; i < 3; i++) lines[i] = GameObject.Find("線段 " + i).GetComponent<LineRenderer>();

        LineRenderer q = lineQuestion[indexQuestion].lineSteps[0];

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

        // 正確
        if (indexStep == lineQuestion[indexQuestion].lineSteps.Length - 1 && indexCurrent == lineQuestion[indexQuestion].lineSteps[indexStep].positionCount)
        {
            finish = true;

            lineObject[indexQuestion].transform.Find("提示 " + indexStep).gameObject.SetActive(false);

            for (int i = 0; i < 3; i++) lines[i].enabled = false;
            imgQuestion.color = Color.white;

            StartCoroutine(Correct());
        }

        if (indexStep == 0 && indexStep < lineQuestion[indexQuestion].lineSteps.Length && indexLine == lineQuestion[indexQuestion].lineSteps[indexStep].positionCount)
        {
            lineObject[indexQuestion].transform.Find("提示 " + indexStep).gameObject.SetActive(false);
            indexStep++;
            lineObject[indexQuestion].transform.Find("提示 " + indexStep).gameObject.SetActive(true);
        }
        else if (lineQuestion[indexQuestion].lineSteps.Length > 2 && indexStep == 1 && indexStep < lineQuestion[indexQuestion].lineSteps.Length && indexLine == lineQuestion[indexQuestion].lineSteps[indexStep].positionCount + lineQuestion[indexQuestion].lineSteps[indexStep - 1].positionCount)
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
            LineRenderer q = lineQuestion[indexQuestion].lineSteps[indexStep];

            // 減去前一個步驟的數量
            if (indexStep == 1) indexCurrent = indexLine - lineQuestion[indexQuestion].lineSteps[indexStep - 1].positionCount;
            else if (indexStep == 2) indexCurrent = indexLine - lineQuestion[indexQuestion].lineSteps[indexStep - 1].positionCount - lineQuestion[indexQuestion].lineSteps[indexStep - 2].positionCount;
            else if (indexCurrent < q.positionCount) indexCurrent = indexLine;

            if (indexCurrent == lineQuestion[indexQuestion].lineSteps[indexStep].positionCount) return;                                 // 避免 編號 跑到 最後一個 導致錯誤
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