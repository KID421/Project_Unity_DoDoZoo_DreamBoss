using UnityEngine;

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

    public static Lv4_Engineer instance;

    /// <summary>
    /// 本次題目的編號
    /// </summary>
    private int indexQuestion;

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
        indexQuestion = Random.Range(0, answers.Length);
        objQuestiones.GetChild(indexQuestion).gameObject.SetActive(true);
        objButtons.GetChild(indexQuestion).gameObject.SetActive(true);

        countCorrect = answers[indexQuestion].rectAnswers.Length;
    }
}

[System.Serializable]
public struct Lv4_Answer
{
    [Header("答案位置")]
    public RectTransform[] rectAnswers;
}