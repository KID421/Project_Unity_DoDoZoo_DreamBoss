using UnityEngine;
using System.Collections;

public class Lv5_People : MonoBehaviour
{
    [Header("停止位置")]
    public RectTransform posStop;
    [Header("通過位置")]
    public RectTransform posPass;
    [Header("是否為右邊的人")]
    public bool peopleRight;
    [Header("移動速度"), Range(0f, 100f)]
    public float speed = 50f;

    private RectTransform rect;
    /// <summary>
    /// 原始位置
    /// </summary>
    private Vector2 posOriginal;

    private void Start()
    {
        rect = transform.Find("物件").GetComponent<RectTransform>();
        posOriginal = rect.anchoredPosition;

        Lv5_PoliceVersion1.instance.onSwitchLight += SwitchLight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(posStop.position, 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(posPass.position, 10f);
    }

    /// <summary>
    /// 切換燈號後要執行的事情
    /// </summary>
    /// <param name="right">當前燈號是否為右邊</param>
    private void SwitchLight(bool right)
    {
        StartCoroutine(Walk(right));
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

        // 走到終點
        if (pos == posPass.anchoredPosition) rect.anchoredPosition = posOriginal;
    }
}
