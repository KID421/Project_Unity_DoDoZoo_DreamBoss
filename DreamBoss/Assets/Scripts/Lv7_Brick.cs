using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lv7_Brick : MonoBehaviour
{
    [Header("是否為起點")]
    public bool isStart;
    [Header("是否為終點")]
    public bool isEnd;
    [Header("是否有金幣")]
    public bool hasCoin;
    
    /// <summary>
    /// 金幣預製物 - 透過 Resources Load
    /// </summary>
    private GameObject coin;

    private void Awake()
    {
        CreateCoin();
        SettingEnd();
    }

    /// <summary>
    /// 生成金幣
    /// </summary>
    private void CreateCoin()
    {
        if (hasCoin)
        {
            coin = Resources.Load<GameObject>("金幣");
            GameObject temp = Instantiate(coin, transform);
            temp.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    /// <summary>
    /// 設定終點：
    /// 終點區塊
    /// 終點旗子
    /// </summary>
    private void SettingEnd()
    {
        if (isEnd)
        {
            RectTransform rect = GetComponent<RectTransform>();
            StartCoroutine(CreateEndFlag(rect.anchoredPosition));
        }
    }

    /// <summary>
    /// 生成終點旗子
    /// </summary>
    /// <param name="pos"></param>
    private IEnumerator CreateEndFlag(Vector2 pos)
    {
        // 生成旗子在畫布裡面
        GameObject flag = Resources.Load<GameObject>("終點旗子");
        GameObject objFlag = Instantiate(flag, GameObject.Find("畫布").transform);

        // 設定旗子在終點上方 600
        RectTransform rect = objFlag.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(pos.x, 600);

        // 以 1000 的速度往終點飛過去
        while (Vector2.Distance(rect.anchoredPosition, pos) > 50)
        {
            rect.anchoredPosition -= Vector2.up * Time.deltaTime * 1000;
            yield return null;
        }

        rect.anchoredPosition = pos;

        // 設定為終點圖樣
        object[] sprites = Resources.LoadAll("北極熊_素材");
        GetComponent<Image>().sprite = (Sprite)sprites[4];
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
        GetComponent<RectTransform>().sizeDelta = new Vector2(85, 95);
    }
}
