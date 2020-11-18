using UnityEngine;
using PDollarGestureRecognizer;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class Lv1_Teacher : LevelBase
{
    [Header("手勢物件 - 線條渲染")]
    public Transform gesture;
    [Header("繪製區域樣式")]
    public GUIStyle style;
    [Header("題目：圖片")]
    public Image imgQuestion;
    public Sprite[] sprQuestion;

    private string[] questions =
    {
        "0", "1", "2","3", "4", "5", "6", "7", "8", "9",
        "A", "B", "C", "D", "E", "F", "G", "H", "I", 
        "J", "K", "L", "M", "N", "O", "P", "Q", "R",
        "S", "T", "U", "V", "W", "X", "Y", "Z"
    };

    private string answer;

    private List<Gesture> traingSet = new List<Gesture>();                              // 手勢清單

    private List<Point> points = new List<Point>();                                     // 點
    private int strokeId = -1;                                                          // 編號

    private Vector3 virtualKeyPosition = Vector2.zero;                                  // 玩家點擊座標
    private Rect drawArea;                                                              // 繪製區域

    private RuntimePlatform platform;                                                   // 平台
    private int vertexCount = 0;                                                        // 頂點數量

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();         // 手勢線條渲染
    private LineRenderer currentGestureLineRenderer;                                    // 目前線條渲染

    private string message;                                                             // 訊息
    private bool recognized;                                                            // 辨識
    private string newGestureNmae = "";                                                 // 手勢名稱

    private void Start()
    {
        platform = Application.platform;                                                                    // 目前平台

        int w = Screen.width / 2;
        drawArea = new Rect(w / 2, (Screen.height - w) / 2, w, w);                                          // 設定彗置區域

        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");             // 讀取 預先製作 手勢
        foreach (TextAsset gestureXml in gesturesXml)
            traingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");                   // 讀取 自訂 手勢
        foreach (string filePath in filePaths)
            traingSet.Add(GestureIO.ReadGestureFromFile(filePath));

        
        Question(0);        // 發問題
    }

    protected override void Update()
    {
        base.Update();

        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)                                            // 如果 平台 為 Android 或者 IPhoneP
        {
            if (Input.touchCount > 0) virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);     // 點擊座標 為 觸碰 座標
        }
        else                                                                                                                            // 否則 其他平台
        {
            if (Input.GetMouseButton(0)) virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);                // 點擊座標 為 滑鼠 座標
        }

        if (drawArea.Contains(virtualKeyPosition))                                      // 如果 玩家點擊區域 在繪製區域內
        {
            if (Input.GetMouseButtonDown(0))                                            // 如果 按下左鍵
            {
                if (recognized)                                                         // 如果 正在辨識
                {
                    recognized = false;                                                 // 不在辨識
                    strokeId = -1;                                                      // 編號 = -1

                    points.Clear();                                                     // 清除所有點

                    foreach (LineRenderer lineRenderer in gestureLinesRenderer)         // 迴圈執行每個線條渲染
                    {
                        lineRenderer.positionCount = 0;                                 // 數量歸零
                        Destroy(lineRenderer.gameObject);                               // 刪除線條渲染
                    }
                }

                ++strokeId;                                                             // 編號遞曾

                Transform tmpGesture = Instantiate(gesture, transform.position, transform.rotation) as Transform;   // 生成 手勢物件
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();                               // 取得手勢物件的線條渲染

                gestureLinesRenderer.Add(currentGestureLineRenderer);                                               // 加到清單內

                vertexCount = 0;                                                                                    // 頂點數量歸零
            }

            if (Input.GetMouseButton(0))                                                                            // 如果 按住 左鍵
            {
                points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));                       // 將玩家點擊座標加到清單內

                currentGestureLineRenderer.positionCount = ++vertexCount;                                           // 添加節點數量
                currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
            }
        }
    }

    private void OnGUI()
    {
        GUI.Box(drawArea, "繪製區域", style);

        GUI.Label(new Rect(10, Screen.height - 40, 500, 50), message);
    }

    /// <summary>
    /// 辨識
    /// </summary>
    public bool Recognize()
    {
        recognized = true;

        Gesture candidate = new Gesture(points.ToArray());
        Result gestureResult = PointCloudRecognizer.Classify(candidate, traingSet.ToArray());

        message = gestureResult.GestureClass + " " + gestureResult.Score;

        print(message);
        print(answer);
        print(gestureResult.GestureClass == answer && gestureResult.Score > 0.3f);

        return gestureResult.GestureClass == answer && gestureResult.Score > 0.3f;
    }

    protected override void TimeStop()
    {
        if (Recognize()) StartCoroutine(Win()); else StartCoroutine(Lose());
    }

    protected override void Question(float delayStart)
    {
        int r = Random.Range(0, questions.Length);
        answer = questions[r];

        imgQuestion.sprite = sprQuestion[r];

        base.Question(delayStart);
    }
}
