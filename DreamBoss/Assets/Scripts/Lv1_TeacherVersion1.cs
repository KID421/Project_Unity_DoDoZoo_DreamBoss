using UnityEngine;
using UnityEngine.UI;

public class Lv1_TeacherVersion1 : LevelBase
{
    [Header("繪製判斷距離"), Range(0.5f, 5)]
    public float drawDistance = 1.3f;
    [Header("題目編號：數字")]
    public int indexQuestionNumber;
    [Header("題目編號：字母")]
    public int indexQuestionLetter;
    [Header("題目：數字 0 - 10")]
    public Sprite[] sprQuestionsNumbers;
    /// <summary>
    /// 題目線條：放所有製作完成的題目線條
    /// </summary>
    [Header("題目線條：有 Line Renderer 的物件")]
    public TeacherQuestion[] lineQuestionNumbers;
    [Header("題目物件，父物件，包含提示物件")]
    public GameObject[] lineObjectNumbers;
    [Header("題目：英文 A - Z")]
    public Sprite[] sprQuestionsLetters;
    /// <summary>
    /// 題目線條：放所有製作完成的題目線條
    /// </summary>
    [Header("題目線條：有 Line Renderer 的物件")]
    public TeacherQuestion[] lineQuestionLetters;
    [Header("題目物件，父物件，包含提示物件")]
    public GameObject[] lineObjectLetters;
    [Header("字母完成的圖示")]
    public Sprite[] sprLetterIconFinish;
    [Header("字母完成的單字")]
    public Sprite[] sprLetterWordFinish;
    [Header("切換英文字母與數字模式的按鈕")]
    public Button btnSwitch;

    /// <summary>
    /// 是否為英文字母模式：預設為不是
    /// </summary>
    public static bool englishMode;

    /// <summary>
    /// 題目圖片：數字
    /// </summary>
    private Image imgQuestionNumber;
    /// <summary>
    /// 題目圖片：字母
    /// </summary>
    private Image imgQuestionLetter;
    /// <summary>
    /// 數字：線條渲染：玩家繪製產生的線條
    /// </summary>
    private LineRenderer[] numberLines = new LineRenderer[3];
    /// <summary>
    /// 數字：線條編號：玩家繪製產生的線條編號
    /// </summary>
    private int numberIndexLine;
    /// <summary>
    /// 數字：步驟的編號
    /// </summary>
    private int numberIndexStep = 0;
    /// <summary>
    /// 數字：目前的線條編號 - 每一線段從 0 - 結束用
    /// </summary>
    private int numberIndexCurrent;
    /// <summary>
    /// 數字：是否結束
    /// </summary>
    private bool numberFinish;
    /// <summary>
    /// 完成字母圖示
    /// </summary>
    private Image imgFinishLetterIcon;
    /// <summary>
    /// 完成字母單字
    /// </summary>
    private Image imgFinishLetterWord;

    /// <summary>
    /// 字母：線條渲染：玩家繪製產生的線條
    /// </summary>
    private LineRenderer[] letterLines = new LineRenderer[4];
    /// <summary>
    /// 字母：線條編號：玩家繪製產生的線條編號
    /// </summary>
    private int letterIndexLine;
    /// <summary>
    /// 字母：步驟的編號
    /// </summary>
    private int letterIndexStep = 0;
    /// <summary>
    /// 字母：目前的線條編號 - 每一線段從 0 - 結束用
    /// </summary>
    private int letterIndexCurrent;
    /// <summary>
    /// 字母：是否結束
    /// </summary>
    private bool letterFinish;
    /// <summary>
    /// 鎖
    /// </summary>
    private Image imgLock;

    private GameObject objLineNumber;           // 線段數字
    private GameObject objLineLetter;           // 線段字母
    private GameObject objQuestionNumber;       // 題目數字
    private GameObject objQuestionLetter;       // 題目字母

    protected override void Awake()
    {
        base.Awake();

        objLineNumber = GameObject.Find("線段數字");
        objLineLetter = GameObject.Find("線段字母");
        objQuestionNumber = GameObject.Find("題目數字");
        objQuestionLetter = GameObject.Find("題目字母");

        imgFinishLetterIcon = GameObject.Find("完成字母圖示").GetComponent<Image>();
        imgFinishLetterWord = GameObject.Find("完成字母單字").GetComponent<Image>();

        imgLock = GameObject.Find("鎖").GetComponent<Image>();
        imgLock.GetComponent<Button>().onClick.AddListener(PaidManager.instance.ClickLock);

        InitializeLineNumber();
        InitializeLineLetter();

        objLineLetter.SetActive(false);
        objQuestionLetter.SetActive(false);

        btnSwitch.onClick.AddListener(() => { SwitchEnglishAndNumber(); });        // 切換英文字母與數字模式

        SwitchEnglishAndNumber(false);
    }

    /// <summary>
    /// 重新設定到數字模式
    /// </summary>
    public void ResetToNumberMode()
    {
        englishMode = false;
        SwitchEnglishAndNumber(false);
    }

    /// <summary>
    /// 初始化線條：隨機題目數字
    /// </summary>
    private void InitializeLineNumber()
    {
        // 隨機題目
        imgQuestionNumber = GameObject.Find("題目圖片：數字").GetComponent<Image>();
        indexQuestionNumber = Random.Range(0, lineQuestionNumbers.Length);

        // 測試
        // indexQuestionNumber = 8;

        imgQuestionNumber.sprite = sprQuestionsNumbers[indexQuestionNumber];
        // 顯示提示物件
        lineObjectNumbers[indexQuestionNumber].SetActive(true);

        for (int i = 0; i < 3; i++) numberLines[i] = GameObject.Find("線段數字 " + i).GetComponent<LineRenderer>();

        LineRenderer q = lineQuestionNumbers[indexQuestionNumber].lineSteps[0];

        // 取得題目的第一個點
        Vector3 posLineQuestion = q.GetPosition(numberIndexLine);
        numberLines[0].positionCount = 1;
        numberLines[0].SetPosition(numberIndexLine, posLineQuestion);
        // 編號遞增
        numberIndexLine++;
    }

    /// <summary>
    /// 初始化線條：隨機題目字母
    /// </summary>
    private void InitializeLineLetter()
    {
        // 隨機題目
        imgQuestionLetter = GameObject.Find("題目圖片：字母").GetComponent<Image>();
        indexQuestionLetter = Random.Range(0, lineQuestionLetters.Length);

        // 測試
        // indexQuestionLetter = 2;

        imgQuestionLetter.sprite = sprQuestionsLetters[indexQuestionLetter];
        // 顯示提示物件
        lineObjectLetters[indexQuestionLetter].SetActive(true);

        for (int i = 0; i < 4; i++) letterLines[i] = GameObject.Find("線段字母 " + i).GetComponent<LineRenderer>();

        LineRenderer q = lineQuestionLetters[indexQuestionLetter].lineSteps[0];

        // 取得題目的第一個點
        Vector3 posLineQuestion = q.GetPosition(letterIndexLine);
        letterLines[0].positionCount = 1;
        letterLines[0].SetPosition(letterIndexLine, posLineQuestion);
        // 編號遞增
        letterIndexLine++;
    }

    protected override void Update()
    {
        base.Update();

        CheckMousePoisitionNumber();
        CheckMousePoisitionLetter();
    }

    /// <summary>
    /// 檢查滑鼠觸碰點擊位置
    /// 如果很靠近題目的線條位置就產生線條
    /// </summary>
    private void CheckMousePoisitionNumber()
    {
        // 如果是英文模式就跳出
        if (englishMode) return;

        // 已經完成
        if (numberFinish) return;

        // 正確
        if (numberIndexStep == lineQuestionNumbers[indexQuestionNumber].lineSteps.Length - 1 && numberIndexCurrent == lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep].positionCount)
        {
            numberFinish = true;

            lineObjectNumbers[indexQuestionNumber].transform.Find("提示 " + numberIndexStep).gameObject.SetActive(false);

            for (int i = 0; i < 3; i++) numberLines[i].enabled = false;
            imgQuestionNumber.color = Color.white;

            StartCoroutine(Correct());
        }

        if (numberIndexStep == 0 && numberIndexStep < lineQuestionNumbers[indexQuestionNumber].lineSteps.Length && numberIndexLine == lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep].positionCount)
        {
            lineObjectNumbers[indexQuestionNumber].transform.Find("提示 " + numberIndexStep).gameObject.SetActive(false);
            numberIndexStep++;
            lineObjectNumbers[indexQuestionNumber].transform.Find("提示 " + numberIndexStep).gameObject.SetActive(true);
        }
        else if (lineQuestionNumbers[indexQuestionNumber].lineSteps.Length > 2 && numberIndexStep == 1 && numberIndexStep < lineQuestionNumbers[indexQuestionNumber].lineSteps.Length && numberIndexLine == lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep].positionCount + lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep - 1].positionCount)
        {
            lineObjectNumbers[indexQuestionNumber].transform.Find("提示 " + numberIndexStep).gameObject.SetActive(false);
            numberIndexStep++;
            lineObjectNumbers[indexQuestionNumber].transform.Find("提示 " + numberIndexStep).gameObject.SetActive(true);
        }

        // 如果按住左鍵
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // 取得滑鼠位置
            Vector3 posMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 posMouseWorld = Camera.main.ScreenToWorldPoint(posMouse);

            // 取得題目位置
            LineRenderer q = lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep];

            // 減去前一個步驟的數量
            if (numberIndexStep == 1) numberIndexCurrent = numberIndexLine - lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep - 1].positionCount;
            else if (numberIndexStep == 2) numberIndexCurrent = numberIndexLine - lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep - 1].positionCount - lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep - 2].positionCount;
            else if (numberIndexCurrent < q.positionCount) numberIndexCurrent = numberIndexLine;

            if (numberIndexCurrent == lineQuestionNumbers[indexQuestionNumber].lineSteps[numberIndexStep].positionCount) return;                                 // 避免 編號 跑到 最後一個 導致錯誤
            Vector3 posLineQuestion = q.GetPosition(numberIndexCurrent);                                                          // 取得題目的每個當前位置
            
            float dis = Vector3.Distance(posMouseWorld, posLineQuestion);                                                   // 判斷 滑鼠 與 題目 距離

            //print("題目：" + posLineQuestion);
            //print("滑鼠：" + posMouseWorld);
            //print("距離：" + dis);

            if (dis < drawDistance)                                                                                         // 如果距離 小於 判斷距離 就畫出線條
            {
                numberLines[numberIndexStep].positionCount = (numberIndexCurrent + 1);
                numberLines[numberIndexStep].SetPosition(numberIndexCurrent, posLineQuestion);
                numberIndexLine++;
            }
        }
    }

    /// <summary>
    /// 檢查滑鼠觸碰點擊位置
    /// 如果很靠近題目的線條位置就產生線條
    /// </summary>
    private void CheckMousePoisitionLetter()
    {
        // 如果還沒付費就跳出
        if (!PaidManager.paid) return;
        // 如果不是英文模式就跳出
        if (!englishMode) return;
        // 已經完成
        if (letterFinish) return;

        // 正確
        if (letterIndexStep == lineQuestionLetters[indexQuestionLetter].lineSteps.Length - 1 && letterIndexCurrent == lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep].positionCount)
        {
            letterFinish = true;

            lineObjectLetters[indexQuestionLetter].transform.Find("提示 " + letterIndexStep).gameObject.SetActive(false);

            for (int i = 0; i < 4; i++) letterLines[i].enabled = false;

            imgQuestionLetter.color = Color.clear;

            StartCoroutine(Correct());

            imgFinishLetterWord.enabled = true;
            imgFinishLetterIcon.enabled = true;
            imgFinishLetterIcon.sprite = sprLetterIconFinish[indexQuestionLetter];
            imgFinishLetterWord.sprite = sprLetterWordFinish[indexQuestionLetter];
        }

        if (letterIndexStep == 0 && lineQuestionLetters[indexQuestionLetter].lineSteps.Length == 1 && letterIndexLine == lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep].positionCount)
        {
            
        }
        else if (letterIndexStep == 0 && letterIndexStep < lineQuestionLetters[indexQuestionLetter].lineSteps.Length && letterIndexLine == lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep].positionCount)
        {
            lineObjectLetters[indexQuestionLetter].transform.Find("提示 " + letterIndexStep).gameObject.SetActive(false);
            letterIndexStep++;
            lineObjectLetters[indexQuestionLetter].transform.Find("提示 " + letterIndexStep).gameObject.SetActive(true);
        }
        else if (lineQuestionLetters[indexQuestionLetter].lineSteps.Length > 2 && letterIndexStep == 1 && letterIndexStep < lineQuestionLetters[indexQuestionLetter].lineSteps.Length && letterIndexLine == lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep].positionCount + lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 1].positionCount)
        {
            lineObjectLetters[indexQuestionLetter].transform.Find("提示 " + letterIndexStep).gameObject.SetActive(false);
            letterIndexStep++;
            lineObjectLetters[indexQuestionLetter].transform.Find("提示 " + letterIndexStep).gameObject.SetActive(true);
        }
        else if (lineQuestionLetters[indexQuestionLetter].lineSteps.Length > 3 && letterIndexStep == 2 && letterIndexStep < lineQuestionLetters[indexQuestionLetter].lineSteps.Length && letterIndexLine == lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep].positionCount + lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 1].positionCount + lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 2].positionCount)
        {
            lineObjectLetters[indexQuestionLetter].transform.Find("提示 " + letterIndexStep).gameObject.SetActive(false);
            letterIndexStep++;
            lineObjectLetters[indexQuestionLetter].transform.Find("提示 " + letterIndexStep).gameObject.SetActive(true);
        }

        // 如果按住左鍵
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // 取得滑鼠位置
            Vector3 posMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 posMouseWorld = Camera.main.ScreenToWorldPoint(posMouse);

            // 取得題目位置
            LineRenderer q = lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep];

            // 減去前一個步驟的數量
            if (letterIndexStep == 1) letterIndexCurrent = letterIndexLine - lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 1].positionCount;
            else if (letterIndexStep == 2) letterIndexCurrent = letterIndexLine - lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 1].positionCount - lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 2].positionCount;
            else if (letterIndexStep == 3) letterIndexCurrent = letterIndexLine - lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 1].positionCount - lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 2].positionCount - lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep - 3].positionCount;
            else if (letterIndexCurrent < q.positionCount) letterIndexCurrent = letterIndexLine;

            if (letterIndexCurrent == lineQuestionLetters[indexQuestionLetter].lineSteps[letterIndexStep].positionCount) return;            // 避免 編號 跑到 最後一個 導致錯誤
            Vector3 posLineQuestion = q.GetPosition(letterIndexCurrent);                                                                    // 取得題目的每個當前位置

            float dis = Vector3.Distance(posMouseWorld, posLineQuestion);                                                                   // 判斷 滑鼠 與 題目 距離

            if (dis < drawDistance)                                                                                                         // 如果距離 小於 判斷距離 就畫出線條
            {
                letterLines[letterIndexStep].positionCount = (letterIndexCurrent + 1);
                letterLines[letterIndexStep].SetPosition(letterIndexCurrent, posLineQuestion);
                letterIndexLine++;
            }
        }
    }

    /// <summary>
    /// 切換英文字母與數字
    /// </summary>
    /// <param name="change">是否要切換模式</param>
    private void SwitchEnglishAndNumber(bool change = true)
    {
        if (change) englishMode = !englishMode;

        if (englishMode)
        {
            numberIndexCurrent = 1;
            btnSwitch.transform.Find("圓圈").GetComponent<RectTransform>().anchoredPosition = new Vector2(65, 0);
            btnSwitch.transform.Find("有顏色底圖").GetComponent<Image>().color = new Color(0.5f, 0.8f, 0.3f);
            objLineLetter.SetActive(true);
            objLineNumber.SetActive(false);
            objQuestionLetter.SetActive(PaidManager.paid);
            objQuestionNumber.SetActive(false);
            imgQuestionLetter.enabled = true;
            imgQuestionNumber.enabled = false;

            imgLock.enabled = !PaidManager.paid;
        }
        else
        {
            numberIndexCurrent = 0;
            btnSwitch.transform.Find("圓圈").GetComponent<RectTransform>().anchoredPosition = new Vector2(-65, 0);
            btnSwitch.transform.Find("有顏色底圖").GetComponent<Image>().color = new Color(0.8f, 0.2f, 0.3f);
            objLineLetter.SetActive(false);
            objLineNumber.SetActive(true);
            objQuestionLetter.SetActive(false);
            objQuestionNumber.SetActive(true);
            imgQuestionLetter.enabled = false;
            imgQuestionNumber.enabled = true;

            imgLock.enabled = false;
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