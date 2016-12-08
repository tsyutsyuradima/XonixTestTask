using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct myLine
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
};

public static class LineHelper
{
    //	-----------------------------------	
    //  Following method checks is currentLine(line drawn by last two points) collided with line 
    //	-----------------------------------	
    public static bool isVectorIntersectCollide(Vector2 vector, List<Vector2> pointsList)
    {
        if (pointsList.Count < 1 || vector == null)
            return false;

        for (int i = 0; i < pointsList.Count; i++)
        {
            int nextIndex = (i < pointsList.Count-1) ? i + 1 : 0;
            if (LineHelper.isVectorIntersect(vector, new myLine() { StartPoint = pointsList[i], EndPoint = pointsList[nextIndex]}, 0.04f))
                return true;
        }
        return false;
    }

    //	-----------------------------------	
    //  Following method checks are two Arrays of point Collided
    //	-----------------------------------	
    public static bool areLinesCollide(List<Vector2> pointsList1, List<Vector2> pointsList2)
    {
        if (pointsList1.Count < 2 || pointsList2.Count < 2)
            return false;

        int TotalLines1 = pointsList1.Count - 1;
        myLine[] lines1 = new myLine[TotalLines1];
        if (TotalLines1 > 1)
        {
            for (int i = 0; i < TotalLines1; i++)
            {
                lines1[i].StartPoint = (Vector3)pointsList1[i];
                lines1[i].EndPoint = (Vector3)pointsList1[i + 1];
            }
        }

        int TotalLines2 = pointsList2.Count - 1;
        myLine[] lines2 = new myLine[TotalLines2];
        if (TotalLines2 > 1)
        {
            for (int i = 0; i < TotalLines2; i++)
            {
                lines2[i].StartPoint = (Vector3)pointsList2[i];
                lines2[i].EndPoint = (Vector3)pointsList2[i + 1];
            }
        }

        for (int i = 0; i < TotalLines1; i++)
        {
            for (int j = 0; j < TotalLines2; j++)
            {
                if (isLinesIntersect(lines1[i], lines2[j]))
                    return true;
            }
        }
        return false;
    }

    //	-----------------------------------	
    //  Following method checks is currentLine(line drawn by last two points) collided with line 
    //	-----------------------------------	
    public static bool isLineCollide(List<Vector2> pointsList)
    {
        if (pointsList.Count < 2)
            return false;
        int TotalLines = pointsList.Count - 1;
        myLine[] lines = new myLine[TotalLines];
        if (TotalLines > 1)
        {
            for (int i = 0; i < TotalLines; i++)
            {
                lines[i].StartPoint = (Vector2)pointsList[i];
                lines[i].EndPoint = (Vector2)pointsList[i + 1];
            }
        }
        for (int i = 0; i < TotalLines - 1; i++)
        {
            myLine currentLine;
            currentLine.StartPoint = (Vector2)pointsList[pointsList.Count - 2];
            currentLine.EndPoint = (Vector2)pointsList[pointsList.Count - 1];
            if (isLinesIntersect(lines[i], currentLine))
                return true;
        }
        return false; 
    }
    //	-----------------------------------	
    //	Following method checks whether given two points are same or not
    //	-----------------------------------	
    public static bool checkPoints(Vector3 pointA, Vector3 pointB)
    {
        return (pointA.x >= pointB.x - GameController.playerSpeed &&
                pointA.x <= pointB.x + GameController.playerSpeed &&
                pointA.y >= pointB.y - GameController.playerSpeed &&
                pointA.y <= pointB.y + GameController.playerSpeed);

          //return   (pointA.x == pointB.x && pointA.y == pointB.y);
    }
    //	-----------------------------------	
    //	Following method checks whether given two line intersect or not
    //	-----------------------------------	
    public static bool isLinesIntersect(myLine L1, myLine L2, float zone = 0f)
    {
        if (checkPoints(L1.StartPoint, L2.StartPoint) ||
            checkPoints(L1.StartPoint, L2.EndPoint) ||
            checkPoints(L1.EndPoint, L2.StartPoint) ||
            checkPoints(L1.EndPoint, L2.EndPoint))
            return false;

        return ((Mathf.Max(L1.StartPoint.x, L1.EndPoint.x) + zone >= Mathf.Min(L2.StartPoint.x, L2.EndPoint.x) - zone) &&
                (Mathf.Max(L2.StartPoint.x, L2.EndPoint.x) + zone >= Mathf.Min(L1.StartPoint.x, L1.EndPoint.x) - zone) &&
                (Mathf.Max(L1.StartPoint.y, L1.EndPoint.y) + zone >= Mathf.Min(L2.StartPoint.y, L2.EndPoint.y) - zone) &&
                (Mathf.Max(L2.StartPoint.y, L2.EndPoint.y) + zone >= Mathf.Min(L1.StartPoint.y, L1.EndPoint.y) - zone)
               );
    }
    //	-----------------------------------	
    //	Following method checks whether given point intersect with line
    //	-----------------------------------	
    public static bool isVectorIntersect(Vector2 V1, myLine L2, float zone = 0.04f)
    {
        if (checkPoints(V1, L2.StartPoint) || checkPoints(V1, L2.EndPoint))
            return true;
        float xMin = Mathf.Min(L2.StartPoint.x, L2.EndPoint.x);
        xMin -= zone;
        float xMax = Mathf.Max(L2.StartPoint.x, L2.EndPoint.x);
        xMax += zone;
        float yMin = Mathf.Min(L2.StartPoint.y, L2.EndPoint.y);
        yMin -= zone;
        float yMax = Mathf.Max(L2.StartPoint.y, L2.EndPoint.y);
        yMax += zone;

        return ((V1.x >= xMin) && (xMax >= V1.x) &&
                (V1.y >= yMin) && (yMax >= V1.y));
    }


    public static bool IsPointInPolygon(List<Vector2> polygon, Vector2 p)
    {
        bool inside = false;
        for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
        {
            if ((polygon[i].y > p.y) != (polygon[j].y > p.y) &&
                 p.x < (polygon[j].x - polygon[i].x) * (p.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x)
            {
                inside = !inside;
            }
        }

        return inside;

        //bool result = false;
        //int j = polygon.Count - 1;
        //for (int i = 0; i < polygon.Count; i++)
        //{
        //    if (polygon[i].y < testPoint.y && 
        //        polygon[j].y >= testPoint.y || 
        //        polygon[j].y < testPoint.y && 
        //        polygon[i].y >= testPoint.y)
        //    {
        //        if (polygon[i].x + (testPoint.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < testPoint.x)
        //        {
        //            result = true;
        //        }
        //    }
        //    j = i;
        //}
        //return result;
    }
}