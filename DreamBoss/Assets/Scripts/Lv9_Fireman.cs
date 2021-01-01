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
    private RectTransform rectCar;
    /// <summary>
    /// 消防車右邊位置
    /// </summary>
    private RectTransform rectCarPositionRight;
    /// <summary>
    /// 消防車左邊位置
    /// </summary>
    private RectTransform rectCarPositionLeft;
    /// <summary>
    /// 河馬：角色
    /// </summary>
    private RectTransform rectCharacter;
    /// <summary>
    /// 在消防車上的河馬：角色 - 無裝備
    /// </summary>
    private RectTransform rectCharacterOnCar;
    /// <summary>
    /// 嘿嘿
    /// </summary>
    private GameObject goHey;
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
        rectCar = GameObject.Find("消防車").GetComponent<RectTransform>();
        rectCarPositionRight = GameObject.Find("消防車右邊位置").GetComponent<RectTransform>();
        rectCarPositionLeft = GameObject.Find("消防車左邊位置").GetComponent<RectTransform>();
        rectCharacter = GameObject.Find("角色 - 無裝備").GetComponent<RectTransform>();
        rectCharacterOnCar = GameObject.Find("角色 - 消防車上").GetComponent<RectTransform>();
        goHey = GameObject.Find("嘿嘿物件");
    }

    /// <summary>
    /// 大聲公正確後
    /// </summary>
    public void LoudlyCorrect()
    {
        StartCoroutine(NumberEffect());
    }

    /// <summary>
    /// 大聲公上的數字效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator NumberEffect()
    {
        Vector2 posNumber = textLoudlyNumber.rectTransform.anchoredPosition;
        Vector2 posNumberEnd = posNumber + new Vector2(350, 80);

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
        if (all.ToList().Count == 5) StartCoroutine(AllEquipmentAndCarIn());
    }

    /// <summary>
    /// 所有裝備都穿上並且消防車進入
    /// </summary>
    /// <returns></returns>
    private IEnumerator AllEquipmentAndCarIn()
    {
        goHey.SetActive(false);

        yield return StartCoroutine(MoveRectToPosition(rectCar, rectCarPositionRight.anchoredPosition));

        yield return MoveRectToPosition(rectCharacter, rectCharacter.anchoredPosition - Vector2.right * 200);

        rectCharacter.gameObject.SetActive(false);
        rectCharacterOnCar.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(1);

        StartCoroutine(MoveRectToPosition(rectCar, rectCarPositionLeft.anchoredPosition));
        
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoveRectToPosition(rectBG, new Vector2(700, 0)));
    }
}
