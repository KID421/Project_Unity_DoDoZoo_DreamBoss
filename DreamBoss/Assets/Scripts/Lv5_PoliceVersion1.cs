using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lv5_PoliceVersion1 : LevelBase
{
    [Header("紅綠燈：左綠黃紅、右綠黃紅")]
    public GameObject[] lights;
    [Header("闖紅燈左邊與右邊")]
    public GameObject rectRedLeft;
    public GameObject rectRedRight;
    [Header("闖紅燈出現機率"), Range(0f, 1f)]
    public float redPercent = 1;

    public static Lv5_PoliceVersion1 instance;

    /// <summary>
    /// 是否點擊第一次
    /// </summary>
    public bool click;

    /// <summary>
    /// 右邊綠燈是否亮
    /// </summary>
    private bool rightLight = true;
    /// <summary>
    /// 按鈕：按鈕上方
    /// </summary>
    private Button btnClick;
    /// <summary>
    /// 分數
    /// </summary>
    private int score;
    /// <summary>
    /// 介面：分數
    /// </summary>
    private Text textScore;

    public delegate void SwitchLight(bool right);
    public event SwitchLight onSwitchLight;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
        textScore = GameObject.Find("分數").GetComponent<Text>();

        btnClick = GameObject.Find("按鈕上方").GetComponent<Button>();
        btnClick.onClick.AddListener(() => 
        {
            // 如果 右邊綠燈 並且 右邊所有人都通過 或者 左邊綠燈 並且 左邊所有人都通過 才能切換燈號
            if ((rightLight && Lv5_People.passRightAll) || (!rightLight && Lv5_People.passLeftAll)) StartCoroutine(ButtonEffect());
        });
    }

    private void Start()
    {
        StartCoroutine(SetLight());

        Timer.instance.onTimeStop += TimerStop;
        Timer.instance.onTimeLessTen += TimeLessTen;
    }

    /// <summary>
    /// 時間小於十秒
    /// </summary>
    private void TimeLessTen()
    {
        float r = Random.Range(0f, 1f);

        if (r <= redPercent)
        {
            if (!rightLight) rectRedRight.SetActive(true);
            else rectRedLeft.SetActive(true);
        }
    }

    /// <summary>
    /// 時間停止
    /// </summary>
    private void TimerStop()
    {
        StartCoroutine(Pass());
    }

    /// <summary>
    /// 切換紅綠燈
    /// </summary>
    private IEnumerator SetLight()
    {
        rightLight = !rightLight;

        // 設定紅綠燈
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }

        // 如果左邊是綠燈
        // 左邊 紅燈 2 > 綠燈 0
        // 右邊 黃燈 4 > 紅燈 5
        if (!rightLight)
        {
            lights[2].SetActive(true);
            lights[4].SetActive(true);
            yield return new WaitForSeconds(0.8f);
            lights[2].SetActive(false);
            lights[4].SetActive(false);
            lights[0].SetActive(true);
            lights[5].SetActive(true);
        }
        // 如果右邊是綠燈
        // 左邊 黃燈 1 > 紅燈 2
        // 右邊 紅燈 5 > 綠燈 3
        if (rightLight)
        {
            lights[1].SetActive(true);
            lights[5].SetActive(true);
            yield return new WaitForSeconds(0.8f);
            lights[1].SetActive(false);
            lights[5].SetActive(false);
            lights[2].SetActive(true);
            lights[3].SetActive(true);
        }

        onSwitchLight(rightLight);
    }

    /// <summary>
    /// 按鈕壓下去彈開的效果
    /// </summary>
    private IEnumerator ButtonEffect()
    {
        click = true;
        btnClick.interactable = false;
        RectTransform btn = btnClick.GetComponent<RectTransform>();

        Vector3 posOriginal = btn.anchoredPosition;
        Vector3 pos = btn.anchoredPosition;

        float y = pos.y - 60;

        while (pos.y > y)
        {
            pos.y -= 10;
            btn.anchoredPosition = pos;
            yield return new WaitForSeconds(0.02f);
        }

        StartCoroutine(SetLight());                                         // 切換紅綠燈
        btn.anchoredPosition = posOriginal;                                 // 按鈕彈回

        yield return new WaitForSeconds(1);                                 // 等一秒再才能再按
        btnClick.interactable = true;
    }

    /// <summary>
    /// 設定分數
    /// </summary>
    /// <param name="value">要增減的值</param>
    public void SetScore(int value)
    {
        score += value;
        score = Mathf.Clamp(score, 0, 999);
        textScore.text = score + "";
    }

    /// <summary>
    /// 播放動畫
    /// </summary>
    /// <param name="aniName">動畫參數名稱：開心笑、開心跳、困惑</param>
    public void PlayAnimation(string aniName)
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName(aniName)) ani.SetTrigger(aniName);
    }
}
