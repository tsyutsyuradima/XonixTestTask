using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartController : MonoBehaviour
{
    [SerializeField]
    List<Image> Hearts;

    Image Heart4Hide;

    void Start ()
    {
        GameController.Inst.onStartGame += GameManager_onStartGame; ;
        GameController.Inst.onBroken += GameManager_onBroken;
        GameController.Inst.onLevelUp += GameManager_onLevelUp;
	}

    private void GameManager_onLevelUp()
    {
        ResetHearts();
    }

    private void GameManager_onStartGame()
    {
        ResetHearts();
    }

    private void GameManager_onBroken(Vector3 pos)
    {
        int currentHearts = GameController.Inst.GetHearts();
        if (Hearts.Count > currentHearts)
        {
            Heart4Hide = Hearts[currentHearts];
            Heart4Hide.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    }

    void ResetHearts()
    {
        Heart4Hide = null;
        foreach (Image img in Hearts)
        {
            Color color = img.color;
            color.a = 1f;
            img.color = color;
            img.transform.localScale = Vector3.one;
        }
    }

    void Update()
    {
        if (Heart4Hide != null)
        {
            Vector3 scale = Heart4Hide.transform.localScale;
            scale.x = scale.y = scale.x - 0.005f;
            Heart4Hide.transform.localScale=scale;

            Color color = Heart4Hide.color;
            color.a -= 0.01f;
            Heart4Hide.color = color;
            if (color.a <= 0.4)
                Heart4Hide = null;
        }
    }
}
