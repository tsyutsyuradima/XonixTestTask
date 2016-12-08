using UnityEngine;
using System.Collections.Generic;

public class WaterEnemyBehaviour : EnemyBehaviour
{
    public override void Start()
    {
        base.Start();
        currentDirection = Direction.TopLeft;
    }

    public override void CheckDirrection()
    {
        CheckIsEnemyCollidePlayerLine();
        collideOrientation = isEnemyZoneCollide();
        base.CheckDirrection();
    }

    Orientation isEnemyZoneCollide()
    {
        List<Vector2> pointsList = GameController.Inst.GetEnemyZone();
        for (int i = 0; i < pointsList.Count; i++)
        {
            int nextIndex = (i < pointsList.Count - 1) ? i + 1 : 0;
            myLine line = new myLine() { StartPoint = pointsList[i], EndPoint = pointsList[nextIndex] };
            if (LineHelper.isVectorIntersect(transform.position, line, 0.02f))
            {
                if ((line.StartPoint.x - line.EndPoint.x) < 0.2f && (line.StartPoint.x - line.EndPoint.x) > -0.2f)
                    return Orientation.Vertical;
                else if ((line.StartPoint.y - line.EndPoint.y) < 0.2f && (line.StartPoint.y - line.EndPoint.y) > -0.2f)
                    return Orientation.Horizontal;
            }
        }
        if (!LineHelper.IsPointInPolygon(pointsList, transform.position))
        {
            return Orientation.Vertical;
        }
        return Orientation.NONE;
    }

    void CheckIsEnemyCollidePlayerLine()
    {
        if (GameController.Inst.isDrawingNewZone)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            foreach (Vector2 pos in GameController.Inst.GetDrawLine())
            {
                if (x >= pos.x - GameController.enemySpeed &&
                    x <= pos.x + GameController.enemySpeed &&
                    y >= pos.y - GameController.enemySpeed &&
                    y <= pos.y + GameController.enemySpeed)
                {
                    GameController.Inst.Broken();
                    return;
                }
            }
        }
    }
}