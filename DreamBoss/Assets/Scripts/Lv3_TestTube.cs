using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Lv3_TestTube : MonoBehaviour
{
    [Header("細胞正確位置：試管內的四個子物件")]
    public RectTransform[] cellsPosition = new RectTransform[4];

    /// <summary>
    /// 細胞：放在試管內的細胞
    /// </summary>
    private List<Lv3_DragObject> cells = new List<Lv3_DragObject>();

    /// <summary>
    /// 細胞數量
    /// </summary>
    private int countCell;

    /// <summary>
    /// 是否過關
    /// </summary>
    public static bool pass;

    private void Start()
    {
        SpawnCell();
    }

    private void SpawnCell()
    {
        GameObject goCell = Lv3_Scientist.instance.cells[Random.Range(0, Lv3_Scientist.instance.cells.Length)];
        GameObject spawnCell = Instantiate(goCell);

        Lv3_DragObject cell = spawnCell.GetComponent<Lv3_DragObject>();

        // 設定細胞為子物件並調整尺寸
        cell.transform.SetParent(transform);
        cell.transform.localScale = Vector3.one * 0.8f;

        // 取得細胞與細胞位置的變形元件
        RectTransform rectCell = cell.GetComponent<RectTransform>();
        RectTransform rectPosition = cellsPosition[countCell];

        cell.GetComponent<Button>().interactable = false;
        cell.enabled = false;

        rectCell.anchoredPosition = rectPosition.anchoredPosition;

        // 細胞添加到清單內並將編號遞增
        cells.Add(cell);
        countCell++;
    }

    /// <summary>
    /// 設定細胞：於 Lv3_DragObject 拖拉結束後呼叫 OnEndDrag
    /// </summary>
    /// <param name="cell">當前拖拉的細胞</param>
    public IEnumerator SetCell(Lv3_DragObject cell)
    {
        if (countCell < 4)
        {
            // 設定細胞為子物件並調整尺寸
            cell.transform.SetParent(transform);
            cell.transform.localScale = Vector3.one * 0.8f;

            // 取得細胞與細胞位置的變形元件
            RectTransform rectCell = cell.GetComponent<RectTransform>();
            RectTransform rectPosition = cellsPosition[countCell];

            cell.GetComponent<Button>().interactable = false;
            cell.enabled = false;

            // 當距離大於 10 時，持續移動到正確位置
            while (Vector2.Distance(rectCell.anchoredPosition, rectPosition.anchoredPosition) > 10)
            {
                rectCell.anchoredPosition = Vector2.Lerp(rectCell.anchoredPosition, rectPosition.anchoredPosition, Time.deltaTime * 100);
                yield return null;
            }

            rectCell.anchoredPosition = rectPosition.anchoredPosition;

            // 如果 細胞 大於 一顆時
            if (countCell > 0)
            {
                // 取得前一顆與當前細胞名稱
                string prevCell = cells[countCell - 1].name;
                string currentCell = cell.name;

                // 如果 前一顆 與 當前 名稱相同 = 正確
                if (prevCell == currentCell) StartCoroutine(Lv3_Scientist.instance.Correct());
                // 否則 前一顆 與 當前 名稱不相同 = 錯誤
                else StartCoroutine(Lv3_Scientist.instance.Wrong());
            }
            // 否則 沒有任何細胞時 正確
            else if (countCell == 0) StartCoroutine(Lv3_Scientist.instance.Correct());

            // 細胞添加到清單內並將編號遞增
            cells.Add(cell);
            countCell++;

            //CheckThreeSameCell();
            CheckTwoSameCell();
        }
    }

    /// <summary>
    /// 檢查是不是有三顆相連的相同細胞
    /// </summary>
    private void CheckTwoSameCell()
    {
        // 如果細胞數量為 2
        if (countCell == 2)
        {
            // 1 與 2 顆 是否相同
            bool one_two = cells[0].name == cells[1].name;
            // 如果 1 與 2 相同
            if (one_two)
            {
                Destroy(cells[0].gameObject);
                Destroy(cells[1].gameObject);
            }
            
            // 否則 1 與 2，2 與 3 不同時：可能為 紅綠綠 的狀況
            // 如果細胞數量為 4 顆
            else if (countCell == 4)
            {
                // 2 與 3 是否相同
                bool two_three_2 = cells[1].name == cells[2].name;
                // 3 與 4 是否相同
                bool three_four_2 = cells[2].name == cells[3].name;

                // 可能為 紅綠綠綠 的狀況
                // 如果 2 與 3，3 與 4 相同就過關
                if (two_three_2 && three_four_2)
                {
                    pass = true;
                    StartCoroutine(Lv3_Scientist.instance.Pass());
                }
            }
        }
        // 如果細胞數量為 3
        if (countCell == 3)
        {
            // 2 與 3 顆 是否相同
            bool two_three = cells[1].name == cells[2].name;
            // 如果 2 與 3 相同
            if (two_three)
            {
                Destroy(cells[1].gameObject);
                Destroy(cells[2].gameObject);
            }
        }
        // 如果細胞數量為 4
        if (countCell == 4)
        {
            // 3 與 4 顆 是否相同
            bool three_four = cells[2].name == cells[3].name;
            // 如果 3 與 4 相同
            if (three_four)
            {
                Destroy(cells[3].gameObject);
                Destroy(cells[4].gameObject);
            }
        }
    }

    /// <summary>
    /// 檢查是不是有三顆相連的相同細胞
    /// </summary>
    private void CheckThreeSameCell()
    {
        // 如果細胞數量為 3 或 4 顆 再進行檢查
        if (countCell > 2)
        {
            // 1 與 2 顆 是否相同
            bool one_two = cells[0].name == cells[1].name;
            // 2 與 3 顆 是否相同
            bool two_three = cells[1].name == cells[2].name;

            // 如果 1 與 2，2 與 3 相同 就過關
            if (one_two && two_three)
            {
                pass = true;
                StartCoroutine(Lv3_Scientist.instance.Pass());
            }
            // 否則 1 與 2，2 與 3 不同時：可能為 紅綠綠 的狀況
            // 如果細胞數量為 4 顆
            else if (countCell == 4)
            {
                // 2 與 3 是否相同
                bool two_three_2 = cells[1].name == cells[2].name;
                // 3 與 4 是否相同
                bool three_four_2 = cells[2].name == cells[3].name;

                // 可能為 紅綠綠綠 的狀況
                // 如果 2 與 3，3 與 4 相同就過關
                if (two_three_2 && three_four_2)
                {
                    pass = true;
                    StartCoroutine(Lv3_Scientist.instance.Pass());
                }
            }
        }
    }
}
