using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini1_objectManager : MonoBehaviour
{
    /*
    public GameObject enemyBPrefab;

    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    */
    public GameObject enemyPrefab;
    public GameObject bulletPlayerPrefab;
    
    public GameObject bulletEnemyPrefab;
    /*
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletFollowerPrefab;
    public GameObject bulletBossAPrefab;
    public GameObject bulletBossBPrefab;
    public GameObject explosionPrefab;

    GameObject[] enemyB;
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;
    */
    GameObject[] enemy;
    GameObject[] bulletPlayer;

    /*
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletFollower;
    GameObject[] bulletBossA;
    GameObject[] bulletBossB;
    GameObject[] explosion;

    */
    GameObject[] bulletEnemy;
    GameObject[] targetPool;

    void Awake()
    {
        enemy = new GameObject[10];
        bulletPlayer = new GameObject[100];
        bulletEnemy = new GameObject[100];

        /*
        explosion = new GameObject[20];
        */

        Generate();
    }

    void Generate()
    {
        
        //#1.Enemy
        for (int index = 0; index < enemy.Length; index++)
        {
            enemy[index] = Instantiate(enemyPrefab);
            enemy[index].SetActive(false);
        }
        /*
        for (int index = 0; index < enemyL.Length; index++)
        {
            enemyL[index] = Instantiate(enemyLPrefab);
            enemyL[index].SetActive(false);
        }
        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(enemyMPrefab);
            enemyM[index].SetActive(false);

        }
        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefab);
            enemyS[index].SetActive(false);
        }

        */

        //#3.Bullet
        for (int index = 0; index < bulletPlayer.Length; index++)
        {
            bulletPlayer[index] = Instantiate(bulletPlayerPrefab);
            bulletPlayer[index].SetActive(false);
        }

        
        for (int index = 0; index < bulletEnemy.Length; index++)
        {
            bulletEnemy[index] = Instantiate(bulletEnemyPrefab);
            bulletEnemy[index].SetActive(false);
        }
        /*
        for (int index = 0; index < bulletEnemyB.Length; index++)
        {
            bulletEnemyB[index] = Instantiate(bulletEnemyBPrefab);
            bulletEnemyB[index].SetActive(false);
        }
        for (int index = 0; index < bulletFollower.Length; index++)
        {
            bulletFollower[index] = Instantiate(bulletFollowerPrefab);
            bulletFollower[index].SetActive(false);
        }

        for (int index = 0; index < bulletBossA.Length; index++)
        {
            bulletBossA[index] = Instantiate(bulletBossAPrefab);
            bulletBossA[index].SetActive(false);
        }
        for (int index = 0; index < bulletBossB.Length; index++)
        {
            bulletBossB[index] = Instantiate(bulletBossBPrefab);
            bulletBossB[index].SetActive(false);
        }

        //#4.Explosion(Animation)
        for (int index = 0; index < explosion.Length; index++)
        {
            explosion[index] = Instantiate(explosionPrefab);
            explosion[index].SetActive(false);
        }
        */
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            
            case "Enemy":
                targetPool = enemy;
                break;
            case "BulletPlayer":
                targetPool = bulletPlayer;
                break;   
            case "BulletEnemy":
                targetPool = bulletEnemy;
                break;
                /*
            case "Explosion":
                targetPool = explosion;
                break;
                */
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                Debug.Log(type);
                targetPool[index].SetActive(true);

                return targetPool[index];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            
            case "Enemy":
                targetPool = enemy;
                break;
            /*
        case "EnemyL":
            targetPool = enemyL;
            break;
        case "EnemyM":
            targetPool = enemyM;
            break;
        case "EnemyS":
            targetPool = enemyS;
            break;
        */
            case "BulletPlayer":
                targetPool = bulletPlayer;
                break;
            
            case "BulletEnemy":
                targetPool = bulletEnemy;
                break;
                /*
            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "BulletFollower":
                targetPool = bulletFollower;
                break;
            case "BulletBossA":
                targetPool = bulletBossA;
                break;
            case "BulletBossB":
                targetPool = bulletBossB;
                break;
            case "explosion":
                targetPool = explosion;
                break;
            */
        }

        return targetPool;
    }
}
