using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

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
    private Vector2[] posCorrectAllOutOfSchool =
    {
        new Vector2(-20, -45),
        new Vector2(-24, -39),
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
    /// 帽子制服
    /// 帽子運動
    /// 褲子運動
    /// 褲子制服
    /// 衣服制服
    /// 衣服運動
    /// 書包運動
    /// 書包制服
    /// 襪子
    /// 鞋子運動
    /// 鞋子制服
    /// </summary>
    private Vector2[] posCorrectAllInSchool =
    {
        new Vector2(-30, 260),
        new Vector2(5, 270),
        new Vector2(-18, -150),
        new Vector2(-18, -140),
        new Vector2(-19, -45),
        new Vector2(-20, -46),
        new Vector2(0, -84),
        new Vector2(-54, -73),
        new Vector2(-20, -270),
        new Vector2(-18, -286),
        new Vector2(-19, -290)
    };

    /// <summary>
    /// 正確位置：兩組
    /// </summary>
    public DressPosition[] dressPositions;

    /// <summary>
    /// 目前選取的場景
    /// 0 戶外
    /// 1 學校
    /// </summary>
    public int indexCurrent;

    /// <summary>
    /// 是否選取所有部位：頭飾、上衣、飾品、褲子、鞋子
    /// </summary>
    [HideInInspector]
    public bool[] chooseParts = { false, false, false, false, false };
    [Header("判定距離：小於此距離就算判定成功")]
    public float distance = 30f;
    [Header("小女孩")]
    public Animator aniGirl;
    [Header("切換學校與戶外按鈕")]
    public Button btnSwitch;
    [Header("學校")]
    public GameObject[] dressSchool;
    [Header("戶外")]
    public GameObject[] dressOutside;

    /// <summary>
    /// 是否在學校內
    /// </summary>
    private bool inSchool;

    /// <summary>
    /// 小女孩拍照位置
    /// </summary>
    private Transform traPictureParent;

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

        dressPositions[0].posCorrectAll = posCorrectAllOutOfSchool;
        dressPositions[1].posCorrectAll = posCorrectAllInSchool;

        instance = this;
        traPictureParent = GameObject.Find("小女孩拍照位置").transform;
        
        btnSwitch.onClick.AddListener(SwitchSchool);                                // 切換學校戶外按鈕點擊設定
    }

    /// <summary>
    /// 切換學校與戶外
    /// </summary>
    private void SwitchSchool()
    {
        inSchool = !inSchool;

        if (inSchool)
        {
            indexCurrent = 1;
            btnSwitch.transform.Find("圓圈").GetComponent<RectTransform>().anchoredPosition = new Vector2(65, 0);
            btnSwitch.transform.Find("有顏色底圖").GetComponent<Image>().color = new Color(0.5f, 0.8f, 0.3f);

            for (int i = 0; i < dressSchool.Length; i++) dressSchool[i].SetActive(true);
            for (int i = 0; i < dressOutside.Length; i++) dressOutside[i].SetActive(false);
        }
        else
        {
            indexCurrent = 0;
            btnSwitch.transform.Find("圓圈").GetComponent<RectTransform>().anchoredPosition = new Vector2(-65, 0);
            btnSwitch.transform.Find("有顏色底圖").GetComponent<Image>().color = new Color(0.8f, 0.2f, 0.3f);

            for (int i = 0; i < dressSchool.Length; i++) dressSchool[i].SetActive(false);
            for (int i = 0; i < dressOutside.Length; i++) dressOutside[i].SetActive(true);
        }
    }

    /// <summary>
    /// 分享按鈕
    /// </summary>
    public void ButtonCameraClickToShare()
    {
        StartCoroutine(Pass());
        StartCoroutine(MoveToPicturePosition());
    }

    /// <summary>
    /// 所有 刺蝟小女孩拍照 標籤物件移動到分享畫面
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveToPicturePosition()
    {
        yield return new WaitForSeconds(2f);

        //GameObject[] girl = GameObject.FindGameObjectsWithTag("刺蝟小女孩拍照");
        //for (int i = 0; i < girl.Length; i++)
        //{
        //    girl[i].transform.SetParent(traPictureParent); 
        //}

        dressSchool[0].transform.SetParent(traPictureParent);
        GameObject.Find("渲染圖片 小女孩").transform.SetParent(traPictureParent);
        dressSchool[1].transform.SetParent(traPictureParent);
        dressOutside[0].transform.SetParent(traPictureParent);

        for (int i = 0; i < traPictureParent.childCount; i++)
        {
            Transform child = traPictureParent.GetChild(i);
            if (child.name == "帽子 制服" && child.tag != "刺蝟小女孩拍照") child.gameObject.SetActive(false);

            if (child.name == "戶外" || child.name == "學校")
            {
                for (int j = 0; j < child.childCount; j++)
                {
                    if (child.GetChild(j).tag != "刺蝟小女孩拍照")
                    {
                        child.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }

        traPictureParent.localScale = Vector3.one * 0.5f;
        traPictureParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, 35);

        for (int i = 0; i < traPictureParent.childCount; i++)
        {
            if (traPictureParent.GetChild(i).name == "帽子 制服") traPictureParent.GetChild(i).SetAsFirstSibling();
        }
    }

    /* 舊模式
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
    */
}

/// <summary>
/// 飾品正確位置
/// </summary>
[System.Serializable]
public struct DressPosition
{
    public Vector2[] posCorrectAll;
}