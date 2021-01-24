using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("登入頁面淡入時間"), Range(20, 30)]
    public float loginFadeTime = 23.5f;

    /// <summary>
    /// 登入頁面
    /// </summary>
    private CanvasGroup groupLigin;
    /// <summary>
    /// 渲染圖片 前導動畫
    /// 包含子物件的登入頁面
    /// </summary>
    private CanvasGroup groupVideoMovie;
    /// <summary>
    /// 略過
    /// </summary>
    private Button btnSkip;
    /// <summary>
    /// Facebook 登錄
    /// </summary>
    private Button btnFB;
    /// <summary>
    /// 遊客登錄
    /// </summary>
    private Button btnNo;
    /// <summary>
    /// 前導動畫
    /// </summary>
    private VideoPlayer videoMovie;

    private void Awake()
    {
        groupLigin = GameObject.Find("登入頁面").GetComponent<CanvasGroup>();
        groupVideoMovie = GameObject.Find("渲染圖片 前導動畫").GetComponent<CanvasGroup>();
        videoMovie = GameObject.Find("前導動畫").GetComponent<VideoPlayer>();
        btnSkip = GameObject.Find("略過").GetComponent<Button>();
        btnSkip.onClick.AddListener(Skip);
        btnFB = GameObject.Find("Facebook 登錄").GetComponent<Button>();
        btnNo = GameObject.Find("遊客登錄").GetComponent<Button>();
        btnFB.onClick.AddListener(FacebookLogin);
        btnNo.onClick.AddListener(NoLogin);
    }

    private void Start()
    {
        StartCoroutine(FadeInLoginIn(loginFadeTime));
    }

    private void Update()
    {
        Loop();
    }

    /// <summary>
    /// 循環
    /// </summary>
    private void Loop()
    {
        if (videoMovie.frame == (long)videoMovie.frameCount - 1)    // frame 為影片影格，如果等於 frameCount 總影格數 - 1 代表跑完
        {
            videoMovie.Stop();                                      // 先停止影片 frame 會變為 -1
            videoMovie.time = 24.2f;                                // 設定時間為 24.2 銜接結尾
            videoMovie.Play();                                      // 播放
        }
    }

    /// <summary>
    /// 略過
    /// </summary>
    private void Skip()
    {
        btnSkip.gameObject.SetActive(false);    // 隱藏略過按鈕
        videoMovie.time = 23.5f;                // 時間快轉到 23.5
        StartCoroutine(FadeInLoginIn(1.5f));    // 淡入登入頁面
    }

    /// <summary>
    /// FB 登錄按鈕
    /// </summary>
    private void FacebookLogin()
    {
        StartCoroutine(CloseVideoMovie());
    }

    /// <summary>
    /// 訪客登錄按鈕
    /// </summary>
    private void NoLogin()
    {
        StartCoroutine(CloseVideoMovie());
    }

    private IEnumerator CloseVideoMovie()
    {
        groupVideoMovie.interactable = false;
        groupVideoMovie.blocksRaycasts = false;

        float a = 1;

        while (a > 0)
        {
            a -= 0.1f;
            groupVideoMovie.alpha = a;
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 登入頁面淡入
    /// </summary>
    /// <param name="showLoginTime">等待多久在淡入</param>
    private IEnumerator FadeInLoginIn(float showLoginTime)
    {
        yield return new WaitForSeconds(showLoginTime);

        btnSkip.gameObject.SetActive(false);                    // 隱藏略過按鈕

        float a = 0;

        while (a < 1)
        {
            a += 0.1f;
            groupLigin.alpha = a;
            yield return new WaitForSeconds(0.05f);
        }

        groupLigin.interactable = true;
        groupLigin.blocksRaycasts = true;
    }
}
