using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour
{
    public Text ScoreCount;

	void Start ()
    {
        GameController.Inst.onStartGame += GameManager_onStartGame; ;
        GameController.Inst.onScoreUpdate += GameManager_onScoreUpdate;
	}

    private void GameManager_onStartGame()
    {
        ScoreCount.text = GameController.Inst.GetCurrentScore().ToString();
    }

    private void GameManager_onScoreUpdate()
    {
        ScoreCount.text = GameController.Inst.GetCurrentScore().ToString();
    }
}
