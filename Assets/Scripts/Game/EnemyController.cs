using UnityEngine;
using System.Collections.Generic;

public class EnemyController
{
    List<GameObject> activatedGroundEnemies = new List<GameObject>();
    List<GameObject> waitGroundEnemies = new List<GameObject>();

    List<GameObject> activatedWaterEnemies = new List<GameObject>();
    List<GameObject> waitWaterEnemies = new List<GameObject>();

    public EnemyController()
    {
        GameController.Inst.onStartGame += GameManager_onStartGame;
        GameController.Inst.onGameOver += GameManager_onGameOver;
        GameController.Inst.onLevelUp += GameManager_onLevelUp;
        GameController.Inst.onScoreUpdate += GameManager_onScoreUpdate;
	}

    private void GameManager_onScoreUpdate()
    {
        List<GameObject> wait4Remove = new List<GameObject>();
        for (int i = 0; i < activatedWaterEnemies.Count; i++)
        {
            if (!LineHelper.IsPointInPolygon(GameController.Inst.GetEnemyZone(), activatedWaterEnemies[i].transform.position))
            {
                activatedWaterEnemies[i].SetActive(false);
                waitWaterEnemies.Add(activatedWaterEnemies[i]);
                wait4Remove.Add(activatedWaterEnemies[i]);
            }
        }
        foreach (GameObject go in wait4Remove)
        {
            activatedWaterEnemies.Remove(go);
        }
        if (activatedWaterEnemies.Count == 0)
            GameController.Inst.LevelUp();
    }

    private void GameManager_onLevelUp(int level)
    {
        foreach (GameObject go in activatedGroundEnemies)
        {
            go.SetActive(false);
            waitGroundEnemies.Add(go);
        }
        foreach (GameObject go in activatedWaterEnemies)
        {
            go.SetActive(false);
            waitWaterEnemies.Add(go);
        }

        CreateNewGroundEnemy(level);
        CreateNewWaterEnemy(level + 1);
    }
    private void GameManager_onGameOver()
    {
        foreach (GameObject go in activatedGroundEnemies)
        {
            go.SetActive(false);
            waitGroundEnemies.Add(go);
        }
        foreach (GameObject go in activatedWaterEnemies)
        {
            go.SetActive(false);
            waitWaterEnemies.Add(go);
        }
    }
    private void GameManager_onStartGame()
    {
        CreateNewGroundEnemy(1);
        CreateNewWaterEnemy(1);
    }

    void CreateNewGroundEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go;
            if (waitGroundEnemies.Count < 1)
            {
                go = new GameObject();
                go.transform.SetParent(Camera.main.transform);
                go.SetActive(false);
                go.name = "GroundEnemy";
                SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load("background", typeof(Sprite)) as Sprite;
                spriteRenderer.color = Color.black;
                go.AddComponent<GroundEnemyBehaviour>();
            }
            else
            {
                go = waitGroundEnemies[0];
                waitGroundEnemies.Remove(go);
            }
            float topX = Screen.width - 25;
            go.transform.position = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(25 ,topX), 10));
            activatedGroundEnemies.Add(go);
            go.SetActive(true);
        }
    }

    void CreateNewWaterEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go;
            if (waitWaterEnemies.Count < 1)
            {
                go = new GameObject();
                go.transform.SetParent(Camera.main.transform);
                go.SetActive(false);
                go.name = "WaterEnemy";
                SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load("background", typeof(Sprite)) as Sprite;
                spriteRenderer.color = Color.white;
                go.AddComponent<WaterEnemyBehaviour>();
            }
            else
            {
                go = waitWaterEnemies[0];
                waitWaterEnemies.Remove(go);
            }

            float topX = Screen.width / 4;
            float topY = Screen.height/4;

            go.transform.position = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(topX, topX*2), Random.Range(topY, topY*2)));
            activatedWaterEnemies.Add(go);
            go.SetActive(true);
        }
    }
}
