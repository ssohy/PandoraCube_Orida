using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject gameUI;
    public int coins;
    public TMP_Text coinsText;


    void Awake()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = string.Format("{0:n0}", coins);
    }
    public void Mini1gameStart()
    {
        SceneManager.LoadScene("MiniGame1");
    }

    public void Mini2gameStart()
    {
        SceneManager.LoadScene("MiniGame1");
    }

    public void ClosetLoad()
    {
        SceneManager.LoadScene("Closet");
    }
    public void GameUIstart()
    {
        gameUI.SetActive(true);
    }
    public void GameUIcancle()
    {
        gameUI.SetActive(false);
    }
}
