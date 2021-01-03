using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Lv9_Fireman : LevelBase
{
    [Header("離正確位置多少單位判定正確")]
    public float distance = 50;

    public static Lv9_Fireman instance;

    /// <summary>
    /// 大聲公上的數字
    /// </summary>
    private Text textLoudlyNumber;
    /// <summary>
    /// 背景：包含所有物件
    /// </summary>
    private RectTransform rectBG;
    /// <summary>
    /// 消防車
    /// </summary>
    private RectTransform rectFiretruck;
    /// <summary>
    /// 消防車右邊位置
    /// </summary>
    private RectTransform rectFiretruckPositionRight;
    /// <summary>
    /// 消防車左邊位置
    /// </summary>
    private RectTransform rectFiretruckPositionLeft;
    /// <summary>
    /// 河馬：角色
    /// </summary>
    private RectTransform rectCharacter;
    /// <summary>
    /// 在消防車上的河馬：角色 - 無裝備
    /// </summary>
    private RectTransform rectCharacterOnFiretruck;
    /// <summary>
    /// 嘿嘿
    /// </summary>
    private GameObject goHey;
    /// <summary>
    /// 群組：房子上的人
    /// </summary>
    private CanvasGroup groupPeople;
    /// <summary>
    /// 圖片：梯子
    /// </summary>
    private Image imgLadder;
    /// <summary>
    /// 消防車
    /// </summary>
    private Lv9_DragObjectFiretruck firetruck;
    /// <summary>
    /// 粒子系統：水柱
    /// </summary>
    private ParticleSystem psWater;
    /// <summary>
    /// 所有裝備清單
    /// </summary>
    private Dictionary<string, bool> equipments = new Dictionary<string, bool>
    {
        { "帽子", false }, { "手套 右手", false }, { "手套 左手", false }, { "衣服", false }, { "鞋子", false }
    };

    protected override void Awake()
    {
        instance = this;

        base.Awake();

        Initialize();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Initialize()
    {
        textLoudlyNumber = GameObject.Find("大聲公上的數字").GetComponent<Text>();
        rectBG = GameObject.Find("背景").GetComponent<RectTransform>();
        rectFiretruck = GameObject.Find("消防車").GetComponent<RectTransform>();
        firetruck = GameObject.Find("消防車").GetComponent<Lv9_DragObjectFiretruck>();
        rectFiretruckPositionRight = GameObject.Find("消防車右邊位置").GetComponent<RectTransform>();
        rectFiretruckPositionLeft = GameObject.Find("消防車左邊位置").GetComponent<RectTransform>();
        rectCharacter = GameObject.Find("角色 - 無裝備").GetComponent<RectTransform>();
        rectCharacterOnFiretruck = GameObject.Find("角色 - 消防車上").GetComponent<RectTransform>();
        goHey = GameObject.Find("嘿嘿物件");
        groupPeople = GameObject.Find("房子上的人").GetComponent<CanvasGroup>();
        psWater = GameObject.Find("水柱").GetComponent<ParticleSystem>();
        imgLadder = GameObject.Find("梯子").GetComponent<Image>();
    }

    /// <summary>
    /// 大聲公正確後
    /// </summary>
    public void LoudlyCorrect()
    {
        StartCoroutine(NumberEffect());
    }

    /// <summary>
    /// 大聲公上的數字效果：大聲公後背景移動到右邊
    /// </summary>
    /// <returns></returns>
    private IEnumerator NumberEffect()
    {
        Vector2 posNumber = textLoudlyNumber.rectTransform.anchoredPosition;
        Vector2 posNumberEnd = posNumber + new Vector2(200, 50);

        while (Vector2.Distance(posNumber, posNumberEnd) > 10)
        {
            posNumber = Vector2.Lerp(posNumber, posNumberEnd, 0.2f);
            textLoudlyNumber.rectTransform.anchoredPosition = posNumber;
            textLoudlyNumber.color += new Color(0, 0, 0, 0.3f);
            yield return new WaitForSeconds(0.05f);
        }

        textLoudlyNumber.rectTransform.anchoredPosition = posNumberEnd;

        yield return new WaitForSeconds(1);
        StartCoroutine(MoveRectToPosition(rectBG, new Vector2(-700, 0)));
    }

    /// <summary>
    /// 移動 Rect 物件到目的地
    /// </summary>
    /// <param name="pos">目的地座標</param>
    private IEnumerator MoveRectToPosition(RectTransform rect, Vector2 pos)
    {
        Vector2 posOriginal = rect.anchoredPosition;

        while (Vector2.Distance(posOriginal, pos) > 5)
        {
            posOriginal = Vector2.Lerp(posOriginal, pos, 0.15f);
            rect.anchoredPosition = posOriginal;
            yield return new WaitForSeconds(0.05f);
        }

        rect.anchoredPosition = pos;
    }

    /// <summary>
    /// 消防員裝備
    /// </summary>
    /// <param name="equip">裝備名稱</param>
    public void FiremanEquipment(string equip)
    {
        equipments[equip] = true;
        var all = equipments.Where(x => x.Value == true);
        if (all.ToList().Count == 5) StartCoroutine(AllEquipmentAndFiretruckIn());
    }

    /// <summary>
    /// 所有裝備都穿上並且消防車進入
    /// </summary>
    /// <returns></returns>
    private IEnumerator AllEquipmentAndFiretruckIn()
    {
        goHey.SetActive(false);     // 隱藏 嘿嘿
        groupPeople.alpha = 1;      // 顯示房子上的人

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoveRectToPosition(rectFiretruck, rectFiretruckPositionRight.anchoredPosition));            // 車子 進入 右邊位置
        // yield return MoveRectToPosition(rectCharacter, rectCharacter.anchoredPosition - Vector2.right * 100);                // 角色走向車子

        rectCharacter.gameObject.SetActive(false);                                                                              // 地上角色隱藏
        rectCharacterOnFiretruck.GetComponent<Image>().color = new Color(1, 1, 1, 1);                                           // 車上角色顯示

        yield return new WaitForSeconds(1);                                                                                     // 中間等待一秒鐘

        StartCoroutine(MoveRectToPosition(rectFiretruck, rectFiretruckPositionLeft.anchoredPosition - Vector2.up * 50));        // 車子 進入 左邊位置

        while (rectFiretruck.localScale.x > 0.8f)                                                                               // 車子縮小
        {
            rectFiretruck.localScale -= Vector3.one * 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);                                                                      // 等待 0.5 秒
        yield return StartCoroutine(MoveRectToPosition(rectBG, new Vector2(700, 0)));                               // 背景回到左邊

        imgLadder.color = new Color(1, 1, 1, 1);                                                                    // 顯示梯子
        yield return StartCoroutine(LadderMove(400, 50));                                                           // 梯子 上升 400

        firetruck.enabled = true;                                                                                   // 啟動消防車功能
    }

    /// <summary>
    /// 移動梯子
    /// </summary>
    /// <param name="move">要移動的單位</param>
    /// <param name="moveStep">每次移動的值</param>
    private IEnumerator LadderMove(float move, float moveStep)
    {
        RectTransform rectLadder = imgLadder.GetComponent<RectTransform>();                                         // 梯子 變形元件
        Vector2 posLadderUp = rectLadder.anchoredPosition + Vector2.up * move;                                      // 梯子 上升座標

        while (Vector2.Distance(rectLadder.anchoredPosition, posLadderUp) > 5)                                      // 梯子 上升
        {
            rectLadder.anchoredPosition += Vector2.up * moveStep;
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 所有人都下樓梯
    /// </summary>
    public IEnumerator AllPeopleDownLadder()
    {
        firetruck.enabled = false;
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(MoveRectToPosition(rectFiretruck, rectFiretruckPositionLeft.anchoredPosition - Vector2.up * 50));       // 車子 回到 左邊位置
        yield return StartCoroutine(LadderMove(-400, -50));                                                                                 // 梯子 下降 400
        imgLadder.color = new Color(1, 1, 1, 0);                                                                                            // 隱藏梯子

        psWater.Play();
    }
}
