using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyBehaviour : EnemyBehaviour
{
    Vector3 topLeftPos;
    Vector3 bottomRightPos;

    public override void Start()
    {
        base.Start();
        currentDirection = Direction.BottomLeft;
        topLeftPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        bottomRightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 5, 5));
    }

    public override void CheckDirrection()
    {
        collideOrientation = isScreenBorderCollide();
        if (collideOrientation == Orientation.NONE)
            collideOrientation = isEnemyZoneCollide();

        CheckIsEnemyCollidePlayer();
        base.CheckDirrection();
    }

    Orientation isScreenBorderCollide()
    {
        float x = transform.position.x;
        if (x >= bottomRightPos.x - GameController.enemySpeed/2 &&
            x <= bottomRightPos.x + GameController.enemySpeed/2 ||
            x >= topLeftPos.x - GameController.enemySpeed/2 &&
            x <= topLeftPos.x + GameController.enemySpeed/2)
        {
            return Orientation.Vertical;
        }

        float y = transform.position.y;
        if (y >= bottomRightPos.y - GameController.enemySpeed/2 &&
            y <= bottomRightPos.y + GameController.enemySpeed/2 ||
            y >= topLeftPos.y - GameController.enemySpeed/2 &&
            y <= topLeftPos.y + GameController.enemySpeed/2)
        {
            return Orientation.Horizontal;
        }

        return Orientation.NONE;
    }

    Orientation isEnemyZoneCollide()
    {
        List<Vector2> pointsList = GameController.Inst.GetEnemyZone();
        for (int i = 0; i < pointsList.Count; i++)
        {
            int nextIndex = (i < pointsList.Count - 1) ? i + 1 : 0;
            myLine line = new myLine() { StartPoint = pointsList[i], EndPoint = pointsList[nextIndex] };
            if (LineHelper.isVectorIntersect(transform.position , line, 0.02f))
            {
                if ((line.StartPoint.x - line.EndPoint.x) < 0.2f && (line.StartPoint.x - line.EndPoint.x) > - 0.2f)
                    return Orientation.Vertical;
                else if ((line.StartPoint.y - line.EndPoint.y) < 0.2f && (line.StartPoint.y - line.EndPoint.y) > -0.2f)
                    return Orientation.Horizontal;
            }
        }

        if (LineHelper.IsPointInPolygon(pointsList, transform.position))
        {
            return Orientation.Vertical;
        }
        return Orientation.NONE;
    }

    void CheckIsEnemyCollidePlayer()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        Vector2 pos = GameController.Inst.GetPlayerPos();
        if (x >= pos.x - GameController.enemySpeed &&
            x <= pos.x + GameController.enemySpeed &&
            y >= pos.y - GameController.enemySpeed &&
            y <= pos.y + GameController.enemySpeed)
        {
            GameController.Inst.Broken();
            collideOrientation = Orientation.Vertical;
        }
    }
}
