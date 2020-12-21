using UnityEngine;
using System.Collections;

public class Lv3_Scientist : LevelBase
{
    [Header("細胞")]
    public GameObject[] cells;

    /// <summary>
    /// 生成的細胞總數
    /// </summary>
    private int totalCreateCell;

    public static Lv3_Scientist instance;

    protected override void Awake()
    {
        base.Awake();

        instance = this;

        CreateCell();
    }

    /// <summary>
    /// 隨機生成細胞在畫布裡面，座標：165，250
    /// </summary>
    private void CreateCell()
    {
        // 如果數量已經達到 16 個
        if (totalCreateCell == 16)
        {
            // 如果試管並沒有判定過關，就撥放錯誤動畫、音效並且兩秒後重來
            if (!Lv3_TestTube.pass)
            {
                ani.SetTrigger("錯誤");
                aud.PlayOneShot(soundWrong);
                Invoke("Replay", 2);
            }
            // 跳出不生成第 17 顆
            return;
        }

        totalCreateCell++;
        int r = Random.Range(0, cells.Length);
        GameObject tempCell = Instantiate(cells[r], GameObject.Find("畫布").transform);
        tempCell.GetComponent<RectTransform>().anchoredPosition = new Vector2(165, 250);
    }

    public override IEnumerator Correct(int index = 0)
    {
        yield return base.Correct(index);

        // 正確後 產生新的細胞
        CreateCell();
    }

    public override IEnumerator Wrong()
    {
        yield return base.Wrong();

        // 錯誤後 產生新的細胞
        CreateCell();
    }
}
