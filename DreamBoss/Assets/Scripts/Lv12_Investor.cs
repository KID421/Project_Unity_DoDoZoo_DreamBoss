using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class Lv12_Investor : LevelBase
{
    [Header("捲動間隔時間"), Range(0f, 1f)]
    public float interval = 0.02f;
    [Header("遊戲開始擁有的資金")]
    private int coin = 1000;
    [Header("圖片：-100")]
    public Sprite spr100;
    [Header("圖片：-200")]
    public Sprite spr200;
    [Header("獲取的物品圖片：金幣、鈔票、股票、豬公、汽車、直升機、房子")]
    public Sprite[] sprObjects;

    /// <summary>
    /// 文字：擁有的資金
    /// </summary>
    private Text textCoin;
    /// <summary>
    /// 原本的間隔時間
    /// </summary>
    private float intervalOriginal;
    /// <summary>
    /// 按鈕 100 上方
    /// </summary>
    private Button btn100;
    /// <summary>
    /// 按鈕 200 上方
    /// </summary>
    private Button btn200;
    /// <summary>
    /// 內容物
    /// </summary>
    private RectTransform[,] contents = new RectTransform[2, 3];
    /// <summary>
    /// 是否捲動結束
    /// </summary>
    private bool[,] finishes = { { false, false, false }, { false, false, false } };
    /// <summary>
    /// 三個內容區塊的第一個子物件
    /// </summary>
    private RectTransform[,] firstChilds = new RectTransform[2, 3];
    /// <summary>
    /// 在錢包上面的金幣：扣除金額
    /// </summary>
    private RectTransform rectCoinOnWallet;
    /// <summary>
    /// 取得的物件圖片
    /// </summary>
    private Image imgGetObject;

    protected override void Awake()
    {
        base.Awake();

        FindAndButtonSetting();
    }

    /// <summary>
    /// 尋找物件與按鈕設定
    /// </summary>
    private void FindAndButtonSetting()
    {
        intervalOriginal = interval;                                                // 設定原始間隔時間

        // 介面
        textCoin = GameObject.Find("擁有的資金").GetComponent<Text>();
        textCoin.text = coin + "";

        rectCoinOnWallet = GameObject.Find("扣除金額").GetComponent<RectTransform>();

        imgGetObject = GameObject.Find("取得的物件圖片").GetComponent<Image>();

        // 尋找三排內容物 - 100 元
        contents[0, 0] = GameObject.Find("內容物 100 1").GetComponent<RectTransform>();
        contents[0, 1] = GameObject.Find("內容物 100 2").GetComponent<RectTransform>();
        contents[0, 2] = GameObject.Find("內容物 100 3").GetComponent<RectTransform>();
        // 尋找三排內容物 - 200 元
        contents[1, 0] = GameObject.Find("內容物 200 1").GetComponent<RectTransform>();
        contents[1, 1] = GameObject.Find("內容物 200 2").GetComponent<RectTransform>();
        contents[1, 2] = GameObject.Find("內容物 200 3").GetComponent<RectTransform>();

        // 一百塊與兩百塊按鈕點擊設定：三排內容物開始捲動
        btn100 = GameObject.Find("按鈕 100 上方").GetComponent<Button>();
        btn200 = GameObject.Find("按鈕 200 上方").GetComponent<Button>();

        btn100.onClick.AddListener(() =>
        {
            if (coin <= 0) return;
            if (coin < 100) return;

            for (int i = 0; i < 6; i++) contents[i / 3, i % 3].transform.parent.gameObject.SetActive(i < 3);

            SetCoin(-100);
            StartCoroutine(ScrollContent(0, 0));
            StartCoroutine(ScrollContent(0, 1));
            StartCoroutine(ScrollContent(0, 2));
            StartCoroutine(ButtonEffect(btn100.GetComponent<RectTransform>()));
        });
        btn200.onClick.AddListener(() =>
        {
            if (coin <= 0) return;
            if (coin < 200) return;

            for (int i = 0; i < 6; i++) contents[i / 3, i % 3].transform.parent.gameObject.SetActive(i > 2);

            SetCoin(-200);
            StartCoroutine(ScrollContent(1, 0));
            StartCoroutine(ScrollContent(1, 1));
            StartCoroutine(ScrollContent(1, 2));
            StartCoroutine(ButtonEffect(btn200.GetComponent<RectTransform>()));
        });
    }

    /// <summary>
    /// 設定金幣並更新介面
    /// </summary>
    /// <param name="value">要更新金幣的值，例如：100、-100</param>
    private void SetCoin(int value)
    {
        coin += value;
        textCoin.text = coin + "";

        Sprite spr = value == -100 ? spr100 : spr200;

        StartCoroutine(CoinEffect(rectCoinOnWallet, spr));
    }

    private IEnumerator ButtonEffect(RectTransform btn)
    {
        btn100.interactable = false;
        btn200.interactable = false;

        Vector3 posOriginal = btn.anchoredPosition;
        Vector3 pos = btn.anchoredPosition;

        float y = pos.y - 60;

        while (pos.y > y)
        {
            pos.y -= 10;
            btn.anchoredPosition = pos;
            yield return new WaitForSeconds(0.02f);
        }

        btn.anchoredPosition = posOriginal;
    }

    /// <summary>
    /// 金幣特效
    /// </summary>
    /// <param name="rectCoin">要執行特效的金幣</param>
    /// <param name="spr">要顯示的圖片 -100 或 -200</param>
    /// <param name="up">往上升多少，預設為 200</param>
    private IEnumerator CoinEffect(RectTransform rectCoin, Sprite spr, float up = 200)
    {
        Vector3 posOriginal = rectCoin.anchoredPosition;        // 取的原始位置，最後恢復用

        Vector3 pos = rectCoin.anchoredPosition;                // 取得座標 - 加乘用
        float y = pos.y + up;                                   // 往上位移 - 預設 200

        Image img = rectCoin.GetComponent<Image>();             // 取得金幣圖片元件
        img.sprite = spr;                                       // 更新圖片
        img.SetNativeSize();

        while (pos.y < y)                                       // 預設每 0.05 秒上升 30 並顯示
        {
            pos.y += 30;
            img.color += new Color(0, 0, 0, 0.3f);
            rectCoin.anchoredPosition = pos;
            yield return new WaitForSeconds(0.05f);
        }
        while (img.color.a > 0)                                 // 上升後隱藏
        {
            img.color -= new Color(0, 0, 0, 0.3f);
            yield return new WaitForSeconds(0.05f);
        }

        rectCoin.anchoredPosition = posOriginal;                // 回到原始位置
        img.color = new Color(1, 1, 1, 0);                      // 透明
    }

    /// <summary>
    /// 開始捲動內容
    /// </summary>
    /// <param name="indexType">類型：0 - 一百塊， 1 兩百塊</param>
    /// <param name="indexOfContent">要捲動的內容編號 0 - 2</param>
    private IEnumerator ScrollContent(int indexType, int indexOfContent)
    {
        contents[indexType, indexOfContent].parent.parent.GetChild(3).gameObject.SetActive(false);     // 貓頭鷹消失

        int count = 0;                              // 捲動次數歸零
        int countTotal = Random.Range(10, 16);      // 捲動次數 10 - 15 次，會乘以 100，實際次數為 100 - 150
        countTotal = 5;
        interval = intervalOriginal;                // 設定為原始間隔時間
        finishes[indexType, indexOfContent] = false;           // 將所有捲動內容設定為尚未結束

        // 當捲動次數 小於 總捲動次數 * 10 時 - 100 - 150 次
        while (count < countTotal * 10)
        {
            count++;                                                                                                    // 捲動次數遞增
            contents[indexType, indexOfContent].anchoredPosition -= Vector2.up * 27.5f;                                            // 每次往下捲動 27.5 - 每張卡片大小為 275

            if (count % 10 == 0)                                                                                        // 每捲動 10 次 就將 第一個子物件向上位移
            {
                firstChilds[indexType, indexOfContent] = contents[indexType, indexOfContent].GetChild(0).GetComponent<RectTransform>();       // 取得第一個子物件
                firstChilds[indexType, indexOfContent].anchoredPosition += Vector2.up * 275f * 4;                                  // 向上移動 大小 275 * 4 (內容物有幾張)
                firstChilds[indexType, indexOfContent].SetAsLastSibling();                                                         // 並將其設定為最後一個子物件
            }

            // 漸漸變慢的效果
            float intervalTemp = interval;                                                                              // 區域變數讓每個內容區別間隔時間
            if (count > countTotal * 10 - 5) intervalTemp = 0.2f;                                                       // 剩下 05 次 0.3 秒
            else if (count > countTotal * 10 - 10) intervalTemp = 0.15f;                                                // 剩下 10 次 0.2 秒
            else if (count > countTotal * 10 - 20) intervalTemp = 0.1f;                                                 // 剩下 20 次 0.1 秒

            yield return new WaitForSeconds(intervalTemp);                                                              // 等待間隔時間
        }

        firstChilds[indexType, indexOfContent] = contents[indexType, indexOfContent].GetChild(0).GetComponent<RectTransform>();               // 取得第一個子物件
        finishes[indexType, indexOfContent] = true;                                                                                // 設定此內容已經結束

        bool[] column = new bool[3];
        for (int i = 0; i < 3; i++) column[i] = finishes[indexType, i];

        RectTransform[] cihlds = new RectTransform[3];
        for (int i = 0; i < 3; i++) cihlds[i] = firstChilds[indexType, i];

        var allFinish = column.Where(x => x == true);                                                                 // 查詢三個內容中有幾個完成
        if (allFinish.ToList().Count == 3)                                                                              // 如果 所有內容都完成
        {
            var allFirstChilds = cihlds.Where(x => x.name == cihlds[0].name);                                 // 取得所有內容的物品 是不是 等於 第一個物品 (如果都是代表三個相同)
            if (allFirstChilds.ToList().Count == 3) GetObject(cihlds[0].GetChild(0).name);

            btn100.interactable = true;
            btn200.interactable = true;
        }
    }

    /// <summary>
    /// 取得物件
    /// </summary>
    /// <param name="objectName">獲得物件的名稱</param>
    private void GetObject(string objectName)
    {
        switch (objectName)
        {
            case "金幣":
                StartCoroutine(CoinEffect(imgGetObject.rectTransform, sprObjects[0]));
                print("+100");
                break;
            case "鈔票":
                StartCoroutine(CoinEffect(imgGetObject.rectTransform, sprObjects[1]));
                print("+200");
                break;
            case "股票":
                StartCoroutine(CoinEffect(imgGetObject.rectTransform, sprObjects[2]));
                print("股票");
                break;
            case "豬公":
                StartCoroutine(CoinEffect(imgGetObject.rectTransform, sprObjects[3]));
                print("豬公");
                break;
            case "汽車":
                StartCoroutine(CoinEffect(imgGetObject.rectTransform, sprObjects[4]));
                print("汽車");
                break;
            case "直升機":
                StartCoroutine(CoinEffect(imgGetObject.rectTransform, sprObjects[5]));
                print("直升機");
                break;
            case "房子":
                StartCoroutine(CoinEffect(imgGetObject.rectTransform, sprObjects[6]));
                print("房子");
                break;
        }
    }
}
