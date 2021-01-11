using UnityEngine;

[ExecuteInEditMode]
public class Lv1_Tip : MonoBehaviour
{
    public TipObjecct[] tipObjects;

    private void Update()
    {
        SetTip();
    }

    private void SetTip()
    {
        for (int i = 0; i < tipObjects.Length; i++)
        {
            Transform temp = transform.Find("提示 " + i);
            tipObjects[i].traStart = temp.Find("提示開始");
            tipObjects[i].traEnd = temp.Find("提示結束");
            tipObjects[i].traArrow = temp.Find("箭頭");

            Transform tempLine = transform.GetChild(i);
            LineRenderer line = tempLine.GetComponent<LineRenderer>();

            tipObjects[i].traStart.position = line.GetPosition(0);
            tipObjects[i].traEnd.position = line.GetPosition(line.positionCount - 1);

            tipObjects[i].traArrow.position = line.GetPosition(0);
            tipObjects[i].traArrow.localEulerAngles = new Vector3(0, 0, tipObjects[i].angleArrow);
        }
    }
}

[System.Serializable]
public struct TipObjecct
{
    public Transform traStart;
    public Transform traEnd;
    public Transform traArrow;
    public int angleArrow;
}