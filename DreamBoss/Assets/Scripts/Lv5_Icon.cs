using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lv5_Icon : MonoBehaviour
{
    /// <summary>
    /// +5、-5、問號、困惑、閃電、+100、-15
    /// </summary>
    [Header("圖示")]
    public Sprite[] sprIcons;

    private Image img;
    private RectTransform rect;
    private Vector2 posOriginal;
    private Vector3 sizeOriginal;

    private void Awake()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        posOriginal = rect.anchoredPosition;
        sizeOriginal = rect.localScale;
    }

    /// <summary>
    /// 設定圖片
    /// </summary>
    /// <param name="index">圖片編號：+5、-5、問號、困惑、喇叭、+100、-15</param>
    public void SetImage(int index, bool flip = false)
    {
        rect.localScale = sizeOriginal;
        rect.eulerAngles = new Vector3(0, flip ? 180 : 0, 0);
        img.sprite = sprIcons[index];
        img.SetNativeSize();

        if (index == 0) rect.localScale *= 3;               // +5 放大
        else if (index == 1) rect.localScale *= 3;          // -5 放大
        else if (index == 4) rect.localScale *= 5;          // 喇叭 放大
        else rect.localScale = sizeOriginal;
    }

    /// <summary>
    /// 開始淡入淡出
    /// </summary>
    public void StartFadeInAndOut()
    {
        StartCoroutine(FadeInAndOut());
    }

    /// <summary>
    /// 開始上下晃動
    /// </summary>
    public void StartUpAndDown()
    {
        StartCoroutine(UpAndDown());
    }

    /// <summary>
    /// 開始往上移動
    /// </summary>
    public void StartUp()
    {
        StartCoroutine(Up());
    }

    public void ResetIcon()
    {
        img.color = new Color(1, 1, 1, 0);
        rect.anchoredPosition = posOriginal;
    }

    /// <summary>
    /// 淡入並淡出
    /// </summary>
    private IEnumerator FadeInAndOut()
    {
        float a = img.color.a;

        while (a < 1)
        {
            a += 0.1f;
            img.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        while (a > 0)
        {
            a -= 0.1f;
            img.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.05f);
        }

        img.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// 上下晃動
    /// </summary>
    private IEnumerator UpAndDown()
    {
        Vector2 pos = rect.anchoredPosition;
        float yOriginal = pos.y;
        float yUp = pos.y + 30;
        float interval = 0.08f;
        float step = 3;

        for (int i = 0; i < 2; i++)
        {
            while (pos.y < yUp)
            {
                pos.y += step;
                rect.anchoredPosition = pos;
                yield return new WaitForSeconds(interval);
            }

            while (pos.y > yOriginal)
            {
                pos.y -= step;
                rect.anchoredPosition = pos;
                yield return new WaitForSeconds(interval);
            }
        }

        rect.anchoredPosition = posOriginal;
    }

    /// <summary>
    /// 往上移動
    /// </summary>
    private IEnumerator Up()
    {
        Vector2 pos = rect.anchoredPosition;
        float yUp = pos.y + 400;

        while (pos.y < yUp)
        {
            pos.y += 10;
            rect.anchoredPosition = pos;
            yield return new WaitForSeconds(0.05f);
        }

        rect.anchoredPosition = posOriginal;
    }
}
