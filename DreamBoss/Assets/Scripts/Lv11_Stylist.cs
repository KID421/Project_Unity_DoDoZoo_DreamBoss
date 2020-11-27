using UnityEngine;
using System.Linq;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Lv11_Stylist : LevelBase
{
    /// <summary>
    /// 所有飾品
    /// 0 藍色上衣
    /// 1 黃色上衣
    /// 2 水壺
    /// 3 背包
    /// 4 粉色鞋子
    /// 5 咖啡鞋子
    /// 6 頭帶
    /// 7 帽子
    /// 8 裙子
    /// 9 褲子
    /// </summary>
    [Header("所有部位")]
    public RectTransform[] rectParts;

    /// <summary>
    /// 所有飾品正確的位置
    /// 0 藍色上衣
    /// 1 黃色上衣
    /// 2 水壺
    /// 3 背包
    /// 4 粉色鞋子
    /// 5 咖啡鞋子
    /// 6 頭帶
    /// 7 帽子
    /// 8 裙子
    /// 9 褲子
    /// </summary>
    public Vector2[] posCorrectAll =
    {
        new Vector2(-20, -45),
        new Vector2(-20, -39),
        new Vector2(-5, -75),
        new Vector2(-51, -61),
        new Vector2(-19, -290),
        new Vector2(-19, -288),
        new Vector2(-26, 277),
        new Vector2(-5, 269),
        new Vector2(-19, -135),
        new Vector2(-19, -155)
    };
    /// <summary>
    /// 是否選取所有部位：頭飾、上衣、飾品、褲子、鞋子
    /// </summary>
    /// [HideInInspector]
    public bool[] chooseParts = { false, false, false, false, false };

    /// <summary>
    /// 選取的部位編號 
    /// 0 藍色上衣
    /// 1 黃色上衣
    /// 2 水壺
    /// 3 粉色鞋子
    /// 4 咖啡鞋子
    /// 5 頭帶
    /// 6 帽子
    /// 7 裙子
    /// 8 褲子
    /// </summary>
    public int index { get; set; }

    public static Lv11_Stylist instance;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    /// <summary>
    /// 玩家點選了飾品
    /// </summary>
    /// <param name="index">選取飾品的編號：頭飾 0、上衣 1、飾品 2、褲子 3、鞋子 4</param>
    public void ChoosePart(int indexPart)
    {
        if (!chooseParts[indexPart])
        {
            chooseParts[indexPart] = true;
            StartCoroutine(PartMoveToCorrectPosition());
        }
    }

    /// <summary>
    /// 移動到正確位置
    /// </summary>
    /// <returns></returns>
    private IEnumerator PartMoveToCorrectPosition()
    {
        final.transform.SetAsLastSibling();
        final.raycastTarget = true;

        while (Vector2.Distance(rectParts[index].anchoredPosition, posCorrectAll[index]) > 0.3f)
        {
            rectParts[index].anchoredPosition = Vector2.Lerp(rectParts[index].anchoredPosition, posCorrectAll[index], Time.deltaTime * 10);
            yield return null;
        }

        final.transform.SetAsFirstSibling();
        final.raycastTarget = false;

        var truePart = chooseParts.Where(x => x == true);

        if (truePart.ToList().Count == 5) StartCoroutine(Correct());
    }
}
