using UnityEngine;

public class Lv11_DragObject : DragObjectBase
{
    //[Header("編號"), Tooltip("所有飾品：0 藍色上衣, 1 黃色上衣, 2 水壺, 3 背包, 4 粉色鞋子, 5 咖啡鞋子, 6 頭帶, 7 帽子, 8 裙子, 9 褲子")]
    //public int index;
    [Header("部位：0 頭飾、1 上衣、2 飾品、3 褲子、4 鞋子")]
    public int indexPart;

    [Header("與自己同一組的飾品")]
    public RectTransform rectOther;

    protected override void Awake()
    {
        base.Awake();

        DressPosition dressPos = Lv11_Stylist.instance.dressPositions[Lv11_Stylist.instance.indexCurrent];
        posCorrect = dressPos.posCorrectAll[index];                                                         // 取得正確位置
        distance = Lv11_Stylist.instance.distance;
    }

    protected override void PositionCorrect()
    {
        /* 舊版本 -- KID 2020.12.18
        if (!Lv11_Stylist.instance.chooseParts[indexPart])
        {
            Lv11_Stylist.instance.chooseParts[indexPart] = true;
            base.PositionCorrect();
        }
        else
        {
            PositionWrong();
        }
        -- */

        // -- KID 2020.12.18 如果 與自己同一組的飾品 已經 在正確位置
        // 小女孩開心動畫
        Lv11_Stylist.instance.aniGirl.SetTrigger("開心");
        
        if (rectOther != null && rectOther.GetComponent<Lv11_DragObject>().correct)
        {
            // 移回原位並且設定為尚未在正確位置
            rectOther.GetComponent<Lv11_DragObject>().PositionWrong();
            rectOther.GetComponent<Lv11_DragObject>().correct = false;
            rectOther.gameObject.tag = "Untagged";
        }

        // 本身移到正確位置
        tag = "刺蝟小女孩拍照";
        base.PositionCorrect();
        // -- KID
    }
}
