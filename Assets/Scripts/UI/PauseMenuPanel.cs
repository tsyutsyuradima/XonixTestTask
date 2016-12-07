using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuPanel : MonoBehaviour
{
    [SerializeField]
    Button BtnPlay;

    void Start ()
    {
        BtnPlay.onClick.AddListener(() => OnBtnPlayClick());
	}
	
	void OnBtnPlayClick ()
    {
        GameController.Inst.ResumeGame();
	}
}
