using UnityEngine;

public class Lv5_Violation : MonoBehaviour
{
    public PositionStartAndEnd[] posStartAndEnd = new PositionStartAndEnd[2];

    private void OnDrawGizmos()
    {
        for (int i = 0; i < posStartAndEnd.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(posStartAndEnd[i].rectStart.position, 10);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(posStartAndEnd[i].rectEnd.position, 10);
        }
    }
}

/// <summary>
/// 座標：起點與終點
/// </summary>
[System.Serializable]
public struct PositionStartAndEnd
{
    public string name;
    [Header("起點")]
    public RectTransform rectStart;
    [Header("終點")]
    public RectTransform rectEnd;
}