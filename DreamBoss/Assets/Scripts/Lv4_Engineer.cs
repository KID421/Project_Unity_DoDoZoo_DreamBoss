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
        if (questions.Count == 0)
        {
            List<Lv4_Answer> allAnswer = answers.ToList();

            for (int i = 0; i < countQuestion; i++)
            {
                int r = Random.Range(0, allAnswer.Count);
                questions.Add(allAnswer[r]);
                questionsIndex.Add(r);
                allAnswer.RemoveAt(r);
            }
        }

        // indexQuestion = Random.Range(0, answers.Length);
        // countCorrect += answers[indexQuestion].rectAnswers.Length;

        indexQuestion = questionsIndex[0];
        countCorrect = questions[0].rectAnswers.Length;

        objQuestiones.GetChild(indexQuestion).gameObject.SetActive(true);
        objButtons.GetChild(indexQuestion).gameObject.SetActive(true);
    }

    protected override IEnumerator Pass(bool showShare = true)
    {
        if (questions.Count > 1)
        {
            yield return base.Pass(false);
            questions.RemoveAt(0);
            questionsIndex.RemoveAt(0);
            winCount = 0;
            yield return new WaitForSeconds(2);
            Replay();
        }
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