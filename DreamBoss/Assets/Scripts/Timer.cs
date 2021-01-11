using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 計時器：倒數計時版本
/// </summary>
public class Timer : MonoBehaviour
{
    [Header("總時數"), Range(0, 300)]
    public float total = 60;

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

    public delegate void timeStop();
    public event timeStop onTimeStop;

    public static Timer instance;

    public bool stop;

    private void Awake()
    {
        instance = this;
        imgTime = transform.Find("時鐘效果").GetComponent<Image>();
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
        }
        else
        {
            timer -= Time.deltaTime;
            imgTime.fillAmount = timer / total;
            textTime.text = timer.ToString("f0");
        }
    }
}
