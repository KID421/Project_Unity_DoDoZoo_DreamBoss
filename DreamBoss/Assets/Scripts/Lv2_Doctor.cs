using UnityEngine;
using UnityEngine.UI;

public class Lv2_Doctor : LevelBase
{
    [Header("頭部部位：七個 - 頭髮、耳朵、眉毛、眼睛、鼻子、嘴巴、下巴")]
    public GameObject[] partsHead;

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
        indexCurrent = Random.Range(0, partsHead.Length);
        partsHead[indexCurrent].SetActive(false);
    }

    /// <summary>
    /// 點擊部位
    /// 頭部：頭髮 0、耳朵 1、眉毛 2、眼睛 3、鼻子 4、嘴巴 5、下巴 6
    /// </summary>
    /// <param name="index">點擊部位的編號</param>
    public void ClickPart(int index)
    {
        if (index == indexCurrent)
        {
            ShowAllPart();
            StartCoroutine(Win());
        }
        else StartCoroutine(Lose());
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
        for (int i = 0; i < partsHead.Length; i++) partsHead[i].SetActive(true);
    }
}
