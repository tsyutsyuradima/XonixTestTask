using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void ChangeDirection(Vector2 pos);
public delegate void SimpleAction();
public delegate void IntAction(int value);
public delegate void Vector3Action(Vector3 pos);
public delegate void SwipeAction(Direction dir);

/// <summary>
/// 
///     GLOABAL Game Controller
/// 
/// </summary>

public class GameController : MonoBehaviour
{
    public static float playerSpeed = 0.04f;
    public static float enemySpeed = 0.04f;


    static GameController inst;
    public static GameController Inst { get { return inst; } set { } }

    public event ChangeDirection onChangeDirection;
    public event Vector3Action onStartMoving;
    public event Vector3Action onEndMoving;
    public event Vector3Action onBroken;

    public event SimpleAction onScoreUpdate;
    public event IntAction onLevelUp;
    public event SimpleAction onStartGame;
    public event SimpleAction onPauseGame;
    public event SimpleAction onResumeGame;
    public event SimpleAction onGameOver;

    [SerializeField]
    PlayerBehaviour player;
    [SerializeField]
    ZoneManager zoneManager;
    [SerializeField]
    DrawLine drawLine;
    [SerializeField]
    AudioClip levelUp;
    [SerializeField]
    AudioClip gameOver;
    [SerializeField]
    AudioClip lose;
    [SerializeField]
    AudioSource audioSource;

    int currentScore = 0;
    int currentLevel = 0;
    int currentHearts = 3;
    float currentLevelTime = 6000f;
    int fullEnemySquare = 0;
    float fillingProcentLevelUp = 0.8f;
    public bool isDrawingNewZone = false;
    Vector3 playerPosition;
    EnemyController enemyController;

    void Awake ()
    {
        inst = this;
    }
    void Start()
    {
        enemyController = new EnemyController();
        player.onChangeDirection += Player_onChangeDirection;
    }

    /// <summary>
    /// This Update is responsible for cheking is player in collider;
    /// </summary>
    void FixedUpdate()
    {
        Vector3 newPos = GetPlayerPos();
        if (playerPosition != newPos)
        {
            bool isInPoligon = LineHelper.IsPointInPolygon(zoneManager.currentPosArray, newPos);
            if (!isDrawingNewZone && isInPoligon)
            {
                isDrawingNewZone = true;

                if (onStartMoving != null)
                    onStartMoving(newPos);
            }
            if (isDrawingNewZone && !isInPoligon)
            {
                isDrawingNewZone = false;

                if (onEndMoving != null)
                    onEndMoving(newPos);
                player.StopMoving();
            }
            playerPosition = newPos;
        }
    }

    public void StartGame()
    {
        currentScore = 0;
        currentLevel = 1;
        currentHearts = 3;
        isDrawingNewZone = false;

        if (onStartGame != null)
            onStartGame();
    }

    public void PauseGame()
    {
        if (onPauseGame != null)
            onPauseGame();
    }

    public void ResumeGame()
    {
        if (onResumeGame != null)
            onResumeGame();
    }

    public void LevelUp()
    {
        audioSource.Stop();
        audioSource.clip = levelUp;
        audioSource.Play();
        currentLevel++;
        currentHearts = 3;
        player.ResetPlayer();

        if (onLevelUp != null)
            onLevelUp(currentLevel);
    }

    public void GameOver()
    {
        audioSource.Stop();
        audioSource.clip = gameOver;
        audioSource.Play();

        if (onGameOver != null)
            onGameOver();

        isDrawingNewZone = false;
    }

    public void Broken()
    {
        isDrawingNewZone = false;
        audioSource.Stop();
        currentHearts--;

        if (onBroken != null)
        {
            audioSource.clip = lose;
            onBroken(player.CurrentPosition.position);
        }  
        if (currentHearts == 0 && onGameOver != null)
        {
            audioSource.clip = gameOver;
            onGameOver();
        }
        audioSource.Play();
    }

    public void CalculateNewScore(float OpenZoneSquare, float LeftZoneSquare)
    {
        if ((fullEnemySquare - LeftZoneSquare) / fullEnemySquare >= fillingProcentLevelUp)
        {
            LevelUp();
        }

        currentScore += (int)OpenZoneSquare *  (currentLevel + 1);
        if (onScoreUpdate != null)
            onScoreUpdate();
    }

    public void SetFullEnemySquare(int fullEnemySquare)
    {
        this.fullEnemySquare = fullEnemySquare;
    }
    public float GetLevelTime()
    {
        return currentLevelTime;
    }
    public int GetCurrentScore()
    {
        return currentScore;
    }
    public int GetHearts()
    {
        return currentHearts;
    }
    public Vector3 GetPlayerPos()
    {
        return player.CurrentPosition.position;
    }

    public List<Vector2> GetEnemyZone()
    {
        return zoneManager.currentPosArray;
    }

    public List<Vector2> GetDrawLine()
    {
        return drawLine.GetPointsList();
    }

    private void Player_onChangeDirection(Vector2 pos)
    {
        if (isDrawingNewZone && onChangeDirection != null)
            onChangeDirection(pos);
    }
}
