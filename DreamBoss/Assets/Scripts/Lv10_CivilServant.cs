using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lv10_CivilServant : LevelBase
{
    [Header("文件上的印章")]
    public Transform traSealOnPaper;
    [Header("印章")]
    public Transform seals;

    /// <summary>
    /// 正確答案
    /// </summary>
    private int answer;

    protected override void Awake()
    {
        base.Awake();

        RandomSealOnPaper();
        SetSealClick();
    }

    /// <summary>
    /// 設定每個印章點下去的事件
    /// </summary>
    private void SetSealClick()
    {
        for (int i = 0; i < seals.childCount; i++)
        {
            int index = i;
            seals.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { ClickSeal(index); });
        }
    }

    /// <summary>
    /// 點擊後的判斷
    /// </summary>
    /// <param name="sealIndex"></param>
    private void ClickSeal(int sealIndex)
    {
        if (sealIndex == answer) StartCoroutine(Correct());
        else StartCoroutine(Wrong());
    }

    /// <summary>
    /// 隨機顯示一個文件上的印章
    /// </summary>
    private void RandomSealOnPaper()
    {
        answer = Random.Range(0, traSealOnPaper.childCount);

        traSealOnPaper.GetChild(answer).gameObject.SetActive(true);
    }

    public override IEnumerator Correct(int index = 0)
    {
        // 文件上的印章恢復原本顏色
        traSealOnPaper.GetChild(answer).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        yield return base.Correct(index);
    }
}
