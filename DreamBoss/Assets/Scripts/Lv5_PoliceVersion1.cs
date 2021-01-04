using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Lv5_PoliceVersion1 : LevelBase
{
    [Header("紅綠燈：左綠黃紅、右綠黃紅")]
    public GameObject[] lights;

    public static Lv5_PoliceVersion1 instance;

    /// <summary>
    /// 右邊綠燈是否亮
    /// </summary>
    private bool rightLight = true;
    /// <summary>
    /// 按鈕：按鈕上方
    /// </summary>
    private Button btnClick;

    public delegate void SwitchLight(bool right);
    public event SwitchLight onSwitchLight;

    protected override void Awake()
    {
        base.Awake();

        instance = this;

        btnClick = GameObject.Find("按鈕上方").GetComponent<Button>();
        btnClick.onClick.AddListener(() => { StartCoroutine(ButtonEffect()); });
    }

    private void Start()
    {
        StartCoroutine(SetLight());
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
}
