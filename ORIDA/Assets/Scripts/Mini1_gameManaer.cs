using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Mini1_gameManager : MonoBehaviour
{
    public int stage;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform playerPos;

    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public Mini1_objectManager objectManager;

    public List<Mini1_spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    void Awake()
    {
        spawnList = new List<Mini1_spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };
        StageStart();
    }

    public void StageStart()
    {
        //#.Stage UI Load
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart";
        clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nClear";

        //#.Enemy Spawn File Read
        ReadSpawnFile();

        //#.Fade In
        fadeAnim.SetTrigger("In");
    }

    public void StageEnd()
    {
        //#.Clear UI Load
        clearAnim.SetTrigger("On");
        Mini1_audioManager.instance.PlaySfx(Mini1_audioManager.Sfx.Gameclear);


        //#.Fade Out
        fadeAnim.SetTrigger("Out");

        //#.Player Reposition 
        player.transform.position = playerPos.position;

        //#.Stage Increament
        stage++;
        if (stage > 2)
            Invoke("GameOver", 6);

        else
            Invoke("StageStart", 5);


    }

    void ReadSpawnFile()
    {
        //#1.���� �ʱ�ȭ
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //#2. ������ ���� �б�
        TextAsset textFile = Resources.Load("Stage " + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        //#3. �� �پ� ������ ����
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
                break;

            Mini1_spawn spawnData = new Mini1_spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        stringReader.Close();

        nextSpawnDelay = spawnList[0].delay;
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !spawnEnd)
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

        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }

        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Mini1_enemy enemyLogic = enemy.GetComponent<Mini1_enemy>();
        enemyLogic.player = player;
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;

        if (enemyPoint == 5 || enemyPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

        spawnIndex++;

        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].delay;
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
        GameObject explosion = objectManager.MakeObj("Explosion");
        Mini1_explosion explosionLogic = explosion.GetComponent<Mini1_explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
        Mini1_audioManager.instance.PlaySfx(Mini1_audioManager.Sfx.Gameover);
        //  AudioManager.instance.PlayBgm(false);

        //GetComponent<AudioSource>().Stop();

    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

}
