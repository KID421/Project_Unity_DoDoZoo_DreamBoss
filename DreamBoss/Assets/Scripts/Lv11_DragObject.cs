using UnityEngine;

public class Lv11_DragObject : DragObjectBase
{
    //[Header("編號"), Tooltip("所有飾品：0 藍色上衣, 1 黃色上衣, 2 水壺, 3 背包, 4 粉色鞋子, 5 咖啡鞋子, 6 頭帶, 7 帽子, 8 裙子, 9 褲子")]
    //public int index;
    [Header("部位：0 頭飾、1 上衣、2 飾品、3 褲子、4 鞋子")]
    public int indexPart;

    protected override void Awake()
    {
        base.Awake();

        posCorrect = Lv11_Stylist.instance.posCorrectAll[index];        // 取得正確位置
        distance = Lv11_Stylist.instance.distance;
    }

    protected override void PositionCorrect()
    {
        if (!Lv11_Stylist.instance.chooseParts[indexPart])
        {
            Lv11_Stylist.instance.chooseParts[indexPart] = true;
            base.PositionCorrect();
        }
        else
        {
            PositionWrong();
        }
    }
}
