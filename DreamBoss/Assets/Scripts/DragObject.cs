using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("編號"), Tooltip("所有飾品：0 藍色上衣, 1 黃色上衣, 2 水壺, 3 背包, 4 粉色鞋子, 5 咖啡鞋子, 6 頭帶, 7 帽子, 8 裙子, 9 褲子")]
    public int index;
    [Header("部位：0 頭飾、1 上衣、2 飾品、3 褲子、4 鞋子")]
    public int indexPart;

    private RectTransform rect;         // 座標元件
    private Vector2 posOriginal;        // 原始尺寸
    private Vector3 posOffset;          // 滑鼠與中心點位移
    private bool startDrag;             // 是否開始拖拉
    private bool correct;               // 是否正確

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        posOriginal = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (correct) return;                                            // 如果本飾品已在正確位置就跳出

        if (!startDrag)                                                 // 如果還沒拖拉
        {
            startDrag = true;                                           // 開始拖拉

            Vector3 posEvent = new Vector3(eventData.position.x, eventData.position.y, 0);
            posOffset = posEvent - transform.position;

            Lv11_Stylist.instance.ani.SetBool("移動中", true);
        }

        transform.position = eventData.position - new Vector2(posOffset.x, posOffset.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (correct) return;                                                    // 如果本飾品已在正確位置就跳出

        startDrag = false;                                                      // 還沒拖拉
        Lv11_Stylist.instance.ani.SetBool("移動中", false);

        Vector2 posCorrect = Lv11_Stylist.instance.posCorrectAll[index];        // 取得正確位置

        // 判定距離正確位置 20 個單位 並且 此部位尚未拖拉至正確位置
        if (Vector2.Distance(rect.anchoredPosition, posCorrect) < 20f && !Lv11_Stylist.instance.chooseParts[indexPart])
        {
            correct = true;                                                     // 已在正確位置
            Lv11_Stylist.instance.chooseParts[indexPart] = true;                // 此部位已經放置
            rect.anchoredPosition = posCorrect;                                 // 放置正確位置

            StartCoroutine(Lv11_Stylist.instance.Correct());                    // 正確特效
        }
        else
        {
            StartCoroutine(Lv11_Stylist.instance.Wrong());                      // 錯誤特效
            rect.anchoredPosition = posOriginal;                                // 回到原點
        }
    }
}
