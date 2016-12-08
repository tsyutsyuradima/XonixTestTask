using UnityEngine;
using System.Collections;
using System;

public enum Orientation { Vertical, Horizontal, NONE}

public class EnemyBehaviour : MonoBehaviour
{
    protected Direction currentDirection;
    protected Orientation collideOrientation = Orientation.NONE;
    protected bool canMoving = true;

    public virtual void Start()
    {
        GameController.Inst.onPauseGame += GameManager_onPauseGame;
        GameController.Inst.onResumeGame += GameManager_onResumeGame;
    }

    public void GameManager_onPauseGame()
    { canMoving = false;  }

    public void GameManager_onResumeGame()
    { canMoving = true; }

    public virtual void CheckDirrection()
    {
        switch (collideOrientation)
        {
            case Orientation.Horizontal:
                switch (currentDirection)
                {
                    case Direction.TopLeft:
                        currentDirection = Direction.BottomLeft;
                        break;
                    case Direction.TopRight:
                        currentDirection = Direction.BottomRight;
                        break;
                    case Direction.BottomRight:
                        currentDirection = Direction.TopRight;
                        break;
                    case Direction.BottomLeft:
                        currentDirection = Direction.TopLeft;
                        break;
                }
                break;

            case Orientation.Vertical:
                switch (currentDirection)
                {
                    case Direction.TopLeft:
                        currentDirection = Direction.TopRight;
                        break;
                    case Direction.TopRight:
                        currentDirection = Direction.TopLeft;
                        break;
                    case Direction.BottomRight:
                        currentDirection = Direction.BottomLeft;
                        break;
                    case Direction.BottomLeft:
                        currentDirection = Direction.BottomRight;
                        break;
                }
                break;
        }
        collideOrientation = Orientation.NONE;
    }


    public void FixedUpdate()
    {
        if (canMoving)
        {
            Vector3 pos = transform.position;
            switch (currentDirection)
            {
                case Direction.TopLeft:
                    pos.x = (float)Math.Round(pos.x -= GameController.enemySpeed, 2);
                    pos.y = (float)Math.Round(pos.y += GameController.enemySpeed, 2);
                    break;
                case Direction.TopRight:
                    pos.x = (float)Math.Round(pos.x += GameController.enemySpeed, 2);
                    pos.y = (float)Math.Round(pos.y += GameController.enemySpeed, 2);
                    break;
                case Direction.BottomRight:
                    pos.x = (float)Math.Round(pos.x += GameController.enemySpeed, 2);
                    pos.y = (float)Math.Round(pos.y -= GameController.enemySpeed, 2);
                    break;
                case Direction.BottomLeft:
                    pos.x = (float)Math.Round(pos.x -= GameController.enemySpeed, 2);
                    pos.y = (float)Math.Round(pos.y -= GameController.enemySpeed, 2);
                    break;
            }
            transform.position = pos;
            CheckDirrection();
        }
    }
}