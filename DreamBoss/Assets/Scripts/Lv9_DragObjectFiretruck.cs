using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class Lv9_DragObjectFiretruck : DragObjectBase
{
    /// <summary>
    /// 房子上的人 少年
    /// 房子上的人 中年女
    /// 房子上的人 老男人
    /// </summary>
    private Vector2[] posPeoples =
    {
        new Vector2(-680, -640),
        new Vector2(-940, -900),
        new Vector2(-1200, -1160),
    };

    private RawImage[] peopleOnHouse = new RawImage[3];
    private Image[] peopleOnLadder = new Image[3];

    protected override void Awake()
    {
        base.Awake();

        peopleOnHouse[0] = GameObject.Find("房子上的人 男孩").GetComponent<RawImage>();
        peopleOnHouse[1] = GameObject.Find("房子上的人 女人").GetComponent<RawImage>();
        peopleOnHouse[2] = GameObject.Find("房子上的人 老人").GetComponent<RawImage>();

        peopleOnLadder[0] = GameObject.Find("梯子上的人 男孩").GetComponent<Image>();
        peopleOnLadder[1] = GameObject.Find("梯子上的人 女人").GetComponent<Image>();
        peopleOnLadder[2] = GameObject.Find("梯子上的人 老人").GetComponent<Image>();
    }

    private void Start()
    {
        
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!startDrag)
        {
            startDrag = true;
            Vector3 posEvent = new Vector3(eventData.position.x, eventData.position.y, 0);
            posOffset = posEvent - transform.position;
        }

        Vector3 posMove = eventData.position - new Vector2(posOffset.x, posOffset.y);
        posMove.y = transform.position.y;                                                       // 僅移動 X 軸 - 固定 Y 軸

        transform.position = posMove;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        startDrag = false;

        CheckPeople();
    }

    /// <summary>
    /// 檢查房子上的人與梯子的距離是否符合
    /// </summary>
    private void CheckPeople()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 pos = rect.anchoredPosition;

        for (int i = 0; i < posPeoples.Length; i++)
        {
            bool check = CheckInRange(pos, posPeoples[i]);              // 檢查每個人
            if (check)                                                  // 如果在範圍內
            {
                peopleOnHouse[i].color = new Color(1, 1, 1, 0);         // 隱藏房子上的人
                peopleOnLadder[i].color = new Color(1, 1, 1, 1);        // 顯示梯子上的人
                PositionCorrect();
                StartCoroutine(PeopleMoveDown(peopleOnLadder[i]));      // 樓梯上的人往下
            }
        }
    }

    /// <summary>
    /// 檢查範圍
    /// </summary>
    /// <param name="pos">消防車</param>
    /// <param name="people">人的範圍</param>
    /// <returns>是否在範圍內</returns>
    private bool CheckInRange(Vector2 pos, Vector2 people)
    {
        float x = pos.x;

        if (x > people.x && x < people.y) return true;
        else return false;
    }

    protected override void PositionCorrect()
    {
        StartCoroutine(level.Correct());
    }

    /// <summary>
    /// 樓梯上的人往下
    /// </summary>
    /// <param name="people">要下去的人</param>
    private IEnumerator PeopleMoveDown(Image people)
    {
        RectTransform rect = people.GetComponent<RectTransform>();      // 樓梯上的人變形元件
        Vector2 down = rect.anchoredPosition - Vector2.up * 400;        // 預設往下 400

        while (Vector2.Distance(rect.anchoredPosition, down) > 5)       // 往下
        {
            rect.anchoredPosition -= Vector2.up * 50;
            yield return new WaitForSeconds(0.05f);
        }

        people.color = new Color(1, 1, 1, 0);                           // 到底部後消失

        CheckAllPeopleDown();
    }

    /// <summary>
    /// 檢查是否所有人都下樓梯了
    /// </summary>
    private void CheckAllPeopleDown()
    {
        var allPeople = peopleOnHouse.Where(x => x.color.a == 0);
        Lv9_Fireman lv9 = (Lv9_Fireman)level;
        if (allPeople.ToList().Count == peopleOnHouse.Length) StartCoroutine(lv9.AllPeopleDownLadder());
    }
}
