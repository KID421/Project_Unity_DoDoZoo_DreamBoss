using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Lv9_Event : UnityEvent
{

}

[System.Serializable]
public class Lv9_EventWithString : UnityEvent<string>
{

}

public class Lv9_DragObject : DragObjectBase
{
    [Header("正確位置")]
    public RectTransform rectCorrect;
    [Header("正確後要執行的事件")]
    public Lv9_Event correctEvent;
    [Header("正確後要執行的事件 - 帶字串參數")]
    public Lv9_EventWithString correctEventWithString;

    protected override void Awake()
    {
        base.Awake();

        posCorrect = rectCorrect.anchoredPosition;
    }

    private void Start()
    {
        distance = Lv9_Fireman.instance.distance;
    }

    protected override void PositionCorrect()
    {
        base.PositionCorrect();

        correctEvent.Invoke();
        correctEventWithString.Invoke("");
    }
}
