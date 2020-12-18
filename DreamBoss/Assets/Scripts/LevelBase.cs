using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class LevelBase : MonoBehaviour
{
    [Header("互動物件")]
    public Transform interactable;
    [Header("結束畫面")]
    public Image final;
    [Header("正確與錯誤音效")]
    public AudioClip soundCorrect;
    public AudioClip soundWrong;
    [Header("正確特效")]
    public RectTransform correctRect;
    public ParticleSystem correctParticle;
    [Header("是否需要倒數")]
    public bool needCount;
    [Header("倒數時間")]
    public float countTime = 8;
    [Header("全對特效")]
    public ParticleSystem allCorrectParticle;
    [Header("角色動畫控制器")]
    public Animator ani;
    [Header("正確後停留時間")]
    public float timeCorrect = 2;
    [Header("錯誤後停留時間")]
    public float timeWrong = 2;
    [Header("過關後要播放的動畫參數")]
    public string aniPass = "過關";
    [Header("是否需要正確後重新載入遊戲")]
    public bool afterCorrectReplay = true;
    [Header("是否需要錯誤後重新載入遊戲")]
    public bool afterWrongReplay = true;
    [Header("是否需要顯示正確物件")]
    public bool needShowCorrectObject;
    [Header("正確次數：要正確幾次才會過關")]
    public int countCorrect = 5;
    [Header("分享畫面圖片：預設皆為一張")]
    public Sprite[] sprShares;

    protected Transform canvas;
    protected AudioSource aud;
    protected float timer = 0;
    /// <summary>
    /// 分享畫面群組元件
    /// </summary>
    protected CanvasGroup groupShare;
    /// <summary>
    /// 分享畫面圖片
    /// </summary>
    protected Image imgShare;

    public static int winCount;

    protected virtual void Awake()
    {
        canvas = GameObject.Find("畫布").transform;
        aud = GetComponent<AudioSource>();

        // KID 2020.12.16
        // 取得過關分享畫面 
        groupShare = GameObject.Find("過關分享畫面群組").GetComponent<CanvasGroup>();
        imgShare = GameObject.Find("過關分享畫面").GetComponent<Image>();
        // KID --

        InteractableSwitch(false);
    }

    protected virtual void Update()
    {
        if (needCount) TimeCount();
    }

    /// <summary>
    /// 轉換互動物件
    /// </summary>
    /// <param name="interactableSwitch">能否互動</param>
    private void InteractableSwitch(bool interactableSwitch)
    {
        for (int i = 0; i < interactable.childCount; i++)
            interactable.GetChild(i).GetComponent<Button>().interactable = interactableSwitch;
    }

    /// <summary>
    /// 問題
    /// </summary>
    /// <param name="delayStart">延遲開始</param>
    protected virtual IEnumerator Question(float delayStart)
    {
        yield return null;
        Invoke("StartGame", delayStart);
    }

    /// <summary>
    /// 開始遊戲：啟動互動
    /// </summary>
    protected virtual void StartGame()
    {
        InteractableSwitch(true);
    }

    /// <summary>
    /// 正確
    /// </summary>
    public virtual IEnumerator Correct(int index = 0)
    {
        ani.SetTrigger("正確");
        winCount++;
        aud.PlayOneShot(soundCorrect);
        if (correctParticle) correctParticle.Play();
        if (needShowCorrectObject) StartCoroutine(ShowCorrectObject(index));

        if (afterCorrectReplay)
        {
            final.raycastTarget = true;
            final.transform.SetAsLastSibling();

            yield return new WaitForSeconds(timeCorrect);

            if (winCount < countCorrect) Replay();
            else StartCoroutine(WinPanel());
        }
        else
        {
            if (winCount == countCorrect) StartCoroutine(WinPanel());
        }
    }

    /// <summary>
    /// 顯示正確的物件
    /// </summary>
    /// <param name="index">正確物件編號</param>
    private IEnumerator ShowCorrectObject(int index)
    {
        RectTransform rect = interactable.GetChild(index).GetComponent<RectTransform>();
        rect.SetParent(canvas);
        rect.GetComponent<Image>().raycastTarget = false;
        if (correctRect) correctRect.SetAsLastSibling();

        while (rect.anchoredPosition != new Vector2(0, 120))
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(0, 120), 10 * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// 錯誤
    /// </summary>
    public virtual IEnumerator Wrong()
    {
        ani.SetTrigger("錯誤");
        aud.PlayOneShot(soundWrong, 2);

        if (afterWrongReplay)
        {
            final.raycastTarget = true;
            final.transform.SetAsLastSibling();

            yield return new WaitForSeconds(timeWrong);
            Replay();
        }
    }

    /// <summary>
    /// 重新開始
    /// </summary>
    private void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 勝利
    /// </summary>
    private IEnumerator WinPanel()
    {
        ani.SetTrigger(aniPass);
        allCorrectParticle.Play();

        // 等待兩秒後顯示分享畫面
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(ShowShare());

        // 等待一秒後顯示離開場景過場動畫
        //yield return new WaitForSeconds(1);
        //LevelManager.instance.LeaveLevel();

        // 等待一秒後回到選取關卡
        //yield return new WaitForSeconds(1);
        //SceneManager.LoadScene("選取關卡");
    }


    /// <summary>
    /// 顯示分享畫面
    /// </summary>
    private IEnumerator ShowShare()
    {
        imgShare.sprite = sprShares[0];
        groupShare.transform.SetAsLastSibling();

        while (groupShare.alpha < 1)
        {
            groupShare.alpha += 1 * Time.deltaTime;
            yield return null;
        }

        groupShare.interactable = true;
        groupShare.blocksRaycasts = true;
    }

    /// <summary>
    /// 倒數計時
    /// </summary>
    protected void TimeCount()
    {
        if (timer >= countTime && needCount)
        {
            needCount = false;
            TimeStop();
        }
        else timer += Time.deltaTime;
    }

    /// <summary>
    /// 時間倒數完畢
    /// </summary>
    protected virtual void TimeStop()
    {

    }
}
