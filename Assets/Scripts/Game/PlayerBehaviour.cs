using UnityEngine;
using System.Collections;
using System;

public enum Direction { Left, Up, Down, Right, TopLeft, TopRight, BottomRight, BottomLeft, NONE };

/// <summary>
///     This class is Responsible for moving "player".
///     
/// </summary>
/// 
public class PlayerBehaviour : MonoBehaviour
{
    public event ChangeDirection onChangeDirection;

    Direction currentDirection;
    Direction newDirection;
    public Transform CurrentPosition { get { return transform; } set { } }

    Vector3 topLeftPos;
    Vector3 bottomRightPos;
    int topX;
    int topY;

    bool canMoving = false;

    void Start()
    {
        GameController.Inst.onStartGame += Inst_onStartGame;
        GameController.Inst.onPauseGame += Inst_onPauseGame;
        GameController.Inst.onResumeGame += Inst_onResumeGame;
        GameController.Inst.onBroken += Inst_onBroken;


        topX = Screen.width / 2;
        topY = Screen.height - 10;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector2(topX, topY));
        topLeftPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        bottomRightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 5, 5));
        gameObject.AddComponent<SwipeControl>().onSwipe += PlayerBehaviour_onSwipe;
    }

    private void Inst_onBroken(Vector3 pos)
    {
        ResetPlayer();
    }

    private void Inst_onResumeGame()
    {
        canMoving = true;
        newDirection = currentDirection;
    }

    private void Inst_onPauseGame()
    {
        canMoving = false;
        newDirection = Direction.NONE;
    }

    private void Inst_onStartGame()
    {
        canMoving = true;
        ResetPlayer();
    }
    private void PlayerBehaviour_onSwipe(Direction dir)
    {
        newDirection = dir;
    }

    public void ResetPlayer()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector2(topX, topY));
        currentDirection = Direction.NONE;
        newDirection = Direction.NONE;
    }

    public void StopMoving()
    {
        currentDirection = Direction.NONE;
        newDirection = Direction.NONE;
    }

    void FixedUpdate ()
    {
        if (canMoving)
        {
            Vector3 pos = transform.position;
            switch (newDirection)
            {
                case Direction.Left:
                    if (pos.y < topLeftPos.y)
                    {
                        if (currentDirection != Direction.Left) onChangeDirection(pos);
                        pos.y += GameController.playerSpeed;
                        pos.y = (float)Math.Round(pos.y, 2);
                        currentDirection = Direction.Left;
                    }
                    break;
                case Direction.Up:
                    if (pos.x > topLeftPos.x)
                    {
                        if (currentDirection != Direction.Up) onChangeDirection(pos);
                        pos.x -= GameController.playerSpeed;
                        pos.x = (float)Math.Round(pos.x, 2);
                        currentDirection = Direction.Up;
                    }
                    break;
                case Direction.Down:
                    if (pos.y > bottomRightPos.y)
                    {
                        if (currentDirection != Direction.Down) onChangeDirection(pos);
                        pos.y -= GameController.playerSpeed;
                        pos.y = (float)Math.Round(pos.y, 2);
                        currentDirection = Direction.Down;
                    }
                    break;
                case Direction.Right:
                    if (pos.x < bottomRightPos.x)
                    {
                        if (currentDirection != Direction.Right) onChangeDirection(pos);
                        pos.x += GameController.playerSpeed;
                        pos.x = (float)Math.Round(pos.x, 2);
                        currentDirection = Direction.Right;
                    }
                    break;
            }
            transform.transform.position = pos;
        }
    }
}
