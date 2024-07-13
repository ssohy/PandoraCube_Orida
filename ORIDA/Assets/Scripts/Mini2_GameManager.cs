using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mini2_GameManager : MonoBehaviour
{
    public GameObject stairPrefab; // ��� ������
    public int initialStairs = 10; // �ʱ� ���� ��� ��
    public float stepHeight = 1.0f; // �� ����� ����
    public float stepWidth = 2.5f; // �� ����� �ʺ�
    public float leftBoundary = -7.0f; // ���� ���
    public float rightBoundary = 7.0f; // ������ ���

    private List<GameObject> stairs = new List<GameObject>();
    private Transform backgroundTransform;

    public GameObject gameOverSet;

    void Awake()
    {
        backgroundTransform = new GameObject("Background").transform;
        GenerateInitialStairs();
    }

    void GenerateInitialStairs()
    {
        // ù ��° ����� Ư�� ��ġ�� ����
        Vector3 firstStairPosition = new Vector3(1, -3, 0);
        GameObject firstStair = Instantiate(stairPrefab, firstStairPosition, Quaternion.identity, backgroundTransform);
        firstStair.layer = LayerMask.NameToLayer("Stair");
        stairs.Add(firstStair);

        for (int i = 1; i < initialStairs; i++)
        {
            GenerateNewStair();
        }
    }

    public void GenerateNewStair()
    {
        Vector3 newPosition = Vector3.zero;
        if (stairs.Count > 0)
        {
            Vector3 lastStairPosition = stairs[stairs.Count - 1].transform.position;

            // �������� �¿� �̵�
            float randomX = Random.Range(-stepWidth, stepWidth);
            newPosition = new Vector3(
                Mathf.Clamp(lastStairPosition.x + randomX, leftBoundary, rightBoundary),
                lastStairPosition.y + stepHeight,
                lastStairPosition.z
            );
        }
        GameObject newStair = Instantiate(stairPrefab, newPosition, Quaternion.identity, backgroundTransform);
        newStair.layer = LayerMask.NameToLayer("Stair"); // ��ܿ� Stair ���̾� ����
        stairs.Add(newStair);
    }

    public void MoveBackground(float moveDistance)
    {
        backgroundTransform.position += new Vector3(0, moveDistance, 0);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
        //Mini1_audioManager.instance.PlaySfx(Mini1_audioManager.Sfx.Gameover);
        //  AudioManager.instance.PlayBgm(false);

        //GetComponent<AudioSource>().Stop();
    }

    public void homeButton()
    {
        SceneManager.LoadScene("Main");
    }
}
