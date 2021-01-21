using UnityEngine;
using System.Linq;
using System.Collections;

public class Lv5_People : MonoBehaviour
{
    [Header("停止位置")]
    public RectTransform posStop;
    [Header("通過位置")]
    public RectTransform posPass;
    [Header("終點位置")]
    public RectTransform posEnd;
    [Header("是否為右邊的人")]
    public bool peopleRight;
    [Header("是否為汽車")]
    public bool car;
    [Header("移動速度"), Range(0f, 100f)]
    public float speed = 50f;
    /// <summary>
    /// 貨車、汽車、機車、老男、老女、中女、少男、少女
    /// </summary>
    [Header("編號")]
    public int index;

    /// <summary>
    /// 右邊是否通過
    /// </summary>
    public static bool[] passRight = new bool[8];
    /// <summary>
    /// 左邊是否通過
    /// </summary>
    public static bool[] passLeft = new bool[8];
    /// <summary>
    /// 是否右邊全部都通過
    /// </summary>
    public static bool passRightAll
    {
        get
        {
            var all = passRight.Where(x => x == true);
            if (all.ToList().Count == passRight.Length) return true;
            else return false;
        }
    }
    /// <summary>
    /// 是否左邊全部都通過
    /// </summary>
    public static bool passLeftAll
    {
        get
        {
            var all = passLeft.Where(x => x == true);
            if (all.ToList().Count == passLeft.Length) return true;
            else return false;
        }
    }
    /// <summary>
    /// 圖示出現的機率
    /// </summary>
    public static float percentIcon = 0.6f;
    /// <summary>
    /// 扣分機率
    /// </summary>
    public static float percentSubtration = 0.4f;

    /// <summary>
    /// 變形元件
    /// </summary>
    private RectTransform rect;
    /// <summary>
    /// 原始位置
    /// </summary>
    private Vector2 posOriginal;
    /// <summary>
    /// 頭上圖示
    /// </summary>
    private Lv5_Icon icon;
    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;
    /// <summary>
    /// 計時器扣分用
    /// </summary>
    private float timerScore;
    /// <summary>
    /// 是否停止
    /// </summary>
    private bool stop;
    /// <summary>
    /// 是否扣分
    /// </summary>
    private bool subScore;
    /// <summary>
    /// 是否前往終點
    /// </summary>
    private bool goToEnd;

    private void Start()
    {
        rect = transform.Find("物件").GetComponent<RectTransform>();
        posOriginal = rect.anchoredPosition;
        icon = rect.Find("頭上圖示").GetComponent<Lv5_Icon>();

        Lv5_PoliceVersion1.instance.onSwitchLight += SwitchLight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(posStop.position, 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(posPass.position, 10f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(posEnd.position, 10f);
    }

    private void Update()
    {
        Stopping();
    }

    /// <summary>
    /// 切換燈號後要執行的事情
    /// </summary>
    /// <param name="right">當前燈號是否為右邊</param>
    private void SwitchLight(bool right)
    {
        if (goToEnd)
        {
            rect.anchoredPosition = posOriginal;                                        // 回到原點
            goToEnd = false;
        }
        else StartCoroutine(Walk(right));
    }

    /// <summary>
    /// 走路
    /// </summary>
    /// <param name="right">是否亮右邊燈號</param>
    private IEnumerator Walk(bool right)
    {
        // 判定要前往哪一個位置
        Vector2 pos = right == peopleRight ? posPass.anchoredPosition : posStop.anchoredPosition;
        Vector2 posPeople = rect.anchoredPosition;

        // 前往位置
        while (Vector2.Distance(posPeople, pos) > 5)
        {
            posPeople = Vector2.MoveTowards(posPeople, pos, speed);
            rect.anchoredPosition = posPeople;
            yield return new WaitForSeconds(0.05f);
        }

        // 走到停止位置
        if (pos == posStop.anchoredPosition) Stop();

        // 走到終點
        if (pos == posPass.anchoredPosition) Pass();
    }

    /// <summary>
    /// 剛從停止要前往通過位置
    /// </summary>
    private void StartPass()
    {
        stop = false;
        icon.ResetIcon();
        icon.StopAllCoroutines();

        if (!Lv5_PoliceVersion1.instance.click) return;     // 如果還沒點過按鈕就跳出 第一次不加分

        // 加分
        icon.SetImage(0);
        icon.StartUp();
        icon.StartFadeInAndOut();
        Invoke("AddScoreDelay", Random.Range(0f, 0.8f));
    }

    /// <summary>
    /// 延遲加分
    /// </summary>
    private void AddScoreDelay()
    {
        Lv5_PoliceVersion1.instance.SetScore(5);
    }

    /// <summary>
    /// 通過
    /// </summary>
    private void Pass()
    {
        icon.StopAllCoroutines();
        icon.ResetIcon();
        StartCoroutine(WalkToEnd());                                                // 前往終點

        if (rect.anchoredPosition == posPass.anchoredPosition) StartPass();         // 通過後再加分

        if (peopleRight) passRight[index] = true;
        else passLeft[index] = true;
    }

    /// <summary>
    /// 前往終點
    /// </summary>
    private IEnumerator WalkToEnd()
    {
        goToEnd = true;

        Vector2 pos = posEnd.anchoredPosition;
        Vector2 posPeople = rect.anchoredPosition;

        while (Vector2.Distance(posPeople, pos) > 5)
        {
            posPeople = Vector2.MoveTowards(posPeople, pos, speed);
            rect.anchoredPosition = posPeople;
            yield return new WaitForSeconds(0.05f);
        }

        rect.anchoredPosition = posOriginal;                                        // 回到原點
        goToEnd = false;
    }

    /// <summary>
    /// 停止
    /// </summary>
    private void Stop()
    {
        float r = Random.Range(0f, 1f);
        if (r < percentIcon)
        {
            icon.SetImage(2);
            icon.StartUpAndDown();
            icon.StartFadeInAndOut();
            stop = true;
        }
        if (peopleRight) passRight[index] = false;
        else passLeft[index] = false;
    }

    /// <summary>
    /// 停止中
    /// 每 5 秒顯示困惑
    /// 每 7 秒扣 1 分
    /// </summary>
    private void Stopping()
    {
        if (!Timer.instance.stop && stop)                       // 如果 計時器 還沒停止 並且 物件停止 就開始計時
        {
            if (timer >= 5)
            {
                timer = 0;

                float r = Random.Range(0f, 1f);
                if (r < percentIcon)
                {
                    int i = 3;                                  // 圖示預設為 問號 編號 3
                    bool flipIcon = false;

                    if (car)
                    {
                        float rBeBe = Random.Range(0f, 1f);     // 出現喇叭的機率 預設為 4 成
                        if (rBeBe < percentSubtration)
                        {
                            subScore = true;                    // 要扣分
                            i = 4;                              // 如果 機率在 1 成內 就設為 喇叭 編號 4
                            flipIcon = peopleRight;
                        }
                    }
                    icon.StopAllCoroutines();
                    icon.SetImage(i, flipIcon);
                    icon.StartUpAndDown();
                    icon.StartFadeInAndOut();
                }
            }
            else timer += Time.deltaTime;

            if (timerScore >= 7)
            {
                timerScore = 0;
                timer = 0;

                if (subScore)
                {
                    subScore = false;
                    icon.StopAllCoroutines();
                    icon.SetImage(1);
                    icon.StartUp();
                    icon.StartFadeInAndOut();
                    Lv5_PoliceVersion1.instance.SetScore(-5);
                }
            }
            else timerScore += Time.deltaTime;
        }
        else
        {
            timer = 0;
            timerScore = 0;
        }
    }
}
