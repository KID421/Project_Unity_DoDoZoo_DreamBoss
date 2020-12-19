using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
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
    /// 是否點到終點
    /// </summary>
    private bool clickEnd;
    /// <summary>
    /// 點擊的介面
    /// </summary>
    private List<RaycastResult> results;
    /// <summary>
    /// 前一顆磚塊
    /// </summary>
    private RectTransform prevBrick;
    /// <summary>
    /// 倒數計時的全部時間
    /// </summary>
    private float timeTotal;

    [Header("起點顏色")]
    public Color colorStart;
    [Header("滑過顏色")]
    public Color colorNormal;
    [Header("吃金幣音效")]
    public AudioClip soundCoin;
    [Header("關卡")]
    public GameObject[] level;
    [Header("北極熊旗子")]
    public GameObject bear;
    [Header("吃到金幣特效")]
    public ParticleSystem psCoin;
    [Header("金幣文字")]
    public Text textCoin;
    [Header("時間文字")]
    public Text textTime;

    public static int coinCount;

    /// <summary>
    /// 生成後的小北極熊
    /// </summary>
    private RectTransform bearSmall;

    protected override void Awake()
    {
        base.Awake();

        level[Random.Range(0, level.Length)].SetActive(true);

        SetBear();

        // 指定全部時間並更新金幣介面
        timeTotal = countTime;
        textCoin.text = coinCount + "";
    }

    protected override void Update()
    {
        base.Update();

        Mouse();
    }

    /// <summary>
    /// 設定北極熊小隻的
    /// </summary>
    private void SetBear()
    {
        // 取得所有方塊
        var bricks = FindObjectsOfType<Lv7_Brick>();

        for (int i = 0; i < bricks.Length; i++)
        {
            if (bricks[i].isStart)
            {
                //GameObject tempBear = Instantiate(bear, bricks[i].transform);
                GameObject tempBear = Instantiate(bear, GameObject.Find("畫布").transform);
                tempBear.GetComponent<RectTransform>().anchoredPosition = bricks[i].GetComponent<RectTransform>().anchoredPosition;
                bearSmall = tempBear.GetComponent<RectTransform>();
                return;
            }
        }
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

            if (!results[0].gameObject.GetComponent<Lv7_Brick>()) return;

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

                        // -- KID 2020.12.18 添加 小北極熊跟著走
                        //bearSmall.SetParent(results[0].gameObject.transform);
                        //bearSmall.anchoredPosition = Vector3.zero;
                        bearSmall.anchoredPosition = results[0].gameObject.GetComponent<RectTransform>().anchoredPosition;
                        // -- KID

                        // 如果有金幣，金幣增加並刪除金幣播放音效
                        if (results[0].gameObject.GetComponent<Lv7_Brick>().hasCoin)
                        {
                            results[0].gameObject.GetComponent<Lv7_Brick>().hasCoin = false;
                            coinCount++;
                            Destroy(results[0].gameObject.transform.GetChild(0).gameObject);
                            aud.PlayOneShot(soundCoin);
                            ani.SetTrigger("吃到金幣");

                            // -- KID 2020.12.18 添加 小北極熊金幣特效 並 更新金幣介面
                            psCoin.Play();
                            textCoin.text = coinCount + "";
                            // -- KID
                        }

                        // 如果是終點，就改為一般顏色
                        if (results[0].gameObject.GetComponent<Lv7_Brick>().isEnd)
                        {
                            clickEnd = true;
                            results[0].gameObject.GetComponent<Image>().color = colorNormal;
                            StartCoroutine(Correct());
                            // 停止倒數
                            needCount = false;
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
                        clickEnd = true;
                        print(123);
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
                else if (!clickEnd)
                {
                    prevBrick.GetComponent<Image>().color = colorStart;
                }
            }
        }
    }

    protected override void TimeCount()
    {
        base.TimeCount();

        if (timeTotal > 0)
        {
            timeTotal -= Time.deltaTime;
            textTime.text = timeTotal.ToString("F2");
        }
        else
        {
            textTime.text = 0 + "";
        }
    }

    protected override void TimeStop()
    {
        base.TimeStop();

        StartCoroutine(Correct());
    }

    protected override IEnumerator Pass(bool showShare = true)
    {
        if (coinCount >= 20) indexSharePicture = 0;
        else if (coinCount >= 15) indexSharePicture = 1;
        else if (coinCount >= 10) indexSharePicture = 2;
        else indexSharePicture = 3;
        
        yield return base.Pass();
    }
}
