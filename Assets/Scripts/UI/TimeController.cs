using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    Image FildImage;
    [SerializeField]
    Text Timer;

    bool ready;
    float allTime;
    float currentTime;

    void Start()
    {
        GameController.Inst.onPauseGame += GameManager_onPauseGame;
        GameController.Inst.onResumeGame += GameManager_onResumeGame;
        GameController.Inst.onGameOver += GameManager_onGameOver;
        GameController.Inst.onLevelUp += GameManager_onLevelUp;

        allTime = GameController.Inst.GetLevelTime();
    }


    void OnEnable()
    {
        ready = true;
        currentTime = 0;
        FildImage.fillAmount = currentTime;
    }

    private void GameManager_onLevelUp()
    {
        currentTime = 0;
        allTime = GameController.Inst.GetLevelTime();
    }

    private void GameManager_onGameOver()
    {
        ready = false;
        currentTime = 0;
    }

    private void GameManager_onResumeGame()
    {
        ready = true;
    }

    private void GameManager_onPauseGame()
    {
        ready = false;
    }

    void FixedUpdate ()
    {
        if (ready && FildImage != null)
        {
            currentTime++;
            int leftTime = (int)((allTime / 100) - (currentTime / 100));
            Timer.text = leftTime.ToString();

            float procent = currentTime / allTime;
            FildImage.fillAmount = procent;
            if (procent >= 1f)
            {
                ready = false;
                GameController.Inst.GameOver();
            }
        }
    }
}
