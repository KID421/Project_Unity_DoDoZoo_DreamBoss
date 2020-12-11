using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Lv4_DragObject : DragObjectBase
{
    [Header("題目：0 飛機")]
    public int indexQuestion;

    /// <summary>
    /// 這次的答案
    /// </summary>
    private Lv4_Answer answer;

    protected override void Awake()
    {
        base.Awake();

        answer = Lv4_Engineer.instance.answers[indexQuestion];
        posCorrect = answer.rectAnswers[index].anchoredPosition;
        distance = Lv4_Engineer.instance.distance;
    }

    protected override void PositionCorrect()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<Button>().enabled = false;
        answer.rectAnswers[index].gameObject.SetActive(true);

        base.PositionCorrect();
    }

    protected override void StartDrag()
    {
        base.StartDrag();
        Lv4_Engineer.instance.ani.SetBool("移動中", true);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        Lv4_Engineer.instance.ani.SetBool("移動中", false);
    }
}
