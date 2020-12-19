using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Lv4_Engineer : LevelBase
{
    [Header("答案")]
    public Lv4_Answer[] answers;
    [Header("判定距離：小於此距離就算判定成功")]
    public float distance = 50f;
    [Header("題目")]
    public Transform objQuestiones;
    [Header("按鈕")]
    public Transform objButtons;
    [Header("要玩幾道題目")]
    public int countQuestion = 5;

    public static Lv4_Engineer instance;

    /// <summary>
    /// 題目
    /// </summary>
    public static List<Lv4_Answer> questions = new List<Lv4_Answer>();
    /// <summary>
    /// 題目的編號
    /// </summary>
    public static List<int> questionsIndex = new List<int>();

    /// <summary>
    /// 本次題目的編號
    /// </summary>
    private int indexQuestion;

    public List<Lv4_Answer> test = new List<Lv4_Answer>();

    protected override void Awake()
    {
        base.Awake();

        instance = this;

        SetQuestion();
    }

    /// <summary>
    /// 設定題目
    /// </summary>
    private void SetQuestion()
    {
        // 如果 題目數量 為零 就初始化題目
        if (questions.Count == 0)
        {
            // 所有答案：用來抽取並設定每題不同用
            List<Lv4_Answer> allAnswer = answers.ToList();
            // 所有答案：用來配對編號用
            List<Lv4_Answer> tempAnswers = answers.ToList();

            // 執行題數：預設為 5 次
            for (int i = 0; i < countQuestion; i++)
            {
                // 隨機抽題並加入題庫
                int r = Random.Range(0, allAnswer.Count);
                questions.Add(allAnswer[r]);

                // 匹配當前題目編號
                for (int j = 0; j < tempAnswers.Count; j++) if (questions[i].Equals(tempAnswers[j])) questionsIndex.Add(j);

                // 刪除用來抽取用清單
                allAnswer.RemoveAt(r);
            }
        }

        // 取得題目編號與正確數量：根據每一提的按鈕而定
        indexQuestion = questionsIndex[0];
        countCorrect = questions[0].rectAnswers.Length;

        // 顯示題目與按鈕
        objQuestiones.GetChild(indexQuestion).gameObject.SetActive(true);
        objButtons.GetChild(indexQuestion).gameObject.SetActive(true);
    }

    protected override IEnumerator Pass(bool showShare = true)
    {
        // 如果題目還有一題以上就刪除當前題目並歸零正確數量與重新遊戲
        if (questions.Count > 1)
        {
            yield return base.Pass(false);
            questions.RemoveAt(0);
            questionsIndex.RemoveAt(0);
            winCount = 0;
            yield return new WaitForSeconds(2);
            Replay();
        }
        // 否則就過關，顯示分享畫面
        else
        {
            yield return base.Pass(true);
        }
    }
}

[System.Serializable]
public struct Lv4_Answer
{
    [Header("答案位置")]
    public RectTransform[] rectAnswers;
}