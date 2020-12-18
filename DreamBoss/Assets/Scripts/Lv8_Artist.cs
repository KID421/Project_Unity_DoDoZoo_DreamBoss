using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lv8_Artist : LevelBase
{
    /// <summary>
    /// 點選的顏色：紅 蘋果, 橙 橘子, 黃 香蕉, 綠 西瓜, 藍 藍莓, 紫 葡萄, 粉 桃子
    /// </summary>
    public int colorChoose { get; set; }

    [Header("紅 蘋果, 橙 橘子, 黃 香蕉, 綠 西瓜, 藍 藍莓, 紫 葡萄, 粉 桃子")]
    public GameObject[] fruites;
    [Header("所有顏料")]
    public Button[] btnColors;
    [Header("要掉落的水果：蘋果, 橘子, 香蕉, 西瓜, 藍莓, 葡萄, 桃子")]
    public FruitFall[] fruitFall;
    [Header("畫筆圖片：紅、橙、黃、綠、藍、紫、粉")]
    public Sprite[] sprBrush;
    [Header("畫筆")]
    public GameObject brush;

    private int index;

    protected override void Awake()
    {
        base.Awake();

        colorChoose = -1;

        index = Random.Range(0, fruites.Length);

        fruites[index].SetActive(true);

        SetColorChooseEffect();
    }

    /// <summary>
    /// 設定顏料按鈕按下後的效果
    /// </summary>
    private void SetColorChooseEffect()
    {
        for (int i = 0; i < btnColors.Length; i++)
        {
            int index = i;
            btnColors[index].onClick.AddListener(() => { ColorChooseEffect(index); });
        }
    }

    /// <summary>
    /// 選取顏料後的效果：凸出來
    /// </summary>
    /// <param name="index">每個顏料的編號</param>
    public void ColorChooseEffect(int index)
    {
        btnColors[index].interactable = false;
        btnColors[index].GetComponent<RectTransform>().anchoredPosition += Vector2.right * (index < 4 ? 150 : -150);
    }

    /// <summary>
    /// 點擊圖片
    /// </summary>
    public void ClickPicture()
    {
        if (colorChoose == index)
        {
            fruites[index].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            fruites[index].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            StartCoroutine(Correct());
        }
        else
        {
            StartCoroutine(Wrong());
        }
    }

    public override IEnumerator Correct(int index = 0)
    {
        // 顯示筆刷
        brush.GetComponent<SpriteRenderer>().sprite = sprBrush[this.index];
        // 5 與 6 - 紫色與粉色筆刷旋轉 90 度
        if (this.index > 4) brush.transform.eulerAngles = new Vector3(0, 0, 140);
        brush.SetActive(true);
        
        // 掉落數量
        int r = 5;

        for (int i = 0; i < r; i++)
        {
            int fruitR = Random.Range(0, fruitFall[this.index].fruit.Length);
            //Vector3 pos = new Vector3(Random.Range(-10, 10), 8, 0);   // 隨機
            Vector3 pos = new Vector3(-7 + i * 3.5f, 5 + Random.Range(0f, 3.5f), 0);     // 固定
            Vector3 ang = new Vector3(0, 0, Random.Range(-360, 360));
            Instantiate(fruitFall[this.index].fruit[fruitR], pos, Quaternion.Euler(ang));
        }

        return base.Correct(index);
    }
}

/// <summary>
/// 掉落的水果
/// </summary>
[System.Serializable]
public struct FruitFall
{
    [Header("要掉落的水果")]
    public GameObject[] fruit;
}