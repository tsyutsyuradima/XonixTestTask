using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
///         This class is Responsible for drawing Mesh / "zone"
/// </summary>
/// 
public class ZoneManager : MonoBehaviour
{
    public List<Vector2> newPosArray = new List<Vector2>();
    public List<Vector2> currentPosArray = new List<Vector2>();
    public List<Vector2> defaultEnemyZone ;

    public Material material;
    GameObject enemyZone;

    void Start ()
    {
        GameController.Inst.onStartGame += GameManager_onStartGame;
        GameController.Inst.onLevelUp += GameManager_onLevelUp;
        GameController.Inst.onStartMoving += Player_onStartMoving;
        GameController.Inst.onEndMoving += Player_onEndMoving;
        GameController.Inst.onBroken += Player_onBroken;
        GameController.Inst.onChangeDirection += Player_onChangeDirection;
    }

    private void GameManager_onLevelUp()
    {
        newPosArray.Clear();
        DrawEnemyZone();
    }

    private void GameManager_onStartGame()
    {
        newPosArray.Clear();
        DrawGreenZone();
        DrawEnemyZone();
    }
    private void Player_onStartMoving(Vector3 newPos)
    {
        newPosArray.Clear();
        newPos.x = (float)Math.Round(newPos.x, 2);
        newPos.y = (float)Math.Round(newPos.y, 2);
        newPosArray.Add(newPos);
    }
    private void Player_onEndMoving(Vector3 newPos)
    {
        newPos.x = (float)Math.Round(newPos.x, 2);
        newPos.y = (float)Math.Round(newPos.y, 2);
        newPosArray.Add(newPos);

        if (Vector3.Distance(newPos, newPosArray[newPosArray.Count - 1]) > 0.2)
            CutEnemyZone();
        else
            GameController.Inst.Broken();

        newPosArray.Clear();
    }
    private void Player_onBroken(Vector3 pos)
    {
        newPosArray.Clear();
    }

    /// <summary>
    ///     When player change direction we save this position
    /// </summary>
    private void Player_onChangeDirection(Vector2 newPos)
    {
        /// <summary>
        ///    This exeption for situations like this : 
        ///    http://prntscr.com/dg6bon
        /// </summary>
        if (newPosArray.Count > 0)
        {
            myLine newLine = new myLine() { StartPoint = newPosArray[newPosArray.Count - 1], EndPoint = newPos };
            bool isContains = false;
            foreach (Vector2 pos in currentPosArray)
            {
                if (pos.x >= newLine.StartPoint.x - GameController.playerSpeed &&
                    newLine.StartPoint.x + GameController.playerSpeed >= pos.x &&
                    pos.y >= newLine.StartPoint.y - GameController.playerSpeed &&
                    newLine.StartPoint.y + GameController.playerSpeed >= pos.y)
                    isContains = true;
            }
            foreach (Vector2 pos in currentPosArray)
            {
                if (LineHelper.isVectorIntersect(pos, newLine))
                {
                    if (isContains)
                        newPosArray[newPosArray.Count - 1] = pos;
                    else
                        newPosArray.Add(pos);
                }
            };
        }
        newPos.x = (float)Math.Round(newPos.x,2);
        newPos.y = (float)Math.Round(newPos.y,2);
        newPosArray.Add(newPos);
    }
    void DrawGreenZone()
    {
        int topX = Screen.width;
        int topY = Screen.height;

        List<Vector2> greenZonePos = new List<Vector2>();
        greenZonePos.Add(Camera.main.ScreenToWorldPoint(new Vector2(topX, topY)));
        greenZonePos.Add(Camera.main.ScreenToWorldPoint(new Vector2(topX, 0)));
        greenZonePos.Add(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)));
        greenZonePos.Add(Camera.main.ScreenToWorldPoint(new Vector2(0, topY)));

        GameObject greenZoneBackground = DrawElement(greenZonePos, Color.white);
        greenZoneBackground.transform.position = new Vector3(0, 0, 1);
        greenZoneBackground.gameObject.tag = "greenZone";
    }
    void DrawEnemyZone()
    {
        if (enemyZone == null)
        {
            int topX = Screen.width - 20;
            int topY = Screen.height - 20;

            currentPosArray.Add(Camera.main.ScreenToWorldPoint(new Vector2(topX, topY)));
            currentPosArray.Add(Camera.main.ScreenToWorldPoint(new Vector2(topX, 20)));
            currentPosArray.Add(Camera.main.ScreenToWorldPoint(new Vector2(20, 20)));
            currentPosArray.Add(Camera.main.ScreenToWorldPoint(new Vector2(20, topY)));

            float Square = 0f;
            for (int i = 0; i < currentPosArray.Count; i++)
            {
                Vector3 pos = currentPosArray[i];
                pos.x = (float)Math.Round(pos.x, 1);
                pos.y = (float)Math.Round(pos.y, 1);
                if (i < currentPosArray.Count-1)
                    Square += pos.x * currentPosArray[i + 1].y - pos.y * currentPosArray[i + 1].x;
                else
                    Square += pos.x * currentPosArray[0].y - pos.y * currentPosArray[0].x;

                currentPosArray[i] = pos;
            }
            GameController.Inst.SetFullEnemySquare((int)Mathf.Abs(Square));

            defaultEnemyZone = new List<Vector2>(currentPosArray);

            enemyZone = DrawElement(currentPosArray, new Color32(68,138,173,255));
            enemyZone.transform.position = new Vector3(0, 0, 0);
            enemyZone.gameObject.tag = "enemy";
        }
        else
        {
            DrawElement(defaultEnemyZone, new Color32(68, 138, 173, 255), enemyZone);
            currentPosArray = new List<Vector2>(defaultEnemyZone);
        }
    }

    /// <summary>
    /// After successful player moving
    /// This function cut from enemy zone    zone which is succesful detected by player
    /// 
    /// 
    /// Actualy we have a list of vertices of enemy zone (Mesh)
    /// After successful player moving we analyze current enemy zone and insert in it new 
    /// </summary>

    void CutEnemyZone()
    {
        if (newPosArray.Count > 1)
        {
            List<Vector2> firstList = new List<Vector2>();
            float first_S = 0f;
            List<Vector2> secondList = new List<Vector2>();
            float second_S = 0f;

            Vector2 startPos = newPosArray[0];
            Vector2 endPos = newPosArray[newPosArray.Count-1];
            newPosArray.Remove(startPos);
            newPosArray.Remove(endPos);


            /// 
            /// Finding and insert endpoints of new zone which Intersected with current array of vertices;
            /// 

            startPos = InsertVectorToArray(startPos);
            endPos = InsertVectorToArray(endPos);

            int startIndex = currentPosArray.IndexOf(startPos);
            int endIndex = currentPosArray.IndexOf(endPos);

            if (startIndex < 0 || endIndex < 0)
            {
                Debug.LogError("Cannot find position for new line");
                GameController.Inst.Broken();
                return;
            }


            ///
            /// Get Square of two array and detect which bigger
            /// 

            int dif = currentPosArray.Count - startIndex - 1;
            for (int i = 0; i < currentPosArray.Count; i++)
            {
                int index = (i <= dif) ? startIndex + i : i - dif - 1;
                int nextIndex = (i < dif) ? startIndex + i + 1 : i - dif;

                first_S += (currentPosArray[index].x * currentPosArray[nextIndex].y - currentPosArray[index].y * currentPosArray[nextIndex].x);
                firstList.Add(currentPosArray[index]);

                if (currentPosArray[nextIndex] == endPos)
                {
                    firstList.Add(endPos);
                    if (newPosArray.Count > 0)
                    {
                        first_S += (endPos.x * newPosArray[newPosArray.Count - 1].y - endPos.y * newPosArray[newPosArray.Count - 1].x);
                        for (int j = newPosArray.Count - 1; j >= 0; --j)
                        {
                            firstList.Add(newPosArray[j]);
                            if (j > 0)
                                first_S += (newPosArray[j].x * newPosArray[j - 1].y - newPosArray[j].y * newPosArray[j - 1].x);
                            else
                                first_S += (newPosArray[j].x * startPos.y - newPosArray[j].y * startPos.x);
                        }
                    }
                    i = currentPosArray.Count;
                }
            }

            for (int i = 0; i < currentPosArray.Count; i++)
            {
                int index = (startIndex - i >= 0) ? startIndex - i : currentPosArray.Count - (i - startIndex);
                int nextIndex = (startIndex - i - 1 >= 0) ? startIndex - i - 1 : currentPosArray.Count - (i - startIndex) - 1;

                secondList.Add(currentPosArray[index]);
                second_S += (currentPosArray[index].x * currentPosArray[nextIndex].y - currentPosArray[index].y * currentPosArray[nextIndex].x);

                if (currentPosArray[nextIndex] == endPos)
                {
                    secondList.Add(endPos);
                    if (newPosArray.Count > 0)
                    {
                        second_S += (endPos.x * newPosArray[newPosArray.Count - 1].y - endPos.y * newPosArray[newPosArray.Count - 1].x);
                        for (int j = newPosArray.Count - 1; j >= 0; --j)
                        {
                            secondList.Add(newPosArray[j]);
                            if (j > 0)
                                second_S += (newPosArray[j].x * newPosArray[j - 1].y - newPosArray[j].y * newPosArray[j - 1].x);
                            else
                                second_S += (newPosArray[j].x * startPos.y - newPosArray[j].y * startPos.x);
                        }
                    }
                    i = currentPosArray.Count;
                }
            }

            first_S = (int)Mathf.Abs(first_S);
            second_S = (int)Mathf.Abs(second_S);
            currentPosArray = (second_S > first_S) ? secondList : firstList;
            ///
            /// Draw new EnemyZone;
            /// 
            DrawElement(currentPosArray, new Color32(68, 138, 173, 255), enemyZone);
            GameController.Inst.CalculateNewScore((second_S < first_S) ? second_S : first_S , (second_S > first_S) ? second_S : first_S);
        }
    }

    Vector2 InsertVectorToArray(Vector2 pos)
    {
        for (int i = 0; i < currentPosArray.Count - 1; i++)
        {
            if (pos.x >= currentPosArray[i].x - GameController.playerSpeed &&
                currentPosArray[i].x + GameController.playerSpeed >= pos.x &&
                pos.y >= currentPosArray[i].y - GameController.playerSpeed &&
                currentPosArray[i].y + GameController.playerSpeed >= pos.y)
                return currentPosArray[i];
        }
        for (int i = 0; i < currentPosArray.Count; i++)
        {
            int nextIndex = (i < currentPosArray.Count - 1) ? i + 1 : 0;
            if (LineHelper.isVectorIntersect(pos, new myLine() { StartPoint = currentPosArray[i], EndPoint = currentPosArray[nextIndex]}, GameController.playerSpeed))
            {
                pos.x = (currentPosArray[i].x == currentPosArray[nextIndex].x) ? currentPosArray[i].x : pos.x;
                pos.y = (currentPosArray[i].y == currentPosArray[nextIndex].y) ? currentPosArray[i].y : pos.y;
                currentPosArray.Insert(i + 1, pos);
                return pos;
            }
        }
        return Vector3.zero;
    }

   
    GameObject DrawElement(List<Vector2> posArray, Color color, GameObject go = null)
    {
        Triangulator tr = new Triangulator(posArray.ToArray());
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[posArray.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3((float)Math.Round(posArray[i].x, 2), (float)Math.Round(posArray[i].y, 2), 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        
        // Set up game object with mesh;
        GameObject tmp;
        MeshFilter filter;
        MeshRenderer render;

        if (go == null)
        { 
            tmp = new GameObject("element");
            tmp.transform.SetParent(gameObject.transform);
            tmp.transform.position = new Vector3(0, 0, -1);
            render = tmp.AddComponent(typeof(MeshRenderer))as MeshRenderer;
            filter = tmp.AddComponent(typeof(MeshFilter)) as MeshFilter;
        }
        else
        {
            tmp = go;
            render = tmp.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
            filter = tmp.GetComponent(typeof(MeshFilter)) as MeshFilter;
        }

        filter.mesh = msh;
        render.material = material;
        render.material.color = color;
        return tmp;
    }
}
