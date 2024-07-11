using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini1_enemy : MonoBehaviour
{
    public int enemyScore;
    public string enemyName;

    public float speed;
    public int health;
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    Animator anim;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject player;
    public Mini1_objectManager objectManager;
    public Mini1_gameManager gameManager;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {

        Fire();
        Reload();
    }

    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;

        Debug.Log("¸Â´ÂÁß");
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);


        if (health <= 0)
        {
            Mini1_player playerLogic = player.GetComponent<Mini1_player>();
            playerLogic.score += enemyScore;

            //int ran = enemyName == "B" ? 0 : Random.Range(0, 10);

            

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            gameManager.CallExplosion(transform.position, enemyName);

        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            //Mini1_bullet bullet = collision.gameObject.GetComponent<Mini1_bullet>();
            OnHit(5);

            collision.gameObject.SetActive(false);
        }
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
        {
            return;
        }
        if (enemyName == "enemy")
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemy");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        
        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
