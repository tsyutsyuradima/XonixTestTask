using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField]
    Button BtnPlay;

    void Start ()
    {
        BtnPlay.onClick.AddListener(() => OnBtnPlayClick());
	}

    void OnBtnPlayClick()
    {
        GameController.Inst.StartGame();
    }
}
