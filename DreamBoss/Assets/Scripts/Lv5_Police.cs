using UnityEngine;
using UnityEngine.UI;

public class Lv5_Police : LevelBase
{
    [Header("所有物件：車輛與行人 - 共 16 個物件")]
    [Tooltip(
        "貨車右邊、汽車右邊、機車右邊、老年女生右邊、青年女生右邊、青年男生右邊、中年女生右邊、老年男生右邊、" +
        "貨車左邊、汽車左邊、機車左邊、老年男生左邊、青年女生左邊、中年女生左邊、青年男生左邊、老年女生左邊")]
    public GameObject[] allObjects;
    [Header("紅綠燈：左綠黃紅、右綠黃紅")]
    public GameObject[] lights;
    [Header("題目：五題")]
    public PoliceQuestion[] questions;

    protected override void Awake()
    {
        base.Awake();

        ShowQuestion();
    }

    /// <summary>
    /// 顯示題目
    /// </summary>
    private void ShowQuestion()
    {
        int indexQuestion = Random.Range(0, questions.Length);  // 題目

        // 設定車輛與行人
        for (int i = 0; i < allObjects.Length; i++)
        {
            int index = i;

            if (!questions[indexQuestion].carAndPeople[i]) allObjects[i].SetActive(false);
            if (!questions[indexQuestion].leftLight && i < 8) allObjects[i].GetComponent<Button>().onClick.AddListener(() => 
            {
                allObjects[index].GetComponent<Animator>().enabled = true;
                StartCoroutine(Correct()); 
            });
            else if (questions[indexQuestion].leftLight && i >= 8) allObjects[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                allObjects[index].GetComponent<Animator>().enabled = true;
                StartCoroutine(Correct());
            });
            else allObjects[i].GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(Wrong()); });
        }

        // 設定紅綠燈
        for (int i = 0; i < lights.Length; i++)
        {
            // 如果是左邊紅綠燈亮綠燈 就 顯示 左邊綠燈 0 與 右邊紅燈 5
            if (questions[indexQuestion].leftLight && i != 0 && i != 5) lights[i].SetActive(false);
            // 如果是右邊紅綠燈亮綠燈 就 顯示 左邊紅燈 2 與 右邊綠燈 3
            else if (!questions[indexQuestion].leftLight && i != 2 && i != 3) lights[i].SetActive(false);
        }
    } 
}

/// <summary>
/// 警察題目
/// 左邊或右邊紅綠燈亮
/// 左邊與右邊要顯示的車輛與行人
/// </summary>
[System.Serializable]
public struct PoliceQuestion
{
    [Header("紅綠燈：是否為左邊紅綠燈亮綠燈")]
    public bool leftLight;
    [Header("車輛與行人：共 16 個物件 - 0~7 選 3，8~15 選 3")]
    [Tooltip(
        "貨車右邊、汽車右邊、機車右邊、老年女生右邊、青年女生右邊、青年男生右邊、中年女生右邊、老年男生右邊、" +
        "貨車左邊、汽車左邊、機車左邊、老年男生左邊、青年女生左邊、中年女生左邊、青年男生左邊、老年女生左邊")]
    public bool[] carAndPeople;
}
