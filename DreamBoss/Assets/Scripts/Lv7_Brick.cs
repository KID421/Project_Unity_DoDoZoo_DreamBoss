using UnityEngine;

public class Lv7_Brick : MonoBehaviour
{
    [Header("是否為起點")]
    public bool isStart;
    [Header("是否為終點")]
    public bool isEnd;
    [Header("是否有金幣")]
    public bool hasCoin;
    
    private GameObject coin;

    private void Awake()
    {
        if (hasCoin)
        {
            coin = Resources.Load<GameObject>("金幣");
            GameObject temp = Instantiate(coin, transform);
            temp.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
