using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject PausePanel;
    [SerializeField]
    GameObject HUD;
    [SerializeField]
    AudioSource audioSource;

    void Start ()
    {
        GameController.Inst.onStartGame += GameManager_onStartGame;
        GameController.Inst.onPauseGame += GameManager_onPauseGame;
        GameController.Inst.onResumeGame += GameManager_onResumeGame;
        GameController.Inst.onGameOver += GameManager_onGameOver;
	}

    private void GameManager_onStartGame()
    {
        MainMenu.SetActive(false);
        HUD.SetActive(true);
        audioSource.Play();
    }
    private void GameManager_onPauseGame()
    {
        PausePanel.SetActive(true);
    }
    private void GameManager_onResumeGame()
    {
        PausePanel.SetActive(false);
    }
    private void GameManager_onGameOver()
    {
        audioSource.Stop();
        HUD.SetActive(false);
        MainMenu.SetActive(true);
    }
}
