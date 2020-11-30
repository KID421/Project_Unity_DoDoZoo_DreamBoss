using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Lv7_Entrepreneur : LevelBase
{
    /// <summary>
    /// 是否開始點擊
    /// </summary>
    private bool startClick;
    /// <summary>
    /// 是否點到起點
    /// </summary>
    private bool clickStart;

    /// <summary>
    /// 點擊的介面
    /// </summary>
    private List<RaycastResult> results;

    /// <summary>
    /// 前一顆磚塊
    /// </summary>
    private RectTransform prevBrick;

    [Header("起點顏色")]
    public Color colorStart;
    [Header("滑過顏色")]
    public Color colorNormal;
    [Header("吃金幣音效")]
    public AudioClip soundCoin;
    [Header("關卡")]
    public GameObject[] level;

    public static int coinCount;

    protected override void Awake()
    {
        base.Awake();

        level[Random.Range(0, level.Length)].SetActive(true);
    }

    protected override void Update()
    {
        base.Update();

        Mouse();
    }

    private void Mouse()
    {
        // 是否按下左鍵
        if (!startClick && Input.GetKeyDown(KeyCode.Mouse0))
        {
            startClick = true;
        }
        // 是否按下左鍵並且拖拉中
        else if (startClick && Input.GetKey(KeyCode.Mouse0))
        {
            // 滑鼠射線與介面碰撞偵測
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;
            results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);

            // 如果碰到介面
            if (results.Count > 0)
            {
                // 如果還沒開始點擊並且先點擊的是起點：開始，改變顏色記錄前一個磚塊
                if (!clickStart && results[0].gameObject.GetComponent<Lv7_Brick>().isStart)
                {
                    clickStart = true;
                    results[0].gameObject.GetComponent<Lv7_Brick>().isStart = false;
                    results[0].gameObject.GetComponent<Image>().color = colorNormal;
                    prevBrick = results[0].gameObject.GetComponent<RectTransform>();
                }
                // 如果不是起點並且是可點擊磚塊
                else if (clickStart && results[0].gameObject.GetComponent<Lv7_Brick>())
                {
                    //print(Vector2.Distance(prevBrick.anchoredPosition, results[0].gameObject.GetComponent<RectTransform>().anchoredPosition));

                    // 判定與前一顆距離是否小於 85 (避免中斷現象)：改變顏色記錄前一個磚塊
                    if (Vector2.Distance(prevBrick.anchoredPosition, results[0].gameObject.GetComponent<RectTransform>().anchoredPosition) < 90)
                    {
                        results[0].gameObject.GetComponent<Image>().color = colorNormal;
                        prevBrick = results[0].gameObject.GetComponent<RectTransform>();

                        // 如果有金幣，金幣增加並刪除金幣播放音效
                        if (results[0].gameObject.GetComponent<Lv7_Brick>().hasCoin)
                        {
                            results[0].gameObject.GetComponent<Lv7_Brick>().hasCoin = false;
                            coinCount++;
                            Destroy(results[0].gameObject.transform.GetChild(0).gameObject);
                            aud.PlayOneShot(soundCoin);
                            ani.SetTrigger("吃到金幣");
                        }
                    }
                }
            }
        }
        // 如果已經按下左鍵並且放開左鍵
        else if (startClick && Input.GetKeyUp(KeyCode.Mouse0))
        {
            // 如果已經開始並且介面數量大於零
            if (clickStart && results.Count > 0)
            {
                // 判定與前一顆距離是否小於 85 (避免中斷現象)
                if (Vector2.Distance(prevBrick.anchoredPosition, results[0].gameObject.GetComponent<RectTransform>().anchoredPosition) < 90)
                {
                    // 如果是終點，就改為一般顏色
                    if (results[0].gameObject.GetComponent<Lv7_Brick>().isEnd)
                    {
                        results[0].gameObject.GetComponent<Image>().color = colorNormal;
                        StartCoroutine(Correct());
                    }
                    // 否則就改為起點顏色
                    else
                    {
                        startClick = false;
                        clickStart = false;
                        results[0].gameObject.GetComponent<Lv7_Brick>().isStart = true;
                        results[0].gameObject.GetComponent<Image>().color = colorStart;
                        results.Clear();
                    }
                }
                // 否則就改為起點顏色
                else
                {
                    prevBrick.GetComponent<Image>().color = colorStart;
                }
            }
        }
    }
}
