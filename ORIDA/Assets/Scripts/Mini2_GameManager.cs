using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Mini2_GameManager : MonoBehaviour
{
    public Mini2_player player;
    public GameObject[] players, stairs, UI;
    public GameObject backGround;
    public TMP_Text scoreText;
    public Image gauge;
    int score;
    public bool gaugeStart = false;
    float gaugeRedcutionRate = 0.0025f;
    public bool[] IsChangeDir = new bool[20];
    private bool isGameOver = false; // 게임 오버 상태 추적
    public TMP_Text coinsText;

    Vector3 beforePos,
    startPos = new Vector3(-1.6f, -4.0f, 0),
    leftPos = new Vector3(-2.6f, 0.8f, 0),
    rightPos = new Vector3(2.6f, 0.8f, 0),
    leftDir = new Vector3(2.6f, -0.8f, 0),
    rightDir = new Vector3(-2.6f, -0.8f, 0);

    enum State { start, leftDir, rightDir }
    State state = State.start;

    public GameObject gameOver;
    public GameObject pauseUI;

    public int genCoins; // 코인 수
    public int saveCoins;

    void Awake()
    {
        players[0].SetActive(true);
        player = players[0].GetComponent<Mini2_player>();

        StairsInit();
        GaugeReduce();
        StartCoroutine("CheckGauge");
    }

    void StairsInit()
    {
        for (int i = 0; i < 20; i++)
        {
            switch (state)
            {
                case State.start:
                    stairs[i].transform.position = startPos;
                    state = State.leftDir;
                    break;
                case State.leftDir:
                    stairs[i].transform.position = beforePos + leftPos;
                    break;
                case State.rightDir:
                    stairs[i].transform.position = beforePos + rightPos;
                    break;
            }
            beforePos = stairs[i].transform.position;

            if (i != 0)
            {
                if (Random.Range(1, 9) < 3 && i < 19)
                {
                    if (state == State.leftDir) state = State.rightDir;
                    else if (state == State.rightDir) state = State.leftDir;
                    IsChangeDir[(i + 1) % 20] = true;
                }
            }
        }
    }

    //Spawn The Stairs At The Random Location
    void SpawnStair(int num)
    {
        IsChangeDir[(num + 1) % 20] = false;
        beforePos = stairs[num == 0 ? 19 : num - 1].transform.position;
        switch (state)
        {
            case State.leftDir:
                stairs[num].transform.position = beforePos + leftPos;
                break;
            case State.rightDir:
                stairs[num].transform.position = beforePos + rightPos;
                break;
        }

        if (Random.Range(1, 9) < 3)
        {
            if (state == State.leftDir) state = State.rightDir;
            else if (state == State.rightDir) state = State.leftDir;
            IsChangeDir[(num + 1) % 20] = true;
        }
    }

    public void StairMove(int stairIndex, bool isChange, bool isleft)
    {
        if (player.isDie || isGameOver) return;

        //Move stairs to the right or left
        for (int i = 0; i < 20; i++)
        {
            if (isleft) stairs[i].transform.position += leftDir;
            else stairs[i].transform.position += rightDir;
        }

        //Move the stairs below a certain height
        for (int i = 0; i < 20; i++)
            if (stairs[i].transform.position.y < -5) SpawnStair(i);

        //Game over if climbing stairs is wrong
        if (IsChangeDir[(stairIndex + 1) % 20] != isChange)
        {
            GameOver();
            return;
        }

        //Score Update & Gauge Increase
        scoreText.text = (++score).ToString();
        gauge.fillAmount += 0.7f;
        backGround.transform.position += backGround.transform.position.y < -14f ?
            new Vector3(0, 4.7f, 0) : new Vector3(0, -0.05f, 0);
    }

    void GaugeReduce()
    {
        if (gaugeStart)
        {
            //Gauge Reduction Rate Increases As Score Increases
            if (score > 30) gaugeRedcutionRate = 0.0033f;
            if (score > 60) gaugeRedcutionRate = 0.0037f;
            if (score > 100) gaugeRedcutionRate = 0.0043f;
            if (score > 150) gaugeRedcutionRate = 0.005f;
            if (score > 200) gaugeRedcutionRate = 0.005f;
            if (score > 300) gaugeRedcutionRate = 0.0065f;
            if (score > 400) gaugeRedcutionRate = 0.0075f;
            gauge.fillAmount -= gaugeRedcutionRate;
        }
        Invoke("GaugeReduce", 0.01f);
    }

    IEnumerator CheckGauge()
    {
        while (gauge.fillAmount != 0)
        {
            yield return new WaitForSeconds(0.4f);
        }
        GameOver();
    }

    public void GameOver()
    {
        if (isGameOver) return; // 이미 게임 오버 상태인 경우 종료
        isGameOver = true; // 게임 오버 상태로 설정

        gauge.fillAmount = 0.0f;
        gameOver.SetActive(true);
        CheckScore();
    }

    public void homeButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void GameRetry()
    {
        SceneManager.LoadScene("MiniGame2");
    }

    public void Restart() // 일시정지 -> 게임으로 돌아가기
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
    }

    public void CheckScore()
    {
        genCoins = score / 100;
        coinsText.text = string.Format("{0:n0}", genCoins);
        Debug.Log("생성된 코인 수 : " + genCoins);
        // PlayerPrefs에 코인 수 저장
        saveCoins = PlayerPrefs.GetInt("Coins", 0) + genCoins;
        PlayerPrefs.SetInt("Coins", saveCoins);
        PlayerPrefs.Save();
    }
}
