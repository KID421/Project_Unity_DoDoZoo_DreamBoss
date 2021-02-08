using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PaidManager : MonoBehaviour
{
    /// <summary>
    /// 數字 0 - 9
    /// </summary>
    [Header("題目數字 0 - 9 + -")]
    public Sprite[] questionNumber = new Sprite[12];
    [Header("題目")]
    public PaidQuestion[] paidQuestion;
    [Header("正確音效")]
    public AudioClip soundCorrect;
    [Header("錯誤音效")]
    public AudioClip soundWrong;
    [Header("要解鎖的關卡")]
    public Transform[] tranLevel;
    [Header("八關的鎖")]
    public Button[] btnLock = new Button[8];

    /// <summary>
    /// 是否付費
    /// </summary>
    public static bool paid;
    public static PaidManager instance;

    /// <summary>
    /// 數字按鈕 1 - 9
    /// </summary>
    private Button[] btnNumber = new Button[9];
    /// <summary>
    /// 題目數字 1、數字 2 與符號，答案
    /// </summary>
    private Image questionNumber1, questionNumber2, questionSign, answerNumber;
    /// <summary>
    /// 這次的題目編號
    /// </summary>
    private int indexQuestion;
    /// <summary>
    /// 問券
    /// </summary>
    private CanvasGroup groupQuestion;
    /// <summary>
    /// 群組：解鎖畫面
    /// </summary>
    private CanvasGroup groupPaidPanel;
    /// <summary>
    /// 關閉按鈕
    /// </summary>
    private Button btnClose;

    private AudioSource aud;

    private void Awake()
    {
        instance = this;

        GetObject();
        SetQuestion();

        PlayerPrefs.SetInt("是否購買", 0);
        if (PlayerPrefs.GetInt("是否購買") == 1) PaidAndUnlock();

        if (SceneManager.GetActiveScene().name == "選取關卡") for (int i = 0; i < btnLock.Length; i++) btnLock[i].onClick.AddListener(ClickLock);
    }

    /// <summary>
    /// 點擊鎖
    /// </summary>
    public void ClickLock()
    {
        btnClose.interactable = true;

        groupPaidPanel.alpha = 1;
        groupPaidPanel.interactable = true;
        groupPaidPanel.blocksRaycasts = true;

        groupQuestion.alpha = 0;
        groupQuestion.interactable = false;
        groupQuestion.blocksRaycasts = false;

        SetQuestion();
    }

    /// <summary>
    /// 取得物件
    /// </summary>
    private void GetObject()
    {
        aud = GetComponent<AudioSource>();

        questionNumber1 = GameObject.Find("題目數字 1").GetComponent<Image>();
        questionNumber2 = GameObject.Find("題目數字 2").GetComponent<Image>();
        questionSign = GameObject.Find("題目符號").GetComponent<Image>();
        answerNumber = GameObject.Find("答案").GetComponent<Image>();
        groupQuestion = GameObject.Find("問券").GetComponent<CanvasGroup>();
        groupPaidPanel = GameObject.Find("解鎖畫面").GetComponent<CanvasGroup>();
        btnClose = GameObject.Find("關閉").GetComponent<Button>();

        for (int i = 0; i < btnNumber.Length; i++)
        {
            int index = i + 1;
            btnNumber[i] = GameObject.Find("數字按鈕 " + (i + 1)).GetComponent<Button>();
            btnNumber[i].onClick.AddListener(() => { StartCoroutine(ClickNumberAndCheck(index)); });
        }
    }

    /// <summary>
    /// 設定題目
    /// </summary>
    private void SetQuestion()
    {
        answerNumber.sprite = null;
        indexQuestion = Random.Range(0, paidQuestion.Length);
        PaidQuestion paidQ = paidQuestion[indexQuestion];
        questionNumber1.sprite = questionNumber[paidQ.number1];
        questionSign.sprite = questionNumber[paidQ.sign == "+" ? 10 : 11];
        questionNumber2.sprite = questionNumber[paidQ.number2];
        questionNumber1.SetNativeSize();
        questionNumber2.SetNativeSize();
        questionSign.SetNativeSize();
    }

    /// <summary>
    /// 點擊按鈕並檢查是否正確
    /// </summary>
    /// <param name="clickNumber">點擊按鈕數字</param>
    private IEnumerator ClickNumberAndCheck(int clickNumber)
    {
        PaidQuestion paidQ = paidQuestion[indexQuestion];
        int answer = 0;
        if (paidQ.sign == "+") answer = paidQ.number1 + paidQ.number2;
        else answer = paidQ.number1 - paidQ.number2;

        answerNumber.sprite = questionNumber[clickNumber];
        answerNumber.SetNativeSize();

        if (clickNumber == answer)
        {
            aud.PlayOneShot(soundCorrect);
            btnClose.interactable = false;
            yield return new WaitForSeconds(0.5f);
            groupQuestion.alpha = 1;
            groupQuestion.interactable = true;
            groupQuestion.blocksRaycasts = true;
        }
        else
        {
            aud.PlayOneShot(soundWrong);
            yield return new WaitForSeconds(0.5f);
            answerNumber.sprite = null;
            SetQuestion();
        }
    }

    public void PaidAndUnlock()
    {
        paid = true;

        PlayerPrefs.SetInt("是否購買", 1);  // 1 為已經購買，0 為尚未購買

        Color white = new Color(1, 1, 1, 1);

        /* 暫時關閉解鎖*/

        if (SceneManager.GetActiveScene().name != "選取關卡") return;

        for (int i = 0; i < tranLevel.Length; i++)
        {
            tranLevel[i].GetComponent<Button>().interactable = true;
            tranLevel[i].Find("渲染圖片").GetComponent<RawImage>().color = white;
            tranLevel[i].Find("名稱").GetComponent<Image>().color = white;
            tranLevel[i].GetChild(3).gameObject.SetActive(false);
        }
        /**/
    }
}

[System.Serializable]
public struct PaidQuestion
{
    public int number1;
    public string sign;
    public int number2;
}