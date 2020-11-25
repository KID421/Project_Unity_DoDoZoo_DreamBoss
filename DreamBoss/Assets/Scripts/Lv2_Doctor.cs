using UnityEngine;
using UnityEngine.UI;

public class Lv2_Doctor : LevelBase
{
    [Header("頭部與身體")]
    public GameObject[] parts;
    [Header("頭部部位：六個 - 頭髮、耳朵、眉毛、眼睛、鼻子、嘴巴")]
    public GameObject[] partsHead;
    [Header("身體部位：六個 - 脖子、身體、右手、左手、右腳、左腳")]
    public GameObject[] partsBody;

    /// <summary>
    /// 頭部或身體：頭部 0，身體 1
    /// </summary>
    private int indexPart;
    /// <summary>
    /// 當前的隨機部位編號
    /// </summary>
    private int indexCurrent;

    private void Start()
    {
        Question(0);
    }

    protected override void StartGame()
    {
        base.StartGame();
        RandomPartHide();
    }

    /// <summary>
    /// 隨機部位隱藏
    /// </summary>
    public void RandomPartHide()
    {
        indexPart = Random.Range(0, parts.Length);
        parts[indexPart].SetActive(true);

        indexCurrent = Random.Range(0, indexPart == 0 ? partsHead.Length : partsBody.Length);
        if (indexPart == 0) partsHead[indexCurrent].SetActive(false);
        else if (indexPart == 1) partsBody[indexCurrent].SetActive(false);
    }

    /// <summary>
    /// 點擊部位
    /// 頭部：頭髮 0、耳朵 1、眉毛 2、眼睛 3、鼻子 4、嘴巴 5
    /// 身體：脖子 0、身體 1、右手 2、左手 3、右腳 4、左腳 5
    /// </summary>
    /// <param name="index">點擊部位的編號</param>
    public void ClickPart(int index)
    {
        if (index == indexCurrent)
        {
            ShowAllPart();
            StartCoroutine(Correct());
        }
        else StartCoroutine(Wrong());
    }

    /// <summary>
    /// 點擊部位效果
    /// </summary>
    /// <param name="part">被點擊的部位</param>
    public void ClickPartEffect(GameObject part)
    {
        part.GetComponent<Animator>().SetTrigger("泡泡爆炸");
        part.GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// 顯示所有部位
    /// </summary>
    private void ShowAllPart()
    {
        GameObject[] p = indexPart == 0 ? partsHead : partsBody;
        for (int i = 0; i < partsHead.Length; i++) p[i].SetActive(true);
    }
}
