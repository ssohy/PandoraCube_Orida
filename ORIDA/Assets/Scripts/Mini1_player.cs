using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini1_player : MonoBehaviour
{
    
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;
    

    public int life; // ���
    public int score; // ����
    public int genCoins; // ���� ��
    public int saveCoins; 

    public float speed;
    public int power;
    public int maxPower;
    public int boom;
    public int maxBoom;

    public float maxShotDelay; // �Ѿ� ������ ��Ÿ��
    public float curShotDelay; // �Ѿ� ������ ��Ÿ��

    public Mini1_gameManager gameManager;
    public Mini1_objectManager objectManager;
    public bool isHit;
    public bool isBoomTime;


    public GameObject bulletObj; // �Ѿ� ������Ʈ
    //public GameObject boomEffect;

    public bool isRespawnTime;

    SpriteRenderer spriteRenderer;
    void Awake()
    {
        //anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable() // �׾��� �� ��������
    {
        Unbeatable();
        Invoke("Unbeatable", 3);
    }

    
    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;
        if (isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }
    void Update()
    {
        Move();
        Fire();
        ///Boom();
        Reload();

    }

    void Move() //�̵�
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;

        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

    }
  

    void Fire()
    {
        //#.���콺 Ŭ�� �� �߻�
        if (!Input.GetButton("Fire1"))
            return;

        //#.�߻� ������ ��Ÿ��
        if (curShotDelay < maxShotDelay)
            return;

        GameObject bullet = objectManager.MakeObj("BulletPlayer");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        //#.������ �ʱ�ȭ
        curShotDelay = 0;
    }

    //#.�Ѿ� ������
    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Boom()
    {

        if (isBoomTime)
        {
            return;
        }
        if (boom == 0)
        {
            return;
        }
        boom--;
        isBoomTime = true;
        Mini1_audioManager.instance.PlaySfx(Mini1_audioManager.Sfx.AttackB);
        gameManager.UpdateBoomIcon(boom);
        //boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 4f);
        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");

        for (int index = 0; index < enemiesL.Length; index++)
        {
            if (enemiesL[index].activeSelf)
            {
                Mini1_enemy enemyLogic = enemiesL[index].GetComponent<Mini1_enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Mini1_enemy enemyLogic = enemiesM[index].GetComponent<Mini1_enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Mini1_enemy enemyLogic = enemiesS[index].GetComponent<Mini1_enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        GameObject[] bulletsA = objectManager.GetPool("BulletEnemyA");
        GameObject[] bulletsB = objectManager.GetPool("BulletEnemyB");
        for (int index = 0; index < bulletsA.Length; index++)
        {
            if (bulletsA[index].activeSelf)
            {
                bulletsA[index].SetActive(false);
            }
        }
        for (int index = 0; index < bulletsB.Length; index++)
        {
            if (bulletsB[index].activeSelf)
            {
                bulletsB[index].SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border") //#.�� �浹 ����
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
            
        }
        else if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            
            //#.���������� ���
            if (isRespawnTime)
                return;
            
            //#.�¾��� ���
            if (isHit)
                return;

            isHit = true;

            life--;
            //Mini1_audioManager.instance.PlaySfx(Mini1_audioManager.Sfx.Dead);
            gameManager.UpdateLifeIcon(life);
            //gameManager.CallExplosion(transform.position, "P");

            if (life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
    }


    void OffBoomEffect()
    {
        //boomEffect.SetActive(false);
        isBoomTime = false;
    }


    //#.�� �浹 ����
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
            }
            
        }
    }

    public void CheckScore()
    {
        genCoins = score / 100;
        Debug.Log("������ ���� �� : " + genCoins);
        // PlayerPrefs�� ���� �� ����
        saveCoins = PlayerPrefs.GetInt("Coins", 0) + genCoins;
        PlayerPrefs.SetInt("Coins", saveCoins);
        PlayerPrefs.Save();
    }
}
