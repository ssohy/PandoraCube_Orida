using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini1_objectManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bulletPlayerPrefab;
    public GameObject bulletEnemyPrefab;

    GameObject[] enemy;
    GameObject[] bulletPlayer;
    GameObject[] bulletEnemy;
    GameObject[] targetPool;

    void Awake()
    {
        enemy = new GameObject[10];
        bulletPlayer = new GameObject[100];
        bulletEnemy = new GameObject[100];

        Generate();
    }

    void Generate()
    {
        
        //#.Enemy
        for (int index = 0; index < enemy.Length; index++)
        {
            enemy[index] = Instantiate(enemyPrefab);
            enemy[index].SetActive(false);
        }

        //#.Bullet
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
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
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
            case "BulletPlayer":
                targetPool = bulletPlayer;
                break;
            
            case "BulletEnemy":
                targetPool = bulletEnemy;
                break;
        }

        return targetPool;
    }
}
