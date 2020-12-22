using UnityEngine;

public class Lv1_TeacherVersion1 : LevelBase
{
    public LineRenderer lineQuestion;

    private LineRenderer line;
    private int indexLine;

    protected override void Awake()
    {
        base.Awake();

        line = GetComponent<LineRenderer>();

        Vector3 posLineQuestion = lineQuestion.GetPosition(indexLine);
        line.positionCount = 1;
        line.SetPosition(indexLine, posLineQuestion);
        indexLine++;
    }

    protected override void Update()
    {
        base.Update();

        CheckMousePoisition();
    }

    private void CheckMousePoisition()
    {
        if (indexLine > 99) return;

        Vector3 posMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 posMouseWorld = Camera.main.ScreenToWorldPoint(posMouse);

        Vector3 posLineQuestion = lineQuestion.GetPosition(indexLine);

        float dis = Vector3.Distance(posMouseWorld, posLineQuestion);

        //print("題目：" + posLineQuestion);
        //print("滑鼠：" + posMouseWorld);
        //print("距離：" + dis);

        if (dis < 1.5f)
        {
            line.positionCount = (indexLine + 1);
            line.SetPosition(indexLine, posLineQuestion);
            indexLine++;
        }
    }
}
