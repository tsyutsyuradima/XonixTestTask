using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{
    private LineRenderer line;
    private bool isDrawingLine;
    private bool isPause;
    private List<Vector2> pointsList;

    void Awake()
    {
        
    }

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        //line.material = new Material(Shader.Find("Unlit/Color"));
        //line.SetVertexCount(0);
        //line.SetWidth(0.05f, 0.05f);
        //line.SetColors(Color.white, Color.white);
        //line.useWorldSpace = true;
        isDrawingLine = false;
        isPause = false;
        pointsList = new List<Vector2>();
        GameController.Inst.onStartMoving += Player_onStartMoving;
        GameController.Inst.onEndMoving += Player_onEndMoving;
        GameController.Inst.onPauseGame += Player_onPauseGame;
        GameController.Inst.onResumeGame += Player_onResumeGame;
    }

    private void Player_onResumeGame()
    {
        isPause = false;
    }
    private void Player_onPauseGame()
    {
        isPause = true;
    }
    private void Player_onEndMoving(Vector3 pos)
    {
        isDrawingLine = false;
        Refresh();
    }
    private void Player_onStartMoving(Vector3 pos)
    {
        Refresh();
        isDrawingLine = true;
        pointsList.Add(pos);
        line.SetVertexCount(pointsList.Count);
        line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
    }

    void Refresh()
    {
        line.SetVertexCount(0);
        pointsList.Clear();
    }

    void FixedUpdate()
    {
        if (isDrawingLine && !isPause)
        {
            Vector2 targetPos = GameController.Inst.GetPlayerPos();
            if (pointsList.Contains(targetPos))
            {
                isDrawingLine = false;
                Refresh();
                GameController.Inst.Broken();
                return;
            }

            pointsList.Add(targetPos);
            line.SetVertexCount(pointsList.Count);
            line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);

            if (LineHelper.isLineCollide(pointsList))
            {
                isDrawingLine = false;
                Refresh();
                GameController.Inst.Broken();
                return;
            }
        }
    }
}