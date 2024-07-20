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

    public GameObject myItemUI;
    public GameObject drawItemUI;
    

    void Awake()
    {
        
        
    }

    void Update()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = string.Format("{0:n0}", coins);
    }

    // #.Main Scene
    public void Mini1gameStart()
    {
        SceneManager.LoadScene("MiniGame1");
    }

    public void Mini2gameStart()
    {
        SceneManager.LoadScene("MiniGame2");
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

    // #.closet Scene
    public void SelectingItem()
    {
        myItemUI.SetActive(true);
    }

    public void CancleSelectingItem()
    {
        myItemUI.SetActive(false);
    }

    public void DrawItem()
    {
        drawItemUI.SetActive(true);
    }
    public void CancleDrawItem()
    {
        drawItemUI.SetActive(false);
    }

    public void homeButton()
    {
        SceneManager.LoadScene("Main");
    }
}
