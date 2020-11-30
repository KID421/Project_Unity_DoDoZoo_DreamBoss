using UnityEngine;
using UnityEngine.UI;

public class Lv8_Artist : LevelBase
{
    /// <summary>
    /// 點選的顏色：紅 蘋果, 橙 橘子, 黃 香蕉, 綠 西瓜, 藍 藍莓, 紫 葡萄, 粉 桃子
    /// </summary>
    public int colorCoose { get; set; }

    [Header("紅 蘋果, 橙 橘子, 黃 香蕉, 綠 西瓜, 藍 藍莓, 紫 葡萄, 粉 桃子")]
    public GameObject[] fruites;

    private int index;

    protected override void Awake()
    {
        base.Awake();

        colorCoose = -1;

        index = Random.Range(0, fruites.Length);

        fruites[index].SetActive(true);
    }

    public void ClickPicture()
    {
        if ((int)colorCoose == index)
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
}
