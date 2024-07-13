using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class Mini1_gameManager : MonoBehaviour
{
    public int stage;
    public Transform playerPos;

    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Mini1_player forCoinsPlayer;
    public TMP_Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public Mini1_objectManager objectManager;

    public List<Mini1_spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    public GameObject pauseUI;





    void Awake()
    {
        Time.timeScale = 0f;
        spawnList = new List<Mini1_spawn>();
        enemyObjs = new string[] {"Enemy"};
    }

    public void Restart() // 일시정지 -> 게임으로 돌아가기
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
    }


    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay)
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        // #.UI Score Update
        Mini1_player playerLogic = player.GetComponent<Mini1_player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;

        int enemyPoint = Random.Range(0,7);
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Mini1_enemy enemyLogic = enemy.GetComponent<Mini1_enemy>();
        enemyLogic.player = player;
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;

        
        rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        

        //spawnIndex++;
        /*
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }*/

        nextSpawnDelay = Random.Range(1.0f, 3f);
    }

    public void UpdateLifeIcon(int life)
    {
        // #.UI Life Init Disable
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        // #.UI Life Init Disable
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < 3; index++)
            boomImage[index].color = new Color(1, 1, 1, 0);

        for (int index = 0; index < boom; index++)
            boomImage[index].color = new Color(1, 1, 1, 1);
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
        player.transform.position = Vector3.down * 4f;
        player.SetActive(true);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 4f;
        player.SetActive(true);
        Mini1_player playerLogic = player.GetComponent<Mini1_player>();
        playerLogic.isHit = false;
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        //GameObject explosion = objectManager.MakeObj("Explosion");
        //Mini1_explosion explosionLogic = explosion.GetComponent<Mini1_explosion>();

        //explosion.transform.position = pos;
        //explosionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        forCoinsPlayer.CheckScore();
        gameOverSet.SetActive(true);
        //Mini1_audioManager.instance.PlaySfx(Mini1_audioManager.Sfx.Gameover);
        //  AudioManager.instance.PlayBgm(false);

        //GetComponent<AudioSource>().Stop();

    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void homeButton()
    {
        SceneManager.LoadScene("Main");
    }



}
