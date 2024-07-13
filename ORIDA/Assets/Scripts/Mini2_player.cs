using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini2_player : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    int curSprite = 0;

    public float stepHeight = 1.0f; // �� ����� ����
    public float moveSpeed = 2.0f; // �̵� �ӵ�

    private bool isMoving = false;
    private Vector3 targetPosition;

    private Mini2_GameManager mapGenerator;
    public Mini2_GameManager gameManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        mapGenerator = FindObjectOfType<Mini2_GameManager>();
    }

    void Update()
    {
        if (!isMoving)
        {
            UP();
            Turn();
        }
        else
        {
            MoveToTarget();
        }
    }

    void UP() // �ö󰡱�
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W����");
            targetPosition = transform.position + new Vector3(0, stepHeight, 0);

            // �Ʒ��� ����� �ִ��� Ȯ��
            Vector3 belowPosition = transform.position - new Vector3(0, stepHeight / 2, 0);
            Collider2D hit = Physics2D.OverlapCircle(belowPosition, 0.1f, LayerMask.GetMask("Stair"));
            if (hit == null)
            {
                gameManager.GameOver();
                return;
            }

            isMoving = true;

            // ���ο� ��� ���� ��û
            mapGenerator.GenerateNewStair();
        }
    }

    void Turn() // ������ȯ
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (curSprite == 1)
            {
                spriteRenderer.sprite = sprites[0];
                curSprite = 0;
            }
            else
            {
                spriteRenderer.sprite = sprites[1];
                curSprite = 1;
            }
        }
    }

    void MoveToTarget()
    {
        mapGenerator.MoveBackground(-moveSpeed * Time.deltaTime);
        if (transform.position.y <= targetPosition.y)
        {
            isMoving = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bottom")
        {
            gameManager.GameOver();
        }
    }
}
