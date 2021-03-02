using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lv5_Violation : MonoBehaviour
{
    [Header("闖紅燈車子")]
    public RectTransform rect;
    [Header("終點")]
    public RectTransform rectEnd;
    [Header("闖紅燈車子速度"), Range(0, 1000)]
    public int speed = 100;
    [Header("距離終點多少算失敗"), Range(0, 5000)]
    public int failedDistance = 1000;

    /// <summary>
    /// 闖紅燈車子按鈕
    /// </summary>
    private Button btnRed;
    private Lv5_Icon icon;
    /// <summary>
    /// 是否失敗
    /// </summary>
    private bool failed;

    private void Awake()
    {
        btnRed = rect.GetComponent<Button>();
        icon = rect.GetChild(0).GetComponent<Lv5_Icon>();
        btnRed.onClick.AddListener(() => { StartCoroutine(ClickRed()); });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rectEnd.position, 10);
    }

    private void Update()
    {
        RedLight();
        Failed();
    }

    /// <summary>
    /// 點到闖紅燈的車子
    /// </summary>
    private IEnumerator ClickRed()
    {
        btnRed.interactable = false;
        speed = 0;
        rect.GetComponent<CircleCollider2D>().enabled = false;
        Lv5_PoliceVersion1.instance.SetScore(100);
        icon.SetImage(5);
        icon.StartUp();
        icon.StartFadeInAndOut();

        float top = rect.anchoredPosition.y;
        float topUp = top + 200;

        //while (top < topUp)
        //{
        //    top += 30;
        //    Vector3 pos = rect.anchoredPosition;
        //    pos.y = top;
        //    rect.anchoredPosition = pos;
        //    rect.localScale += Vector3.one * 0.05f;
        //    yield return new WaitForSeconds(0.05f);
        //}

        Image img = rect.GetComponent<Image>();
        float a = img.color.a;

        while (a > 0)
        {
            a -= 0.3f;
            img.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 闖紅燈
    /// </summary>
    private void RedLight()
    {
        Vector2 pos = Vector2.MoveTowards(rect.anchoredPosition, rectEnd.anchoredPosition, speed * Time.deltaTime);
        rect.anchoredPosition = pos;
    }

    /// <summary>
    /// 失敗，闖紅燈通過
    /// </summary>
    private void Failed()
    {
        if (!failed)
        {
            float dis = Vector2.Distance(rect.anchoredPosition, rectEnd.anchoredPosition);

            if (dis <= failedDistance)
            {
                failed = true;
                btnRed.interactable = false;
                rect.GetComponent<CircleCollider2D>().enabled = false;
                speed = 200;
                Lv5_PoliceVersion1.instance.SetScore(-15);
                icon.SetImage(6);
                icon.StartUp();
                icon.StartFadeInAndOut();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (name.Contains("右邊") && collision.transform.parent.name.Contains("左邊"))
        {
            print("失敗");
        }
        else if (name.Contains("左邊") && collision.transform.parent.name.Contains("右邊"))
        {
            print("失敗");
        }
    }
}
