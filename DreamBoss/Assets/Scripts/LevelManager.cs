using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 載入關卡管理器與音效管理
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("按鈕音效")]
    public AudioClip soundButton;
    [Header("載入場景延遲時間")]
    public float delayLoadScene = 0.8f;
    [Header("每一關的按鈕")]
    public Button[] buttons;
    [Header("水平捲動")]
    public Scrollbar barHorizontal;

    /// <summary>
    /// 要載入的場景名稱
    /// </summary>
    private string nameScene;
    /// <summary>
    /// 轉場動畫控制器
    /// </summary>
    private Animator aniCrossImage;
    /// <summary>
    /// 返回按鈕：在關卡裡面的，選取關卡的不用
    /// </summary>
    private Button btnBack;
    /// <summary>
    /// 分享畫面內的返回選單
    /// </summary>
    private Button btnShareBackToMenu;
    /// <summary>
    /// 分享畫面內的重新遊戲
    /// </summary>
    private Button btnShareReplay;

    private AudioSource aud;

    public static LevelManager instance;

    /// <summary>
    /// 水平捲動的值
    /// </summary>
    public static float valueBarHorizontal;

    /// <summary>
    /// 是否為選單
    /// </summary>
    private bool isMenu
    {
        get
        {
            return SceneManager.GetActiveScene().name == "選單";
        }
    }

    /// <summary>
    /// 是否為選取關卡
    /// </summary>
    private bool isSelection
    {
        get
        {
            return SceneManager.GetActiveScene().name == "選取關卡";
        }
    }

    private void Awake()
    {
        instance = this;

        aud = GetComponent<AudioSource>();

        if (!isMenu) aniCrossImage = GameObject.Find("轉場動畫").GetComponent<Animator>();

        SetAllButtonClickEvent();
        SetBackButtonInLevel();
        ButtonInLeveleSharePanel();
    }

    private void Start()
    {
        BarHorizontalSetToPlayLevel("");        // 進入選取關卡後更新水平捲動值 - 停在上次位置
    }

    /// <summary>
    /// 在關卡分享畫面內的按鈕
    /// </summary>
    private void ButtonInLeveleSharePanel()
    {
        if (!isMenu && !isSelection)
        {
            btnShareBackToMenu = GameObject.Find("分享畫面返回選單").GetComponent<Button>();
            btnShareReplay = GameObject.Find("分享畫面重新遊戲").GetComponent<Button>();

            btnShareBackToMenu.onClick.AddListener(() => { StartCoroutine(BackToMenu()); });
            btnShareReplay.onClick.AddListener(() => { StartCoroutine(ReplayLevel()); });
        }
    }

    /// <summary>
    /// 返回選單
    /// </summary>
    private IEnumerator BackToMenu()
    {
        LeaveLevel();

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("選單");
    }

    /// <summary>
    /// 重新體驗關卡
    /// </summary>
    private IEnumerator ReplayLevel()
    {
        aniCrossImage.SetTrigger("進入場景");

        // 正確次數歸零
        LevelBase.winCount = 0;

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 設定在關卡內的返回按鈕：播放離開場景轉場動畫
    /// </summary>
    private void SetBackButtonInLevel()
    {
        if (!isMenu && !isSelection)
        {
            btnBack = GameObject.Find("關卡_返回按鈕").GetComponent<Button>();
            btnBack.onClick.AddListener(LeaveLevel);
        }
    }

    /// <summary>
    /// 離開關卡的轉場動畫：離開場景
    /// </summary>
    public void LeaveLevel()
    {
        aniCrossImage.SetTrigger("離開場景");
    }

    /// <summary>
    /// 設定所有按鈕點擊事件
    /// </summary>
    private void SetAllButtonClickEvent()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => { aniCrossImage.SetTrigger("進入場景"); });
        }
    }

    /// <summary>
    /// 延遲載入場景並播放音效
    /// </summary>
    /// <param name="delay"></param>
    public void DelayLoadScene(string nameScene)
    {
        aud.PlayOneShot(soundButton);
        this.nameScene = nameScene;
        Invoke("LoadScene", delayLoadScene);

        BarHorizontalSetToPlayLevel(nameScene);     // 載入時紀錄水平捲動的值
    }

    /// <summary>
    /// 水平卷軸設定為剛玩完的關卡
    /// </summary>
    private void BarHorizontalSetToPlayLevel(string nameScene)
    {
        // 如果不是選關卡 跳出不處理 - 避免沒有水平捲動 錯誤
        if (!isSelection) return;

        // 如果 不是選取關卡 並且 不是空值
        if (nameScene != "選取關卡" && nameScene != "")
        {
            barHorizontal = GameObject.Find("水平捲動").GetComponent<Scrollbar>();  // 取得水平捲動
            valueBarHorizontal = barHorizontal.value;                              // 保存當前水平捲動的值
        }
        // 否則 - 在選取關卡內時
        else
        {
            barHorizontal = GameObject.Find("水平捲動").GetComponent<Scrollbar>();  // 取得水平捲動
            barHorizontal.value = valueBarHorizontal;                              // 設定水平捲動的值
        }
    }

    /// <summary>
    /// 延遲離開遊戲並播放音效
    /// </summary>
    public void DelayQuit()
    {
        aud.PlayOneShot(soundButton);
        Invoke("Quit", delayLoadScene);
    }

    /// <summary>
    /// 載入場景
    /// </summary>
    private void LoadScene()
    {
        LevelBase.winCount = 0;
        SceneManager.LoadScene(nameScene);
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    private void Quit()
    {
        Application.Quit();
    }
}
