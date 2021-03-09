using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 計時器：倒數計時版本
/// </summary>
public class Timer : MonoBehaviour
{
    [Header("總時數"), Range(0, 300)]
    public float total = 60;
    [Header("倒數顏色")]
    public Color colorCountDown;
    [Header("倒數音效")]
    public AudioClip soundCount;
    [Header("是否需要倒數十秒的事件")]
    public bool needLessTenEvent;

    /// <summary>
    /// 圖片時間：時鐘效果 - 360 用
    /// </summary>
    private Image imgTime;
    /// <summary>
    /// 文字時間
    /// </summary>
    private Text textTime;
    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;
    /// <summary>
    /// 時鐘中心
    /// </summary>
    private Image imgCenter;
    /// <summary>
    /// 時鐘邊框
    /// </summary>
    private Image imgBorder;
    /// <summary>
    /// 剩餘十秒鐘呼叫一次
    /// </summary>
    private bool callTimeLessTen;
    /// <summary>
    /// 音源
    /// </summary>
    private AudioSource aud;

    public delegate void timeStop();
    public event timeStop onTimeStop;
    public delegate void timeLessTen();
    public event timeLessTen onTimeLessTen;

    public static Timer instance;

    public bool stop;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        instance = this;
        imgTime = transform.Find("時鐘效果").GetComponent<Image>();
        imgCenter = transform.Find("時鐘中心").GetComponent<Image>();
        imgBorder = transform.Find("時鐘邊框").GetComponent<Image>();
        textTime = transform.Find("時鐘秒數").GetComponent<Text>();
        timer = total;
    }

    private void Update()
    {
        TimerStart();
    }

    /// <summary>
    /// 計時器開始：倒數計時
    /// </summary>
    private void TimerStart()
    {
        if (stop) return;

        if (timer <= 0)
        {
            timer = 0;
            stop = true;
            onTimeStop();
            aud.Stop();
        }
        else
        {
            timer -= Time.deltaTime;
            imgTime.fillAmount = timer / total;
            textTime.text = timer.ToString("f0");

            if (timer > 10.3f && timer < 10.5f)
            {
                imgCenter.color = colorCountDown;
                imgBorder.color = colorCountDown;

                if (!callTimeLessTen)
                {
                    callTimeLessTen = true;
                    if (needLessTenEvent) onTimeLessTen();

                    aud.clip = soundCount;
                    aud.Play();
                }
            }
        }
    }
}
