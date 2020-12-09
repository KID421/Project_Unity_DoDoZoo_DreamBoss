using UnityEngine;
using UnityEngine.EventSystems;

public class DragObjectBase : MonoBehaviour, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 座標元件
    /// </summary>
    protected RectTransform rect;
    /// <summary>
    /// 原始尺寸
    /// </summary>
    protected Vector2 posOriginal;
    /// <summary>
    /// 滑鼠與中心點位移
    /// </summary>
    protected Vector3 posOffset;
    /// <summary>
    /// 是否開始拖拉
    /// </summary>
    protected bool startDrag;
    /// <summary>
    /// 是否正確
    /// </summary>
    protected bool correct;
    /// <summary>
    /// 正確位置
    /// </summary>
    protected Vector2 posCorrect;
    /// <summary>
    /// 判定正確的距離
    /// </summary>
    protected float distance;
    /// <summary>
    /// 關卡基底
    /// </summary>
    protected LevelBase level;

    /// <summary>
    /// 編號
    /// </summary>
    public int index;

    protected virtual void Awake()
    {
        level = FindObjectOfType<LevelBase>();
        rect = GetComponent<RectTransform>();
        posOriginal = rect.anchoredPosition;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (correct) return;                                            // 如果本飾品已在正確位置就跳出

        if (!startDrag)                                                 // 如果還沒拖拉
        {
            startDrag = true;                                           // 開始拖拉

            Vector3 posEvent = new Vector3(eventData.position.x, eventData.position.y, 0);
            posOffset = posEvent - transform.position;

            StartDrag();
        }

        transform.position = eventData.position - new Vector2(posOffset.x, posOffset.y);
    }

    /// <summary>
    /// 開始拖拉，只執行一次
    /// </summary>
    protected virtual void StartDrag()
    {
        level.ani.SetBool("移動中", true);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (correct) return;                                                    // 如果本飾品已在正確位置就跳出

        startDrag = false;                                                      // 還沒拖拉
        level.ani.SetBool("移動中", false);

        //判定距離正確位置 20 個單位 並且 此部位尚未拖拉至正確位置
        if (Vector2.Distance(rect.anchoredPosition, posCorrect) < distance) PositionCorrect();
        else PositionWrong();
    }

    protected virtual void PositionCorrect()
    {
        correct = true;                                                     // 已在正確位置
        rect.anchoredPosition = posCorrect;                                 // 放置正確位置
        StartCoroutine(level.Correct());                                    // 正確特效
    }

    protected virtual void PositionWrong()
    {
        rect.anchoredPosition = posOriginal;                // 回到原點
        StartCoroutine(level.Wrong());                      // 錯誤特效
    }
}
