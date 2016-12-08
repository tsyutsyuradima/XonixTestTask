using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{
    private LineRenderer line;
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
        isPause = false;
        pointsList = new List<Vector2>();
        GameController.Inst.onStartMoving += Player_onStartMoving;
        GameController.Inst.onEndMoving += Player_onEndMoving;
        GameController.Inst.onPauseGame += Player_onPauseGame;
        GameController.Inst.onResumeGame += Player_onResumeGame;
        GameController.Inst.onBroken += Player_onBroken;
    }

    private void Player_onBroken(Vector3 pos)
    {
        line.SetVertexCount(0);
        pointsList.Clear();
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
        Refresh();
    }
    private void Player_onStartMoving(Vector3 pos)
    {
        Refresh();
        pointsList.Add(pos);
        line.SetVertexCount(pointsList.Count);
        line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
    }

    public List<Vector2> GetPointsList()
    {
        return pointsList;
    }

    void Refresh()
    {
        line.SetVertexCount(0);
        pointsList.Clear();
    }

    void FixedUpdate()
    {
        if (GameController.Inst.isDrawingNewZone && !isPause)
        {
            Vector2 targetPos = GameController.Inst.GetPlayerPos();
            if (pointsList.Contains(targetPos))
            {
                Refresh();
                GameController.Inst.Broken();
                return;
            }

            pointsList.Add(targetPos);
            line.SetVertexCount(pointsList.Count);
            line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);

            if (LineHelper.isLineCollide(pointsList))
            {
                Refresh();
                GameController.Inst.Broken();
                return;
            }
        }
    }
}