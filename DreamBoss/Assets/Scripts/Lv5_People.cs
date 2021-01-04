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

    private RectTransform rect;

    private void Start()
    {
        rect = transform.Find("人").GetComponent<RectTransform>();

        Lv5_PoliceVersion1.instance.onSwitchLight += SwitchLight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(posStop.position, 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(posPass.position, 10f);
    }

    private void SwitchLight(bool right)
    {
        StartCoroutine(Walk(right));
    }

    private IEnumerator Walk(bool right)
    {
        Vector2 pos = right == peopleRight ? posPass.anchoredPosition : posStop.anchoredPosition;
        Vector2 posPeople = rect.anchoredPosition;

        while (Vector2.Distance(posPeople, pos) > 5)
        {
            //posPeople = Vector2.Lerp(posPeople, pos, 0.05f);
            posPeople = Vector2.MoveTowards(posPeople, pos, 50f);
            rect.anchoredPosition = posPeople;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
