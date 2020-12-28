using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class Lv12_Investor : LevelBase
{
    [Header("捲動間隔時間"), Range(0f, 1f)]
    public float interval = 0.02f;

    /// <summary>
    /// 原本的間隔時間
    /// </summary>
    private float intervalOriginal;
    /// <summary>
    /// 按鈕 100 上方
    /// </summary>
    private Button btn100;
    /// <summary>
    /// 內容物
    /// </summary>
    private RectTransform[] contents = new RectTransform[3];
    /// <summary>
    /// 是否捲動結束
    /// </summary>
    private bool[] finishes = { false, false, false };
    /// <summary>
    /// 三個內容區塊的第一個子物件
    /// </summary>
    private RectTransform[] firstChilds = new RectTransform[3];

    protected override void Awake()
    {
        base.Awake();

        intervalOriginal = interval;

        contents[0] = GameObject.Find("內容物 1").GetComponent<RectTransform>();
        contents[1] = GameObject.Find("內容物 2").GetComponent<RectTransform>();
        contents[2] = GameObject.Find("內容物 3").GetComponent<RectTransform>();

        btn100 = GameObject.Find("按鈕 100 上方").GetComponent<Button>();
        btn100.onClick.AddListener(() => 
        { 
            StartCoroutine(ScrollContent(0));
            StartCoroutine(ScrollContent(1));
            StartCoroutine(ScrollContent(2));
        });
    }

    /// <summary>
    /// 開始捲動內容
    /// </summary>
    /// <param name="indexOfContent">要捲動的內容編號 0 - 2</param>
    private IEnumerator ScrollContent(int indexOfContent)
    {
        int count = 0;                              // 捲動次數歸零
        int countTotal = Random.Range(10, 16);      // 捲動次數 10 - 15 次，會乘以 100，實際次數為 100 - 150
        countTotal = 5;
        interval = intervalOriginal;                // 設定為原始間隔時間
        finishes[indexOfContent] = false;           // 將所有捲動內容設定為尚未結束

        // 當捲動次數 小於 總捲動次數 * 10 時 - 100 - 150 次
        while (count < countTotal * 10)
        {
            count++;                                                                                                    // 捲動次數遞增
            contents[indexOfContent].anchoredPosition -= Vector2.up * 27.5f;                                            // 每次往下捲動 27.5 - 每張卡片大小為 275

            if (count % 10 == 0)                                                                                        // 每捲動 10 次 就將 第一個子物件向上位移
            {
                firstChilds[indexOfContent] = contents[indexOfContent].GetChild(0).GetComponent<RectTransform>();       // 取得第一個子物件
                firstChilds[indexOfContent].anchoredPosition += Vector2.up * 275f * 4;                                  // 向上移動 大小 275 * 4 (內容物有幾張)
                firstChilds[indexOfContent].SetAsLastSibling();                                                         // 並將其設定為最後一個子物件
            }

            // 漸漸變慢的效果
            float intervalTemp = interval;                                                                              // 區域變數讓每個內容區別間隔時間
            if (count > countTotal * 10 - 5) intervalTemp = 0.2f;                                                       // 剩下 05 次 0.3 秒
            else if (count > countTotal * 10 - 10) intervalTemp = 0.15f;                                                // 剩下 10 次 0.2 秒
            else if (count > countTotal * 10 - 20) intervalTemp = 0.1f;                                                 // 剩下 20 次 0.1 秒

            yield return new WaitForSeconds(intervalTemp);                                                              // 等待間隔時間
        }

        firstChilds[indexOfContent] = contents[indexOfContent].GetChild(0).GetComponent<RectTransform>();               // 取得第一個子物件
        finishes[indexOfContent] = true;                                                                                // 設定此內容已經結束

        var allFinish = finishes.Where(x => x == true);                                                                 // 查詢三個內容中有幾個完成
        if (allFinish.ToList().Count == 3)                                                                              // 如果 所有內容都完成
        {
            var allFirstChilds = firstChilds.Where(x => x.name == firstChilds[0].name);                                 // 取得所有內容的物品 是不是 等於 第一個物品 (如果都是代表三個相同)
            if (allFirstChilds.ToList().Count == 3) print(firstChilds[0].GetChild(0).name);
        }
    }
}
